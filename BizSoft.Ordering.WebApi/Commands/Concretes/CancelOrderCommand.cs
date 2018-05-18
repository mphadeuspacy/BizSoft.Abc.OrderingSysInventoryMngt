using System.Runtime.Serialization;
using MediatR;
using Ordering.WebApi.Commands.Abstracts;

namespace BizSoft.Ordering.WebApi.Commands.Concretes
{
    public class CancelOrderCommand : ICommand, IRequest<bool>
    {
        [DataMember]
        public int OrderNumber { get; private set; }

        public CancelOrderCommand( int orderNumber )
        {
            OrderNumber = orderNumber;
        }
    }
}
