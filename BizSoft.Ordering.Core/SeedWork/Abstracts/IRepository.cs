namespace BizSoft.Ordering.Core.SeedWork.Abstracts
{
    public interface IRepository<TEntity> where TEntity : IEntityRoot
    {
        IDbContextPersister DbContextPersister { get; }
    }
}
