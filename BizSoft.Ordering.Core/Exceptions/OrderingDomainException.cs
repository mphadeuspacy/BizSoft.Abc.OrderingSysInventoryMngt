using System;

namespace BizSoft.Ordering.Core.Exceptions
{
    public class OrderingDomainException : Exception
    {
        public OrderingDomainException(string excepetionMessage) : base(excepetionMessage) { }
    }
}
