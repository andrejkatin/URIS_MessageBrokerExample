using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MessageBrokerBus.Abstraction
{
    public interface IMessageService
    {
        public delegate Task AsyncEvent(object sender, string message);
        public event AsyncEvent MessageArrived;

        public void Publish(string message, string topic);
        public List<string> Consume(string topic, string group);
        public void Subscribe(string topic, string group);
        public void Unsubscribe();
    }
}
