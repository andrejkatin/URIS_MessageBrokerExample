using MessageBrokerBus.ConsumerInterface;
using Newtonsoft.Json.Linq;
using SocketHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using URIS_PackageService.Data;
using URIS_PackageService.Models;

namespace URIS_PackageService
{
    public class PackageService : IConsumerInterface
    {
        public string ConsumerName => "PackageService";

        public string TopicName => "Package";

        private Thread ProcessThread { get; set; }

        private SocketTemplate Socket { get; set; }

        public PackageService()
        {
            Start();
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

        public void ProcessMessage(string message)
        {
            JObject json = JObject.Parse(message);
            try
            {
                Package package = new Package(json);
                PackageRepository.Packages.Add(package);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Given order is in wrong format");
            }
        }
    }
}
