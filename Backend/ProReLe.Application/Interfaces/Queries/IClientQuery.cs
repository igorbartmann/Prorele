using System;
using ProReLe.Domain.Entities;

namespace ProReLe.Application.Interfaces.Queries
{
    public interface IClientQuery : IBaseQuery<Client>
    {
        Client? GetByCpf(string cpf);
        IEnumerable<Client> GetByName(string name);
    }
}