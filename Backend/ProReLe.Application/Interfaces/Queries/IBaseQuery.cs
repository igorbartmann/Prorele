using System;
using ProReLe.Domain.Entities;

namespace ProReLe.Application.Interfaces.Queries
{
    public interface IBaseQuery<TEntity> where TEntity : BaseEntity
    {
        TEntity? GetById(int id);
        IEnumerable<TEntity> GetAll();
    }
}