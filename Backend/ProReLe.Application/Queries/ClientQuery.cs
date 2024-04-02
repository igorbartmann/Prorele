using System;
using Microsoft.EntityFrameworkCore;
using ProReLe.Application.Interfaces.Queries;
using ProReLe.Domain.Entities;
using ProReLe.Domain.Interfaces.OuW;

namespace ProReLe.Application.Queries
{
    public class ClientQuery : IClientQuery
    {
        private readonly IQueryable<Client> ClientQueryable;

        public ClientQuery(IUnitOfWork unitOfWork)
        {
            ClientQueryable = unitOfWork.ClientRepository.Queryable.Where(c => !c.LogicallyExcluded).AsNoTracking();
        }

        public Client? GetById(int id)
        {
            var entity = ClientQueryable.FirstOrDefault(p => p.Id == id);
            return entity;
        }
        public Client? GetByCpf(string cpf)
        {
             var entity = ClientQueryable.FirstOrDefault(c => c.Cpf == cpf);
            return entity;
        }

        public IEnumerable<Client> GetByName(string name)
        {
            var entities = ClientQueryable
                .Where(e => e.Name.Contains(name))
                .ToList();

            return entities;
        }

        public IEnumerable<Client> GetAll()
        {
            var entities = ClientQueryable.ToList();
            return entities;
        }
    }
}