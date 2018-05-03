using System;
using System.Threading;
using System.Threading.Tasks;
using BizSoft.Ordering.Core.SeedWork.Abstracts;
using MediatR;

namespace Ordering.WebApi.Commands.Concretes
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;


        public Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
