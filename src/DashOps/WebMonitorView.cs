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

        private string _commandId;

        public override string CommandId => _commandId ?? (_commandId = IdBuilder.BuildId(Url));

        private string BuildLogFileName(string name)
            => Logs != null ? Path.Combine(Logs, name) : null;

        private const int LOG_INDENT = 18;

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
                var logWriter = CurrentLogFile != null ? new StreamWriter(CurrentLogFile, false, Encoding.UTF8) : null;
                logWriter?.WriteLine("Url:".PadRight(LOG_INDENT) 
                                     + Url);
                logWriter?.WriteLine("Timeout:".PadRight(LOG_INDENT) 
                                     + Timeout);
                logWriter?.WriteLine("Start:".PadRight(LOG_INDENT) 
                                     + startTime.ToString(TS_FORMAT));
                logWriter?.WriteLine("--------------------------------------------------------------------------------");
                logWriter?.Flush();
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
                    logWriter?.WriteLine("Method:".PadRight(LOG_INDENT) 
                                         + response.Method);
                    logWriter?.WriteLine("Status Code:".PadRight(LOG_INDENT) 
                                         + (int)response.StatusCode + " - " + response.StatusCode);
                    logWriter?.WriteLine("Response Url:".PadRight(LOG_INDENT) 
                                         + response.ResponseUri);
                    logWriter?.WriteLine("Server:".PadRight(LOG_INDENT) 
                                         + response.Server);
                    if (!string.IsNullOrWhiteSpace(response.ContentEncoding))
                        logWriter?.WriteLine("Content Type:".PadRight(LOG_INDENT) 
                                             + response.ContentType);
                    if (response.CharacterSet != null)
                        logWriter?.WriteLine("Charset:".PadRight(LOG_INDENT) 
                                             + response.CharacterSet);
                    logWriter?.WriteLine("Content Encoding:".PadRight(LOG_INDENT) 
                                         + response.ContentEncoding);
                    logWriter?.WriteLine("Content Length:".PadRight(LOG_INDENT) 
                                         + response.ContentLength);
                    logWriter?.WriteLine(
                        "--------------------------------------------------------------------------------");
                    logWriter?.Flush();
                    var responseText = ReadResponseAsString(response);
                    logWriter?.WriteLine(responseText);
                    logWriter?.WriteLine(
                        "--------------------------------------------------------------------------------");
                    var endTime = DateTime.Now;
                    logWriter?.WriteLine("End:".PadRight(LOG_INDENT) 
                                         + endTime.ToString(TS_FORMAT));
                    var duration = endTime - startTime;
                    logWriter?.WriteLine("Duration:".PadRight(LOG_INDENT) 
                                         + duration);
                    if (!StatusCodes.Contains((int)response.StatusCode))
                    {
                        logWriter?.WriteLine("Error:".PadRight(LOG_INDENT) 
                                             + "Status code not allowed: " + string.Join(", ", StatusCodes));
                        return (int)response.StatusCode;
                    }
                    if (!RequiredPatterns.All(p => p.IsMatch(responseText)))
                    {
                        logWriter?.WriteLine("Error:".PadRight(LOG_INDENT) 
                                             + "Required pattern did not match");
                        return 2;
                    }
                    if (ForbiddenPatterns.Any(p => p.IsMatch(responseText)))
                    {
                        logWriter?.WriteLine("Error:".PadRight(LOG_INDENT) 
                                             + "Forbidden pattern did match");
                        return 3;
                    }
                    return 0;
                }
                catch (Exception e)
                {
                    logWriter?.WriteLine("Exception:   " + e);
                    return 1;
                }
                finally
                {
                    response?.Close();
                    logWriter?.Close();
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
            using (var r = new StreamReader(ms, encoding,
                detectEncodingFromByteOrderMarks: true,
                bufferSize: 1024,
                leaveOpen: true))
            {
                text = r.ReadToEnd();
            }
            if (response.ContentType != "text/html") return text;
            var charsetPos = text.IndexOf("charset", StringComparison.Ordinal);
            if (charsetPos < 0) return text;
            charsetPos += "charset".Length + 1;
            var charsetEnd = text.IndexOfAny(new[] { ' ', '\"', ';' }, charsetPos);
            if (charsetEnd < charsetPos) return text;
            var charset = text.Substring(charsetPos, charsetEnd - charsetPos);
            if (response.CharacterSet == charset) return text;
            encoding = Encoding.GetEncoding(charset);
            ms.Seek(0, SeekOrigin.Begin);
            using (var r = new StreamReader(ms, encoding,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: false))
            {
                text = r.ReadToEnd();
            }
            return text;
        }

        private void FinalizeLogFile(int exitCode)
        {
            if (CurrentLogFile == null) return;
            var logFile = BuildLogFileName(LogFileManager.FinalizeLogFileName(CurrentLogFile, exitCode));
            if (LogFileManager.WaitForFileAccess(CurrentLogFile))
            {
                File.Move(CurrentLogFile, logFile);
                CurrentLogFile = logFile;
                OnLogIconChanged();
            }
            else
            {
                CurrentLogFile = null;
            }
        }

        protected override void NotifyExecutionFinished(bool success)
        {
            base.NotifyExecutionFinished(success);
            if (!HasExecutionResultChanged)
            {
                File.Delete(CurrentLogFile);
                CurrentLogFile = null;
            }
        }
    }
}
