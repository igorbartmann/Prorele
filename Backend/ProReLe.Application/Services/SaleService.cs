using System;
using ProReLe.Application.Interfaces.Services;
using ProReLe.Application.Response;
using ProReLe.Domain.Entities;
using ProReLe.Domain.Interfaces.OuW;

namespace ProReLe.Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;

        public SaleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _productService = new ProductService(unitOfWork);
        }

        public BaseResponse Include(Sale entity)
        {
            if (entity.Id > 0)
            {
                return new BaseResponse(false, "Invalid sale id!");
            }

            if (entity.Amount < 0)
            {
                return new BaseResponse(false, $"Invalid amount. The quantity must be greater than or equal to zero!");
            }

            if (entity.Discount < 0)
            {
                return new BaseResponse(false, $"Invalid discount. The discount must be greater than or equal to zero!");
            }

            var client = _unitOfWork.ClientRepository.GetById(entity.ClientId);
            if (client is null)
            {
                return new BaseResponse(false, "The client was not found!");
            }

            var product = _unitOfWork.ProductRepository.GetById(entity.ProductId);
            if (product is null) 
            {
                return new BaseResponse(false, "The product was not found!");
            }

            if (product.Amount < entity.Amount)
            {
                return new BaseResponse(false, $"There are no '{entity.Amount}' units of product '{product.Description}' in stock!");
            }            

            product.Amount -= entity.Amount;
            var response = _productService.Update(product);
            if (!response.Success) 
            {
                return response;
            }

            var initialPriceCalculated = product.Price * entity.Amount;
            entity.InitialPrice = initialPriceCalculated;

            var finalPriceCalculated = Math.Max(initialPriceCalculated - entity.Discount, 0);
            entity.FinalPrice = finalPriceCalculated;

            var currentDate = DateTimeOffset.UtcNow;
            entity.Date = currentDate;

            _unitOfWork.SaleRepository.Insert(entity);
            _unitOfWork.Commit();

            return new BaseResponse(true, "Sale registered successfully!");
        }

         public BaseResponse Update(Sale entity)
        {
            var record = _unitOfWork.SaleRepository.GetById(entity.Id);
            if (record is null)
            {
                return new BaseResponse(false, "The record was not found");
            }

            if (entity.Discount < 0)
            {
                return new BaseResponse(false, $"Invalid discount. The discount must be greater than or equal to zero!");
            }

            record.Discount = entity.Discount;

            var finalPriceCalculated = Math.Max(record.InitialPrice - record.Discount, 0);
            record.FinalPrice = finalPriceCalculated;

            _unitOfWork.SaleRepository.Update(record);
            _unitOfWork.Commit();

            return new BaseResponse(true, "Sale updated successfully!");
        }

        public BaseResponse Delete(Sale entity)
        {
            var record = _unitOfWork.SaleRepository.GetById(entity.Id);
            if (record is null)
            {
                return new BaseResponse(false, "The record was not found");
            }

            _unitOfWork.SaleRepository.Delete(record);
            _unitOfWork.Commit();

            return new BaseResponse(true, "Sale deleted successfully!");
        }
    }
}