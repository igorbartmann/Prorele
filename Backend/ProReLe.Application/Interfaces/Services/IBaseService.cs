using System;
using ProReLe.Application.Response;
using ProReLe.Domain.Entities;

namespace ProReLe.Application.Interfaces.Services
{
    public interface IBaseService<TEntity> where TEntity : BaseEntity
    {
        BaseResponse Include(TEntity entity);
        BaseResponse Update(TEntity entity);
        BaseResponse Delete(TEntity entity);
    }
}