using System;
using BizSoft.EventBusServiceBus.Abstracts;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;

namespace BizSoft.EventBusServiceBus.Concretes
{
    public class DefaultServiceBusPersisterConnection : IServiceBusPersisterConnection
    {
        private readonly ServiceBusConnectionStringBuilder _serviceBusConnectionStringBuilder;
        public ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder => _serviceBusConnectionStringBuilder;

        private ITopicClient _topicClient;

        public DefaultServiceBusPersisterConnection( ServiceBusConnectionStringBuilder serviceBusConnectionStringBuilder )
        {
           _serviceBusConnectionStringBuilder = serviceBusConnectionStringBuilder ?? throw new ArgumentNullException( nameof( serviceBusConnectionStringBuilder ) );
            _topicClient = new TopicClient( _serviceBusConnectionStringBuilder, RetryPolicy.Default );
        }

        public ITopicClient CreateModel()
        {
            if (_topicClient.IsClosedOrClosing)
            {
                _topicClient = new TopicClient( _serviceBusConnectionStringBuilder, RetryPolicy.Default );
            }

            return _topicClient;
        }
    }
}
