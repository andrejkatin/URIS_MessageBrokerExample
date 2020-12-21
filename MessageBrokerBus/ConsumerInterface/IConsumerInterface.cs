using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBrokerBus.ConsumerInterface
{
    public interface IConsumerInterface
    {
        string ConsumerName { get; }

        string TopicName { get; }

        void ProcessMessage(string message);

    }
}
