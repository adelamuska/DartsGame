using DartsGame.Data;
using DartsGame.Entities;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Repositories
{
    public abstract class BaseRepository<T> :IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T> Create(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;

        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetById(Guid Id)
        {
            return await _dbSet.FindAsync(Id);
        }

        public virtual async Task<T> Update(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task Delete(Guid Id)
        {

            var entity = await _dbSet.FindAsync(Id);
            if (entity != null)
            {
                if(entity is ISoftDeletable softDeletable)
                {
                    softDeletable.IsDeleted = true;
               
                await _context.SaveChangesAsync();
                }
            }
        }
    }
}
