using LibraryManagementAPI.Data.Entities;

namespace LibraryManagementAPI.Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<List<TEntity>> GetAsync(CancellationToken token);
        Task<TEntity?> GetByIdAsync(int? id, CancellationToken token);
        TEntity Insert(TEntity entity);
        void Update(TEntity entity);
        bool Delete(TEntity entityToDelete);
        Task SaveChangesAsync(CancellationToken token);
    }
}