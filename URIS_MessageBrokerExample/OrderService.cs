using MessageBrokerBus.Abstraction;
using MessageBrokerBus.ConsumerInterface;
using Newtonsoft.Json.Linq;
using SocketHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using URIS_OrderService.Data;
using URIS_OrderService.Models;

namespace URIS_OrderService
{
    public class OrderService : IConsumerInterface
    {
        public string ConsumerName => "OrderService";

        public string TopicName => "Order";

        private Thread ProcessThread { get; set; }

        private SocketTemplate Socket { get; set; }

        public OrderService()
        {
            Start();
        }

        public void Start()
        {
            try
            {
                ProcessThread = new Thread(new ThreadStart(Process));
                ProcessThread.Start();
            }
            catch (ThreadStartException ex)
            {
                ProcessThread.Abort();
                Console.WriteLine(ex);
            }
        }

        private void Process()
        {
            if (TopicName == "" || ConsumerName == "")
            {
                Stop();
                throw new Exception("Topic and Group must be declared!");
            }
            string socketAddress = $"ws://localhost:50872/ws?topic={ TopicName }&group={ ConsumerName }";
            Socket = new SocketTemplate(socketAddress);
            Socket.MessageReceived += Socket_MessageReceived;
            Socket.Connect();
            Console.WriteLine(ConsumerName + " has connected on topic: " + TopicName);
            Socket.ReadData();
        }

        private void Socket_MessageReceived(object sender, string message)
        {
            try
            {
                ProcessMessage(message);
            }
            catch (Exception ex)
            {
                Socket.Disconnect();
                Console.WriteLine(ex);
                Console.WriteLine(ConsumerName + "has disconnected on topic");
            }
        }

        public void Stop()
        {
            Socket.Disconnect();
            Console.WriteLine(ConsumerName + " has disconnected on topic");
        }

        public void ProcessMessage(string message)
        {
            JObject json = JObject.Parse(message);
            try
            {
                Order order = new Order(json);
                OrderRepository.Orders.Add(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Given order is in wrong format");
            }
        }
    }
}
