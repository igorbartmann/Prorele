using System;
using Microsoft.EntityFrameworkCore;
using ProReLe.Domain.Entities;
using ProReLe.Domain.Interfaces.Repositories;

namespace ProReLe.Data.Persistence.Repositories
{
    public class ClientRepository: BaseRepository<Client>, IClientRepository
    {
        private readonly ProReLeContext _context;
        public ClientRepository(ProReLeContext context) : base(context)
        {
            _context = context;
        }

        public override Client? GetById(int id)
        {
            var entity = _context.Clients
                .Where(c => !c.LogicallyExcluded)
                .FirstOrDefault(c => c.Id == id);

            return entity;
        }
    }
}