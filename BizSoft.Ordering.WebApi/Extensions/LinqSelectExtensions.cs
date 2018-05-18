using System;
using System.Collections.Generic;
using System.Linq;

namespace BizSoft.Ordering.WebApi.Extensions
{
    public static class LinqSelectExtensions
    {
        public static IEnumerable<SelectTryResult<TSource, TResult>> SelectTry<TSource, TResult>( this IEnumerable<TSource> enumerable, Func<TSource, TResult> selector )
        {
            foreach (TSource element in enumerable)
            {
                SelectTryResult<TSource, TResult> returnedValue;
                try
                {
                    returnedValue = new SelectTryResult<TSource, TResult>( element, selector( element ), null );
                }
                catch (Exception ex)
                {
                    returnedValue = new SelectTryResult<TSource, TResult>( element, default( TResult ), ex );
                }
                yield return returnedValue;
            }
        }

        public static IEnumerable<TResult> OnCaughtException<TSource, TResult>( this IEnumerable<SelectTryResult<TSource, TResult>> enumerable, Func<Exception, TResult> exceptionHandler )
        {
            return enumerable.Select( x => x.CaughtException == null ? x.Result : exceptionHandler( x.CaughtException ) );
        }

        public class SelectTryResult<TSource, TResult>
        {
            internal SelectTryResult( TSource source, TResult result, Exception exception )
            {
                Result = result;
                CaughtException = exception;
            }

            public TResult Result { get; }
            public Exception CaughtException { get; }
        }
    }
}
