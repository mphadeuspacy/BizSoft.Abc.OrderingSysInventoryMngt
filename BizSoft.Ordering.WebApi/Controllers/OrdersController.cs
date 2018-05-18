using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BizSoft.Ordering.Core.Services.Abstracts;
using BizSoft.Ordering.WebApi.Commands.Concretes;
using BizSoft.Ordering.WebApi.Queries.Abstracts;
using BizSoft.Ordering.WebApi.Queries.Concretes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BizSoft.Ordering.WebApi.Controllers
{
    [Route( "api/v1/[controller]" )]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IOrderQueries _orderQueries;
        
        public OrdersController( IMediator mediator, IOrderQueries orderQueries, IIdentityService identityService )
        {

            _mediator = mediator ?? throw new ArgumentNullException( nameof( mediator ) );

            _orderQueries = orderQueries ?? throw new ArgumentNullException( nameof( orderQueries ) );
        }

        [Route( "cancel" )]
        [HttpPut]
        [ProducesResponseType( (int)HttpStatusCode.OK )]
        [ProducesResponseType( (int)HttpStatusCode.BadRequest )]
        public async Task<IActionResult> CancelOrder( [FromBody]CancelOrderCommand command, [FromHeader( Name = "x-requestid" )] string requestId )
        {
            bool commandResult = false;

            if (Guid.TryParse( requestId, out Guid guid ) && guid != Guid.Empty)
            {
                var requestCancelOrder = new IdentifiedCommand<CancelOrderCommand, bool>( command, guid );

                commandResult = await _mediator.Send( requestCancelOrder );
            }

            return commandResult ? Ok() : BadRequest() as IActionResult;

        }

        [Route( "ship" )]
        [HttpPut]
        [ProducesResponseType( (int)HttpStatusCode.OK )]
        [ProducesResponseType( (int)HttpStatusCode.BadRequest )]
        public async Task<IActionResult> ShipOrder( [FromBody]ShipOrderCommand command, [FromHeader( Name = "x-requestid" )] string requestId )
        {
            bool commandResult = false;

            if (Guid.TryParse( requestId, out Guid guid ) && guid != Guid.Empty)
            {
                var requestShipOrder = new IdentifiedCommand<ShipOrderCommand, bool>( command, guid );

                commandResult = await _mediator.Send( requestShipOrder );
            }

            return commandResult ? Ok() : BadRequest() as IActionResult;
        }

        [Route( "{orderId:int}" )]
        [HttpGet]
        [ProducesResponseType( typeof( OrderViewModel ), (int)HttpStatusCode.OK )]
        [ProducesResponseType( (int)HttpStatusCode.NotFound )]
        public async Task<IActionResult> GetOrder( int orderId )
        {
            try
            {
                var order = await _orderQueries.GetOrderAsync( orderId );

                return Ok( order );
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Route( "" )]
        [HttpGet]
        [ProducesResponseType( typeof( IEnumerable<OrderSummaryViewModel> ), (int)HttpStatusCode.OK )]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderQueries.GetOrdersAsync();

            return Ok( orders );
        }
       

        [Route( "draft" )]
        [HttpPost]
        public async Task<IActionResult> GetOrderDraftFromBasketData( [FromBody] CreateOrderDraftCommand createOrderDraftCommand )
        {
            var draft = await _mediator.Send( createOrderDraftCommand );

            return Ok( draft );
        }
    }
}
