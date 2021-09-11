using ImageLoadUpload.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageLoadUpload.Logics
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<CosmosModel>> GetMultipleAsync(string query);
        Task<CosmosModel> GetAsync(string id);
        Task AddAsync(CosmosModel item);
        Task UpdateAsync(string id, CosmosModel item);
        Task DeleteAsync(string id);
    }
}
