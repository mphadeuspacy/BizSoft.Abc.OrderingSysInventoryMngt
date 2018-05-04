using System;
using System.Collections.Generic;
using System.Reflection;

namespace BizSoft.Ordering.Core.SeedWork.Concretes
{
    public abstract class Enumeration : IComparable
    {
        public string Name { get; }
        public int Id { get; }

        protected Enumeration()
        {
        }

        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => Name;

        public int CompareTo(object otherObject) => Id.CompareTo(((Enumeration) otherObject).Id);

        public static IEnumerable<TEntity> GetAll<TEntity>() where TEntity : Enumeration, new()
        {
            var fields = typeof( TEntity ).GetFields( BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly );

            foreach (var info in fields)
            {
                var instance = new TEntity();

                if (info.GetValue( instance ) is TEntity locatedValue) yield return locatedValue;
            }
        }
    }
}
