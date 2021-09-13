using ImageLoadUpload.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageLoadUpload.Logics
{
    public interface ICosmosDbService
    {
        // Method defination for Cosmos DB CRUD Operations

        #region CRUD Methods
        //Get multiple items using query
        Task<IEnumerable<CosmosModel>> GetMultipleAsync(string query);
        
        //Get single item using id
        Task<CosmosModel> GetAsync(string id);
        
        //Add a item  to Cosmos db     
        Task AddAsync(CosmosModel item);
        
        //Update an item using id
        Task UpdateAsync(string id, CosmosModel item);
        
        //Delete an item using id
        Task DeleteAsync(string id);
        #endregion
    }
}
