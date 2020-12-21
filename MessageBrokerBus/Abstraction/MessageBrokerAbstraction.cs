using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBrokerBus.Abstraction
{
    public abstract class MessageBrokerAbstraction : IMessageService
    {
        protected string ConsumerID { get; set; }
        protected string Group { get; set; }
        protected bool Eof { get; set; }
        protected string Topic { get; set; }

        public MessageBrokerAbstraction(string consumerID, string group, bool eof)
        {
            ConsumerID = consumerID;
            Group = group;
            Eof = eof;
        }

        public MessageBrokerAbstraction(Guid consumerID, string group, bool eof)
        {
            ConsumerID = consumerID.ToString();
            Group = group;
            Eof = eof;
        }

        public MessageBrokerAbstraction(Guid consumerID, string topic, string group)
        {
            ConsumerID = consumerID.ToString();
            Group = group;
        }

        public abstract event IMessageService.AsyncEvent MessageArrived;

        public abstract void Publish(string messageContent, string topic);
        public abstract List<string> Consume(string topic, string group);
        public abstract void Subscribe(string topic, string group);
        public abstract void Unsubscribe();
    }
}
