using System;
using ProReLe.Domain.Entities;
using ProReLe.Domain.Interfaces.Repositories;

namespace ProReLe.Data.Persistence.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private ProReLeContext _context;

        public BaseRepository(ProReLeContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> Queryable => _context.Set<TEntity>().AsQueryable();

        public virtual TEntity? GetById(int id)
        {
            var entity = _context.Set<TEntity>().Find(id);
            return entity;
        }

        public virtual void Insert(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public virtual TEntity Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            return entity;
        }

        public virtual void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }
    }
}