using System;
using ProReLe.Domain.Entities;

namespace ProReLe.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> Queryable {get;}
                
        TEntity? GetById(int id);
        void Insert(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
    }
}