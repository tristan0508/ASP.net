using Dogo.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Dogo.Repositories
{
    public interface IWalkerRepository
    {
        List<Walker> GetAllWalkers();
        Walker GetWalkerById(int id);
        List<Walker> GetWalkersInNeighborhood(int neighborhoodId);
        void AddWalker(Walker walker);
        void UpdateWalker(Walker walker);
        void DeleteWalker(int id);
    }
}