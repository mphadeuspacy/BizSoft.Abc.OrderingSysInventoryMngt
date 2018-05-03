using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizSoft.Ordering.Core.SeedWork.Abstracts
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
