using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
            var tWebRequest = new Task<Tuple<bool, int>>(() =>
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
                    logWriter?.WriteLine("Content Length:".PadRight(LOG_INDENT) 
                                         + response.ContentLength);
                    if (!string.IsNullOrWhiteSpace(response.ContentEncoding))
                        logWriter?.WriteLine("Content Encoding:".PadRight(LOG_INDENT) 
                                             + response.ContentEncoding);
                    logWriter?.WriteLine("Content Type:".PadRight(LOG_INDENT) 
                                         + response.ContentType);
                    if (response.CharacterSet != null)
                        logWriter?.WriteLine("Charset:".PadRight(LOG_INDENT) 
                                             + response.CharacterSet);
                    logWriter?.WriteLine(
                        "--------------------------------------------------------------------------------");
                    logWriter?.Flush();
                    var responseText = ReadResponseAsString(response, out var detectedCharset);
                    logWriter?.WriteLine(responseText);
                    logWriter?.WriteLine(
                        "--------------------------------------------------------------------------------");
                    if (!string.IsNullOrWhiteSpace(detectedCharset))
                        logWriter?.WriteLine("Detected Charset:".PadRight(LOG_INDENT)
                                             + detectedCharset);
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
                        return Tuple.Create(false, (int)response.StatusCode);
                    }
                    if (!RequiredPatterns.All(p => p.IsMatch(responseText)))
                    {
                        logWriter?.WriteLine("Error:".PadRight(LOG_INDENT) 
                                             + "Required pattern did not match");
                        return Tuple.Create(false, (int)response.StatusCode);
                    }
                    if (ForbiddenPatterns.Any(p => p.IsMatch(responseText)))
                    {
                        logWriter?.WriteLine("Error:".PadRight(LOG_INDENT) 
                                             + "Forbidden pattern did match");
                        return Tuple.Create(false, (int)response.StatusCode);
                    }
                    return Tuple.Create(true, (int)response.StatusCode);
                }
                catch (Exception e)
                {
                    logWriter?.WriteLine("Exception:   " + e);
                    return Tuple.Create(false, 0);
                }
                finally
                {
                    response?.Close();
                    logWriter?.Close();
                }
            });
            var tFinalizeLog = tWebRequest.ContinueWith(t =>
            {
                FinalizeLogFile(t.Result.Item1, t.Result.Item2);
                return t.Result;
            });
            var tNotify = tFinalizeLog.ContinueWith(t =>
            {
                NotifyExecutionFinished(t.Result.Item1);
                return t.Result.Item1;
            });
            tWebRequest.Start();
            return tNotify;
        }

        private static string ReadResponseAsString(HttpWebResponse response, out string detectedCharset)
        {
            var ms = new MemoryStream();
            using (var s = response.GetResponseStream()) s.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            var firstTry = string.IsNullOrWhiteSpace(response.CharacterSet) ? "utf-8" : response.CharacterSet;
            Encoding encoding;
            try
            {
                encoding = Encoding.GetEncoding(firstTry);
            }
            catch (ArgumentException)
            {
                encoding = Encoding.UTF8;
            }
            detectedCharset = encoding.EncodingName;
            string text;
            using (var r = new StreamReader(ms, encoding,
                detectEncodingFromByteOrderMarks: true,
                bufferSize: 1024,
                leaveOpen: true))
            {
                text = r.ReadToEnd();
            }
            if (response.ContentType != "text/html") return text;
            var pattern = new Regex(@"charset=(""?)(?<charset>[^ ;""]+?)\1");
            var match = pattern.Match(text);
            if (!match.Success) return text;
            var charset = match.Groups["charset"].Value;
            if (response.CharacterSet == charset) return text;
            try
            {
                encoding = Encoding.GetEncoding(charset);
            }
            catch (ArgumentException)
            {
                return text;
            }
            detectedCharset = encoding.EncodingName;
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

        private void FinalizeLogFile(bool success, int exitCode)
        {
            if (CurrentLogFile == null) return;
            var logFile = BuildLogFileName(LogFileManager.FinalizeLogFileName(CurrentLogFile, success, exitCode));
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
            if (!HasExecutionResultChanged && CurrentLogFile != null && File.Exists(CurrentLogFile))
            {
                File.Delete(CurrentLogFile);
                CurrentLogFile = null;
            }
        }
    }
}
