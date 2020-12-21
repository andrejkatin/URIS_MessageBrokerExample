using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace SocketHandler
{
    public class SocketTemplate
    {
        public event EventHandler<string> MessageReceived;
        private static object consoleLock = new object();
        private const int receiveChunkSize = 1024;

        private string uri = "";
        private ClientWebSocket WebSocket { get; set; }

        public SocketTemplate(string uri)
        {
            this.uri = uri;
            WebSocket = new ClientWebSocket();
        }

        public void Connect()
        {
            try
            {
                WebSocket.ConnectAsync(new Uri(uri), CancellationToken.None).GetAwaiter().GetResult();
            }
            catch (WebSocketException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex);
                throw ex;
            }
        }

        public void ReadData()
        {
            Receive();
        }

        public void Disconnect()
        {
            try
            {
                WebSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None).GetAwaiter().GetResult();

                if (WebSocket != null)
                {
                    WebSocket.Abort();
                    WebSocket.Dispose();
                    Console.WriteLine();

                    lock (consoleLock)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("WebSocket closed.");
                        Console.ResetColor();
                    }
                }
            }
            catch
            {
                if (WebSocket != null)
                {
                    WebSocket.Dispose();
                }

                lock (consoleLock)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("WebSocket has closed.");
                    Console.ResetColor();
                }
            }
        }

        private void Receive()
        {
            // Velicina buffera se proizvoljno definise
            byte[] buffer = new byte[receiveChunkSize];
            var offset = 0;
            var free = buffer.Length;
            while (WebSocket.State == WebSocketState.Open)
            {
                try
                {
                    WebSocketReceiveResult result = WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer, offset, free), CancellationToken.None).GetAwaiter().GetResult();
                    offset += result.Count;
                    free -= result.Count;
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        Disconnect();
                    }
                    // Ukoliko je u okviru buffer-a citava poruka, vrsi se ekstrakcija poruke i njeno prosledjivanje
                    else if (result.EndOfMessage)
                    {
                        string message = encoder.GetString(buffer);
                        int length = message.Length;

                        RaiseMessageReceivedEvent(message);

                        buffer = new byte[receiveChunkSize];
                        offset = 0;
                        free = buffer.Length;
                    }
                    // Ukoliko je poruka duzine vece od velicine buffer-a, vrsi se udvostrucavanje buffera dok
                    // citava poruka ne bude smestena i obradjena
                    else
                    {
                        if (free == 0)
                        {
                            var newSize = buffer.Length * 2;

                            var newBuffer = new byte[newSize];
                            Array.Copy(buffer, 0, newBuffer, 0, offset);
                            buffer = newBuffer;
                            free = buffer.Length - offset;
                        }
                    }
                }
                catch (EncoderFallbackException ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                catch
                {
                    Disconnect();
                }
            }
        }

        protected virtual void RaiseMessageReceivedEvent(string message)
        {
            MessageReceived?.Invoke(this, message);
        }

        static UTF8Encoding encoder = new UTF8Encoding();
    }
}
