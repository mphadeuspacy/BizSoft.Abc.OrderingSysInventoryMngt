using System;
using BizSoft.Ordering.Core.SeedWork.Abstracts;

namespace BizSoft.Ordering.Core.AggregateEntities.Buyer
{
    public class Buyer : Entity, IAggregateRoot
    {
        public string IdentityGuid { get; }

        public string Name { get; }

        public Buyer( string identity, string name )
        {
            IdentityGuid = !string.IsNullOrWhiteSpace( identity ) ? identity : throw new ArgumentNullException( nameof( identity ) );

            Name = !string.IsNullOrWhiteSpace( name ) ? name : throw new ArgumentNullException( nameof( name ) );
        }
    }
}

