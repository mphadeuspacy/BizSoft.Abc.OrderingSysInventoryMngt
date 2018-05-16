namespace BizSoft.Ordering.Core.SeedWork.Abstracts
{
    public interface IRepository<TEntity> where TEntity : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
