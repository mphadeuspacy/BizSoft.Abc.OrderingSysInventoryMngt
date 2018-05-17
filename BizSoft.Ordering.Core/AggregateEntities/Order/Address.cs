﻿using System;
using System.Collections.Generic;
using BizSoft.Ordering.Core.SeedWork.Abstracts;

namespace BizSoft.Ordering.Core.AggregateEntities.Address
{
    public class Address : ValueObject
    {
        public String Street { get; }
        public String City { get; }
        public String State { get; }
        public String Country { get; }
        public String ZipCode { get; }
        
        public Address( string street, string city, string state, string country, string zipcode )
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipcode;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            // Using a yield return statement to return each element one at a time
            yield return Street;
            yield return City;
            yield return State;
            yield return Country;
            yield return ZipCode;
        }
    }
}
