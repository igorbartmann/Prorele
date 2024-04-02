using System;

namespace ProReLe.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public Sale(int productId, int clientId, int amount, decimal discount) : base()
        {
            ProductId = productId;
            ClientId = clientId;
            Amount = amount;
            Discount = discount;
        }

        public Sale(int id, int productId, int clientId, int amount, decimal initialPrice, decimal discount, decimal finalPrice, DateTimeOffset date) : base(id)
        {
            ProductId = productId;
            ClientId = clientId;
            Amount = amount;
            InitialPrice = initialPrice;
            Discount = discount;
            FinalPrice = finalPrice;
            Date = date;
        }

        public int ProductId {get;set;}
        public int ClientId {get;set;}
        public int Amount {get;set;}
        public decimal InitialPrice {get;set;}
        public decimal Discount {get;set;}
        public decimal FinalPrice {get;set;}
        public DateTimeOffset Date {get;set;}

        #region Navigation Properties.
        public virtual Product? Product {get;set;}
        public virtual Client? Client {get;set;}
        #endregion
    }
}