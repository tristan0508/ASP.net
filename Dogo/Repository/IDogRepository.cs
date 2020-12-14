using Dogo.Models;
using System.Collections.Generic;

namespace Dogo.Repositories
{
    public interface IDogRepository
    {
        List<Dog> GetAllDogs();
        Dog GetDogById(int id);
        void AddDog(Dog dog);
    }
}