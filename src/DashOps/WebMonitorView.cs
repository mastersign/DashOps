using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.DashOps
{
    partial class WebMonitorView
    {
        private const string TS_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public override string CommandId => IdBuilder.BuildId(Url);

        private string BuildLogFileName(string name)
            => Logs != null ? Path.Combine(Logs, name) : null;

        public override Task<bool> Check(DateTime startTime)
        {
            if (Logs != null && !Directory.Exists(Logs))
            {
                Directory.CreateDirectory(Logs);
            }
            NotifyExecutionBegin(startTime);
            var tWebRequest = new Task<int>(() =>
            {
                CurrentLogFile = BuildLogFileName(this.PreliminaryLogFileName(startTime));
                throw new NotImplementedException("Missing null check");
                var logWriter = new StreamWriter(CurrentLogFile, false, Encoding.UTF8);
                logWriter.WriteLine("Url:            " + Url);
                logWriter.WriteLine("Timeout:        " + Timeout);
                logWriter.WriteLine("Start:          " + startTime.ToString(TS_FORMAT));
                logWriter.WriteLine("--------------------------------------------------------------------------------");
                logWriter.Flush();
                HttpWebResponse response = null;
                try
                {
                    var wr = WebRequest.CreateHttp(Url);
                    wr.KeepAlive = false;
                    if (Headers != null)
                    {
                        foreach (var headerKey in Headers.Keys)
                        {
                            wr.Headers.Set(headerKey, Headers[headerKey]);
                        }
                    }

                    wr.Method = WebRequestMethods.Http.Get;
                    wr.Timeout = (int)Timeout.TotalMilliseconds;

                    response = (HttpWebResponse)wr.GetResponse();
                    logWriter.WriteLine("Method:         " + response.Method);
                    logWriter.WriteLine("Status Code:    " + (int)response.StatusCode + " - " + response.StatusCode);
                    logWriter.WriteLine("Response Url:   " + response.ResponseUri);
                    logWriter.WriteLine("Server:         " + response.Server);
                    logWriter.WriteLine("Content Type:   " + response.ContentType);
                    logWriter.WriteLine("Content Length: " + response.ContentLength);
                    logWriter.WriteLine(
                        "--------------------------------------------------------------------------------");
                    logWriter.Flush();
                    var responseText = ReadResponseAsString(response);
                    logWriter.WriteLine(responseText);
                    logWriter.WriteLine(
                        "--------------------------------------------------------------------------------");
                    var duration = DateTime.Now - startTime;
                    logWriter.WriteLine("Duration:       " + duration);
                    if (!StatusCodes.Contains((int)response.StatusCode))
                    {
                        logWriter.WriteLine("Error:       Status code not allowed: " + string.Join(", ", StatusCodes));
                        return (int)response.StatusCode;
                    }
                    if (!RequiredPatterns.All(p => p.IsMatch(responseText)))
                    {
                        logWriter.WriteLine("Error:       Required pattern did not match");
                        return 2;
                    }
                    if (ForbiddenPatterns.Any(p => p.IsMatch(responseText)))
                    {
                        logWriter.WriteLine("Error:       Forbidden pattern did match");
                        return 3;
                    }
                    return 0;
                }
                catch (Exception e)
                {
                    logWriter.WriteLine("Exception:   " + e);
                    return 1;
                }
                finally
                {
                    response?.Close();
                    logWriter.Close();
                }
            });
            var tFinalizeLog = tWebRequest.ContinueWith(t =>
            {
                FinalizeLogFile(t.Result);
                return t.Result;
            });
            var tNotify = tFinalizeLog.ContinueWith(t =>
            {
                var success = t.Result == 0;
                NotifyExecutionFinished(success);
                return success;
            });
            tWebRequest.Start();
            return tNotify;
        }

        private static string ReadResponseAsString(HttpWebResponse response)
        {
            var ms = new MemoryStream();
            using (var s = response.GetResponseStream()) s.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            var encoding = Encoding.GetEncoding(response.CharacterSet ?? "utf-8");
            string text;
            using (var r = new StreamReader(ms, encoding, true)) text = r.ReadToEnd();
            if (response.ContentType != "text/html") return text;
            var charsetPos = text.IndexOf("charset", StringComparison.Ordinal);
            if (charsetPos < 0) return text;
            charsetPos += "charset".Length;
            var charsetEnd = text.IndexOfAny(new[] { ' ', '\"', ';' });
            if (charsetEnd < 0) return text;
            var charset = text.Substring(charsetPos, charsetEnd - charsetPos);
            if (response.CharacterSet == charset) return text;
            encoding = Encoding.GetEncoding(charset);
            ms.Seek(0, SeekOrigin.Begin);
            using (var r = new StreamReader(ms, encoding, false)) text = r.ReadToEnd();
            return text;
        }

        private void FinalizeLogFile(int exitCode)
        {
            if (CurrentLogFile == null) return;
            var logFile = BuildLogFileName(LogFileManager.FinalizeLogFileName(CurrentLogFile, exitCode));
            File.Move(CurrentLogFile, logFile);
            CurrentLogFile = logFile;
            OnLogIconChanged();
        }
    }
}
