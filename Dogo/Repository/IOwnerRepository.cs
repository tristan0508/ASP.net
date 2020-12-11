using Dogo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dogo.Repository
{
    public interface IOwnerRepository
    {
        Owner GetById(int id);
        List<Owner> GetOwners();
        void UpdateOwner(Owner owner);
        void DeleteOwner(int ownerId);
        void AddOwner(Owner owner);
        Owner GetOwnerByEmail(string email);
    }
}
