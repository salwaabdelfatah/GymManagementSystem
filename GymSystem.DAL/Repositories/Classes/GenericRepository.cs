using GymSystem.DAL.Data.DbContexts;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        

        public GenericRepository(GymDbContext dbContext) {
            this._dbContext = dbContext;
            _dbSet =_dbContext.Set<TEntity>();
        }
        public void AddAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbSet.Add(entity);
         
        }

        public
            Task<bool> AnyAsync(Expression<Func<TEntity, bool>> Perdicit, CancellationToken ct = default)
        {
            return _dbSet.AsNoTracking().AnyAsync(Perdicit, ct);
        }

        public void DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            
        }

        public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> Perdicit, bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _dbSet : _dbSet.AsNoTracking();
            return query.FirstOrDefaultAsync(Perdicit, ct);
        }



        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _dbSet : _dbSet.AsNoTracking();
            return await query.ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.FindAsync(id,ct);
        }

        public void UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            
        }
    }
}
