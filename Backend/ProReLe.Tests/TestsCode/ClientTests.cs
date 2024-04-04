using Microsoft.EntityFrameworkCore;
using ProReLe.Application.Interfaces.Queries;
using ProReLe.Application.Interfaces.Services;
using ProReLe.Application.Queries;
using ProReLe.Application.Services;
using ProReLe.Data.Persistence;
using ProReLe.Data.Persistence.OuW;
using ProReLe.Domain.Entities;
using Xunit.Extensions.Ordering;

namespace ProReLe.Tests
{
    [Order(2)]
    public class ClientTests
    {
        private readonly IClientQuery _clientQuere;
        private readonly IClientService _clientService;        
        
        public ClientTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ProReLeContext>()
                .UseSqlServer(GlobalValues.CONNECTION_STRING)
                .Options;

            var unitOfWork = new UnitOfWork(new ProReLeContext(dbContextOptions));
            _clientQuere = new ClientQuery(unitOfWork);
            _clientService = new ClientService(unitOfWork);
        }

        [Fact, Order(0)]
        public void PrepareDatabase()
        {
            throw new NotImplementedException("Test case not implemented!");
        }

        [Fact, Order(1)]
        public void IncludeClient()
        {
            throw new NotImplementedException("Test case not implemented!");
        }

        [Fact, Order(2)]
        public void EditClient()
        {
            throw new NotImplementedException("Test case not implemented!");
        }

        [Fact, Order(3)]
        public void DeleteClient()
        {
            throw new NotImplementedException("Test case not implemented!");
        }

        [Fact, Order(4)]
        public void GetClientById()
        {
            throw new NotImplementedException("Test case not implemented!");
        }

        [Fact, Order(5)]
        public void GetClientByCpf()
        {
            throw new NotImplementedException("Test case not implemented!");
        }

        [Fact, Order(6)]
        public void GetClientsByName()
        {
            throw new NotImplementedException("Test case not implemented!");
        }

        [Fact, Order(8)]
        public void GetAllClients()
        {            
            throw new NotImplementedException("Test case not implemented!");
        }
    }
}