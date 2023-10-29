using LibraryManagementAPI.Data.Entities;
using LibraryManagementAPI.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public virtual Task<List<TEntity>> GetAsync()
        {
            return DbSet.AsNoTracking().ToListAsync();
        }

        public virtual Task<TEntity?> GetByIdAsync(int? id)
        {
            var taskEntity = DbSet.AsNoTracking().FirstOrDefaultAsync(_ => _.Id == id);
            return taskEntity;
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            var createdBookEntity = DbSet.Add(entity).Entity;
            await Context.SaveChangesAsync();

            return createdBookEntity;
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;

            await Context.SaveChangesAsync();
        }

        public virtual async Task<bool> DeleteAsync(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }

            DbSet.Remove(entityToDelete);

            var affectedEntities = await Context.SaveChangesAsync();
            return affectedEntities > 0;
        }
    }
}
