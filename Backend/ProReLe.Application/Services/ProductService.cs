using System;
using ProReLe.Application.Interfaces.Services;
using ProReLe.Application.Response;
using ProReLe.Data.Persistence.Configurations;
using ProReLe.Domain.Entities;
using ProReLe.Domain.Interfaces.OuW;

namespace ProReLe.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public BaseResponse Include(Product entity)
        {
            if (entity.Id > 0)
            {
                return new BaseResponse(false, "Invalid product id!");
            }

            if (string.IsNullOrWhiteSpace(entity.Description) 
                || entity.Description.Length < ProductConfiguration.DESCRIPTION_MIN_LENGTH 
                || entity.Description.Length > ProductConfiguration.DESCRIPTION_MAX_LENGTH)
            {
                return new BaseResponse(false, $"Invalid description. The description must have between {ProductConfiguration.DESCRIPTION_MIN_LENGTH} and {ProductConfiguration.DESCRIPTION_MAX_LENGTH} characters.");
            }

            if (entity.Price < 0)
            {
                return new BaseResponse(false, "Invalid price. The price must be greater than zero!");
            }

            if (entity.Amount < 0)
            {
                return new BaseResponse(false, "Invalid amount. The amount must be greater than zero!");
            }

            _unitOfWork.ProductRepository.Insert(entity);
            _unitOfWork.Commit();

            return new BaseResponse(true, "Product inserted successfully!");
        }

         public BaseResponse Update(Product entity)
        {
            var record = _unitOfWork.ProductRepository.GetById(entity.Id);
            if (record is null)
            {
                return new BaseResponse(false, "The record was not found");
            }

            if (string.IsNullOrWhiteSpace(entity.Description) 
                || entity.Description.Length < ProductConfiguration.DESCRIPTION_MIN_LENGTH 
                || entity.Description.Length > ProductConfiguration.DESCRIPTION_MAX_LENGTH)
            {
                return new BaseResponse(false, $"Invalid description. The description must have between {ProductConfiguration.DESCRIPTION_MIN_LENGTH} and {ProductConfiguration.DESCRIPTION_MAX_LENGTH} characters.");
            }

            if (entity.Price < 0)
            {
                return new BaseResponse(false, "Invalid price. The price must be greater than zero!");
            }

            if (entity.Amount < 0)
            {
                return new BaseResponse(false, "Invalid amount. The amount must be greater than zero!");
            }

            record.Description = entity.Description;
            record.Price = entity.Price;
            record.Amount = entity.Amount;

            _unitOfWork.ProductRepository.Update(record);
            _unitOfWork.Commit();

            return new BaseResponse(true, "Product updated successfully!");
        }

        public BaseResponse Delete(Product entity)
        {
            var record = _unitOfWork.ProductRepository.GetById(entity.Id);
            if (record is null)
            {
                return new BaseResponse(false, "The record was not found");
            }

            record.LogicallyExcluded = true;
            
            _unitOfWork.ProductRepository.Update(record);
            _unitOfWork.Commit();

            return new BaseResponse(true, "Product deleted successfully!");
        }
    }
}