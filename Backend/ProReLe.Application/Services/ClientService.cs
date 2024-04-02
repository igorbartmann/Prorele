using System;
using ProReLe.Application.Interfaces.Queries;
using ProReLe.Application.Interfaces.Services;
using ProReLe.Application.Queries;
using ProReLe.Application.Response;
using ProReLe.Data.Persistence.Configurations;
using ProReLe.Domain.Entities;
using ProReLe.Domain.Interfaces.OuW;

namespace ProReLe.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientQuery _clientQuery;

        public ClientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _clientQuery = new ClientQuery(_unitOfWork);
        }

        public BaseResponse Include(Client entity)
        {
            if (entity.Id > 0)
            {
                return new BaseResponse(false, "Invalid client id!");
            }

            if (string.IsNullOrWhiteSpace(entity.Name)
                || entity.Name.Length < ClientConfiguration.NAME_MIN_LENGTH
                || entity.Name.Length > ClientConfiguration.NAME_MAX_LENGTH)
            {
                return new BaseResponse(false, $"Invalid name. The name must have between {ClientConfiguration.NAME_MIN_LENGTH} and {ClientConfiguration.NAME_MAX_LENGTH} characters.");
            }

            if (string.IsNullOrWhiteSpace(entity.Cpf)
                || entity.Cpf.Length < ClientConfiguration.CPF_LENGTH)
            {
                return new BaseResponse(false, "Invalid client CPF!");
            }

            var alreadyRegisteredClient = _clientQuery.GetByCpf(entity.Cpf);
            if (alreadyRegisteredClient is not null)
            {
                return new BaseResponse(false, "There is already a registered client with this CPF!");
            }

            _unitOfWork.ClientRepository.Insert(entity);
            _unitOfWork.Commit();

            return new BaseResponse(true, "Client inserted successfully!");
        }

         public BaseResponse Update(Client entity)
        {
            var record = _unitOfWork.ClientRepository.GetById(entity.Id);
            if (record is null)
            {
                return new BaseResponse(false, "The record was not found");
            }

            if (string.IsNullOrWhiteSpace(entity.Name)
                || entity.Name.Length < ClientConfiguration.NAME_MIN_LENGTH
                || entity.Name.Length > ClientConfiguration.NAME_MAX_LENGTH)
            {
                return new BaseResponse(false, $"Invalid name. The name must have between {ClientConfiguration.NAME_MIN_LENGTH} and {ClientConfiguration.NAME_MAX_LENGTH} characters.");
            }

            record.Name = entity.Name;

            _unitOfWork.ClientRepository.Update(record);
            _unitOfWork.Commit();

            return new BaseResponse(true, "Client updated successfully!");
        }

        public BaseResponse Delete(Client entity)
        {            
            var record = _unitOfWork.ClientRepository.GetById(entity.Id);
            if (record is null)
            {
                return new BaseResponse(false, "The record was not found");
            }

            record.LogicallyExcluded = true;
            
            _unitOfWork.ClientRepository.Update(record);
            _unitOfWork.Commit();

            return new BaseResponse(true, "Client deleted successfully!");
        }
    }
}