using LibraryManagementAPI.Data.Entities;
using LibraryManagementAPI.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Data.Repositories
{
    public abstract class GenericRepository<TEntity> : IRepository<TEntity>
     where TEntity : BaseEntity
    {
        protected readonly LibraryContext Context;
        protected readonly DbSet<TEntity> DbSet;
        protected GenericRepository(LibraryContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }
        public virtual Task<List<TEntity>> GetAsync(CancellationToken cancellationToken)
        {
            return DbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public virtual Task<TEntity?> GetByIdAsync(int? id, CancellationToken cancellationToken)
        {
            var taskEntity = DbSet.AsNoTracking().FirstOrDefaultAsync(_ => _.Id == id, cancellationToken);
            return taskEntity;
        }

        public virtual TEntity Insert(TEntity entity)
        {
            var createdBookEntity = DbSet.Add(entity).Entity;
            return createdBookEntity;
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual bool Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }

            DbSet.Remove(entityToDelete);

            return true;
        }

        public virtual async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }
    }
}