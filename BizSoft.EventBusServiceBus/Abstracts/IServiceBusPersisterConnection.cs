using Microsoft.Azure.ServiceBus;

namespace BizSoft.EventBusServiceBus.Abstracts
{
    public interface IServiceBusPersisterConnection
    {
        ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }

        ITopicClient CreateModel();
    }
}
