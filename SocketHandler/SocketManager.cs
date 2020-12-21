using KafkaMessageBus;
using MessageBrokerBus.Abstraction;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketHandler
{
    public class SocketManager
    {
        private WebSocket Socket;
        public static IMessageService MessageService { get; set; }

        public SocketManager()
        {

        }


        // Metoda kreira na osnovu prosledjenog WebSocketa odgovarajuci objekat klase MessageService i
        // na taj nacin obezbedjuje funkcionalnosti rada sa Kafkom
        public void AddSocket(string topic, string group, WebSocket socket)
        {
            Guid consumerID = Guid.NewGuid();
            MessageService = new MessageService(consumerID, group, false);
            Socket = socket;
            // Na osnovu delegata navodi povezuje se metoda kojom ce se osluskivati dogadjaj i poruka 
            // asinhrono poslati Kafki
            MessageService.MessageArrived += SendMessage;
            // Consumer kreiran putem Socket-a odmah se povezuje na odgovarajuci Topic i Particiju
            MessageService.Subscribe(topic, group);

            try
            {
                WebSocketReceiveResult result = socket.ReceiveAsync(new ArraySegment<byte>(new byte[4096]), CancellationToken.None).GetAwaiter().GetResult();

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    MessageService.Unsubscribe();
                }
            }
            catch
            {
                MessageService.Unsubscribe();
            }
        }

        public async Task SendMessage(object sender, string message)
        {
            if (Socket != null)
            {
                await SendMessageAsync(Socket, message);
            }
        }

        private static async Task SendMessageAsync(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
            {
                return;
            }

            await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
                                                                    offset: 0,
                                                                    count: message.Length),
                                    messageType: WebSocketMessageType.Text,
                                    endOfMessage: true,
                                    cancellationToken: CancellationToken.None);
        }
    }
}
