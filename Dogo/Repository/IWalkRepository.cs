using Dogo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dogo.Repository
{
    interface IWalkRepository
    {
        List<Walk> GetWalksByWalkerId(int id);
    }
}
