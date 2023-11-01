using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity>
        where TEntity : BaseEntity

    {
        private readonly HrMeContext _context;

        protected BaseRepository(HrMeContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        protected IQueryable<TEntity> Query
        {
            get { return _context.Set<TEntity>(); }
        }

        protected IQueryable<TEntity> GetByIdQuery(Guid Id)
        {
            return _context.Set<TEntity>()
                .Where(x => x.Id == Id);
        }

        protected async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Set<TEntity>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        protected async Task Insert(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await SaveChangesAsync();
        }

        protected async Task SaveChangesAsync()
        {
           await _context.SaveChangesAsync();
        }

        protected async Task<bool> EntityExistAsync(Guid id)
        {
            return await _context.Set<TEntity>()
                .AnyAsync(e => e.Id == id);
        }

        protected async Task DeleteEntity(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await SaveChangesAsync();  
        }
    }
}

