using System;

namespace ProReLe.Domain.Entities
{
    public class Client : BaseEntity
    {
        public Client(string name, string cpf) : base()
        {
            Name = name;
            Cpf = cpf;
        }
        public Client(int id, string name, string cpf, bool logicallyExcluded) : base(id)
        {
            Name = name;
            Cpf = cpf;
            LogicallyExcluded = logicallyExcluded;
        }

        public string Name {get;set;}
        public string Cpf {get;set;}
        public bool LogicallyExcluded {get;set;}

    }
}