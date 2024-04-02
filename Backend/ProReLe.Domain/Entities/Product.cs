using System;

namespace ProReLe.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product(string description, decimal price, int amount) : base()
        {
            Description = description;
            Price = price;
            Amount = amount;
        }
        public Product(int id, string description, decimal price, int amount, bool logicallyExcluded) : base(id)
        {
            Description = description;
            Price = price;
            Amount = amount;
            LogicallyExcluded = logicallyExcluded;
        }

        public string Description {get;set;}
        public decimal Price {get;set;}
        public int Amount {get;set;}
        public bool LogicallyExcluded {get;set;}
    }
}