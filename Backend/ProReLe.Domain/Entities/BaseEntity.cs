using System;

namespace ProReLe.Domain.Entities
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            Id = 0;
        }

        public BaseEntity(int id)
        {
            Id = id;
        }

        public int Id {get;set;}
    }
}