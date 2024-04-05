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
            var client = _clientQuere.GetByCpf(GlobalValues.CLIENT_CPF);
            
            if (client is not null)
            {
                var response = _clientService.Delete(client);
                if (!response.Success)
                {
                    Assert.Fail("Unable to clean the database!");
                }
            }
        }

        [Fact, Order(1)]
        public void IncludeClient()
        {
            // Arrange: variables declarations and initilizations.
            var client = new Client(GlobalValues.CLIENT_NAME_BEFORE_EDIT, GlobalValues.CLIENT_CPF);

            // Act: call to the method to be tested.
            var response = _clientService.Include(client);

            // Assert: result validation
            Assert.True(response.Success, "The operation does not return a success status.");
        }

        [Fact, Order(2)]
        public void EditClient()
        {
            // Arrange: variables declarations and initilizations.
            var clientsByName = _clientQuere.GetByName(GlobalValues.CLIENT_NAME_BEFORE_EDIT);

            var client = clientsByName
                .FirstOrDefault(c =>
                    c.Name == GlobalValues.CLIENT_NAME_BEFORE_EDIT
                    && c.Cpf == GlobalValues.CLIENT_CPF);

            if (client is null) {
                Assert.Fail("Client not found!");
            }

            // Act: call to the method to be tested.
            client.Name = GlobalValues.CLIENT_NAME_AFTER_EDIT;

            var response = _clientService.Update(client);

            // Assert: result validation
            Assert.True(response.Success, "The operation does not return a success status.");
        }

        [Fact, Order(3)]
        public void DeleteClient()
        {
            // Arrange: variables declarations and initilizations.
            var clientsByName = _clientQuere.GetByName(GlobalValues.CLIENT_NAME_AFTER_EDIT);

            var client = clientsByName
                .FirstOrDefault(c =>
                    c.Name == GlobalValues.CLIENT_NAME_AFTER_EDIT
                    && c.Cpf == GlobalValues.CLIENT_CPF);

            if (client is null)
            {
                Assert.Fail("Client not found!");
            }

            // Act: call to the method to be tested.
            var response = _clientService.Delete(client);

            // Assert: result validation
            Assert.True(response.Success, "The operation does not return a success status.");
        }

        [Fact, Order(4)]
        public void GetClientById()
        {
            // Arrange: variables declarations and initilizations.
            var clients = _clientQuere.GetAll();

            var clientIndex = clients.Count() / 2;
            var client = clients.ElementAt(clientIndex);

            // Act: call to the method to be tested.
            var clientById = _clientQuere.GetById(client.Id);

            if (clientById is null)
            {
                Assert.Fail("The client was not found!");
            }

            // Assert: result validation
            var clientsAreTheSame =
                client.Id == clientById.Id
                && client.Name == clientById.Name
                && client.Cpf == clientById.Cpf;

            Assert.True(clientsAreTheSame, "The client found is not the same of the query filter.");
        }

        [Fact, Order(5)]
        public void GetClientByCpf()
        {
            // Arrange: variables declarations and initilizations.
            var clients = _clientQuere.GetAll();

            var clientIndex = clients.Count() / 2;
            var client = clients.ElementAt(clientIndex);

            // Act: call to the method to be tested.
            var clientByCpf = _clientQuere.GetByCpf(client.Cpf);

            if (clientByCpf is null)
            {
                Assert.Fail("The client was not found!");
            }

            // Assert: result validation
            var clientsAreTheSame =
                client.Id == clientByCpf.Id
                && client.Name == clientByCpf.Name
                && client.Cpf == clientByCpf.Cpf;

            Assert.True(clientsAreTheSame, "The client found is not the same of the query filter.");
        }

        [Fact, Order(6)]
        public void GetClientsByName()
        {
            // Arrange: variables declarations and initilizations.
            var clients = _clientQuere.GetAll();

            var clientIndex = clients.Count() / 2;
            var client = clients.ElementAt(clientIndex);

            // Act: call to the method to be tested.
            var clientsByName = _clientQuere.GetByName(client.Name);

            if (!clientsByName.Any())
            {
                Assert.Fail("The client was not found!");
            }

            // Assert: result validation
            var clientsAreInTheList = clientsByName
                .Any(c => 
                    c.Id == client.Id
                    && c.Name == client.Name
                    && c.Cpf == client.Cpf);

            Assert.True(clientsAreInTheList, "The client is not in the list.");
        }

        [Fact, Order(7)]
        public void GetAllClients()
        {
            // Act: call to the method to be tested.
            var clients = _clientQuere.GetAll();

            // Assert: result validation
            Assert.True(clients.Any(), "There are no products in the list.");
        }
    }
}