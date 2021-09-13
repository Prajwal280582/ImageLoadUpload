using ImageLoadUpload.Models;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageLoadUpload.Logics
{
    public class CosmosDbLogic : ICosmosDbService
    {
        //private variable for Cosmos DB container
        private Container _container;
        
        //Initailizing the private variable
        public CosmosDbLogic(
            CosmosClient cosmosDbClient,
            string databaseName,
            string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }

        //Add a item to Cosmos db
        public async Task AddAsync(CosmosModel item)
        {
            try
            {
                //Partition key is the unique object id if not sent, will be created by Cosmos Db service
                await _container.CreateItemAsync(item, new PartitionKey(item.id));
            }
            catch (CosmosException ce) //For handling item not found and other exceptions
            {
                Console.WriteLine(ce.Message);
            }
        }

        //Delete a item using id from Cosmos db   
        public async Task DeleteAsync(string id)
        {
            try
            {
                await _container.DeleteItemAsync<CosmosModel>(id, new PartitionKey(id));
            }
            catch (CosmosException ce) //For handling item not found and other exceptions
            {
                Console.WriteLine(ce.Message);
            }
        }

        //Get single item using id from Cosmos DB  sql api
        public async Task<CosmosModel> GetAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<CosmosModel>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ce) //For handling item not found and other exceptions
            {
                Console.WriteLine(ce.Message);
                return null;
            }
        }

        //Get multiple items using query
        public async Task<IEnumerable<CosmosModel>> GetMultipleAsync(string queryString)
        {
            try
            {
                //Results are fetched based on the query passed
                var query = _container.GetItemQueryIterator<CosmosModel>(new QueryDefinition(queryString));
                var results = new List<CosmosModel>();

                //Fetching the details using query and adding to list object
                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    results.AddRange(response.ToList());
                }
                return results;
            }
            catch (CosmosException ce) //For handling item not found and other exceptions
            {
                Console.WriteLine(ce.Message);
                return null;
            }
        }

        //Update an item using id in Cosmos db - sql api
        public async Task UpdateAsync(string id, CosmosModel item)
        {
            try
            {
                await _container.UpsertItemAsync(item, new PartitionKey(id));
            }
            catch (CosmosException ce) //For handling item not found and other exceptions
            {
                Console.WriteLine(ce.Message);
            }
        }
    }
}
