using System;
using MediatR;
using Ordering.WebApi.Commands.Abstracts;

namespace BizSoft.Ordering.WebApi.Commands.Concretes
{
    public class IdentifiedCommand<T, R> : ICommand, IRequest<R> where T : IRequest<R>
    {
        public T Command { get; }
        public Guid Id { get; }
        public IdentifiedCommand( T command, Guid id )
        {
            Command = command;
            Id = id;
        }
    }
}
