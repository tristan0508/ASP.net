using Dogo.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Dogo.Repositories
{
    public interface IWalkerRepository
    {
        List<Walker> GetAllWalkers();
        Walker GetWalkerById(int id);
    }
}