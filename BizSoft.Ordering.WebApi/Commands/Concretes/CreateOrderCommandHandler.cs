using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BizSoft.Ordering.Core.AggregateEntities.Address;
using BizSoft.Ordering.Core.Entities.Order;
using BizSoft.Ordering.Core.SeedWork.Abstracts;
using BizSoft.Ordering.Core.Services.Abstracts;
using MediatR;

namespace Ordering.WebApi.Commands.Concretes
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;

        public CreateOrderCommandHandler
        (
            IMediator mediator,
            IOrderRepository orderRepository,
            IIdentityService identityService
        )
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof( orderRepository ) );

            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }
        
        public async Task<bool> Handle(CreateOrderCommand createOrderCommand, CancellationToken cancellationToken)
        {
            var order = new Order
            ( 
                userId : createOrderCommand.UserId, 
                buyerId: null,
                address: new Address
                (
                    street: createOrderCommand.Street, 
                    city: createOrderCommand.City, 
                    state:  createOrderCommand.State, 
                    country:  createOrderCommand.Country, 
                    zipcode: createOrderCommand.ZipCode
                )
            );

            foreach (var orderItemDto in createOrderCommand.OrderItems)
            {
                order.AddOrderItem
                (
                    orderItemDto.ProductId,
                    orderItemDto.ProductName,
                    orderItemDto.Price,
                    orderItemDto.ImageUri,
                    orderItemDto.NumberOfItems
                );
            }

            _orderRepository.Create(order);

            return await _orderRepository.UnitOfWork.CommitAsync(cancellationToken);
        }
    }
}
