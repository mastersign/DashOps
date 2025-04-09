using System.IO;
using System.IO.Pipes;

namespace Mastersign.DashOps
{
    internal delegate void InterProcessDataHandler(string streamName, byte[] data);

    internal class InterProcessConnector : IDisposable
    {
        private class Session : IDisposable
        {
            private readonly NamedPipeServerStream stream;

            private readonly InterProcessDataHandler dataHandler;

            private static readonly TimeSpan CONNECT_TIMEOUT = TimeSpan.FromSeconds(5);
            private static readonly TimeSpan READ_TIMEOUT = TimeSpan.FromSeconds(5);

            private const int CHUNK_SIZE = 1024;
            private readonly List<byte[]> chunks = [];

            public string PipeName { get; private set; }

            public Session(InterProcessDataHandler dataHandler)
            {
                this.dataHandler = dataHandler;

                PipeName = $"DashOps_{Guid.NewGuid()}";
                stream = new NamedPipeServerStream(
                    PipeName, PipeDirection.InOut,
                    maxNumberOfServerInstances: 1,
                    PipeTransmissionMode.Byte,
                    PipeOptions.Asynchronous,
                    inBufferSize: CHUNK_SIZE, 
                    outBufferSize: CHUNK_SIZE);
                var worker = new Thread(ReadToEnd) { Name = "Interprocess Worker" };
                worker.Start();
            }

            private void ReadToEnd()
            {
                var buffer = new byte[CHUNK_SIZE];
                var timeoutTokenSource = new CancellationTokenSource();
                timeoutTokenSource.CancelAfter(CONNECT_TIMEOUT);
                try
                {
                    stream.WaitForConnectionAsync(timeoutTokenSource.Token).Wait();
                }
                catch (AggregateException e)
                {
                    if (e.InnerException is OperationCanceledException)
                    {
                        NotifyHandler();
                        return;
                    }
                    throw;
                }
                finally
                {
                    timeoutTokenSource.Dispose();
                }
                var timeout = DateTime.Now + READ_TIMEOUT;
                while (DateTime.Now <= timeout)
                {
                    var result = stream.Read(buffer, 0, CHUNK_SIZE);
                    if (result == 0)
                    {
                        if (chunks.Count == 0)
                            Thread.Sleep(100);
                        else
                            break;
                    }
                    else if (result == buffer.Length)
                    {
                        chunks.Add(buffer);
                    }
                    else
                    {
                        var chunk = new byte[result];
                        Array.Copy(buffer, 0, chunk, 0, result);
                        chunks.Add(chunk);
                    }
                }
                NotifyHandler();
            }

            private void NotifyHandler()
            {
                var data = new byte[chunks.Sum(chunk => chunk.Length)];
                var offset = 0;
                foreach (var chunk in chunks)
                {
                    Array.Copy(chunk, 0, data, offset, chunk.Length);
                    offset += chunk.Length;
                }
                dataHandler(PipeName, data);
                chunks.Clear();
            }

            public void Dispose()
            {
                stream.Dispose();
            }
        }

        private readonly Dictionary<string, Session> sessions = [];

        public bool IsDisposed { get; private set; }

        public string StartSession(InterProcessDataHandler dataHandler)
        {
            if (IsDisposed) throw new ObjectDisposedException(GetType().FullName);
            var session = new Session(dataHandler);
            lock (sessions)
            {
                sessions.Add(session.PipeName, session);
            }
            return session.PipeName;
        }

        public void KillSession(string pipeName)
        {
            Session session = null;
            lock (sessions)
            {
                if (sessions.TryGetValue(pipeName, out session))
                {
                    sessions.Remove(pipeName);
                }
            }
            session?.Dispose();
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            var currentSessions = new List<Session>();
            lock (sessions)
            {
                currentSessions.AddRange(sessions.Values);
                sessions.Clear();
            }
            foreach (var session in currentSessions)
            {
                session.Dispose();
            }
        }
    }
}
