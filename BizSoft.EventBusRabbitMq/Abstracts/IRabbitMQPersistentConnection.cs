using System;
using RabbitMQ.Client;

namespace BizSoft.EventBusRabbitMq.Abstracts
{
    public interface IRabbitMqPersistentConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
