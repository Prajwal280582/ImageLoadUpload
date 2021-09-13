using ImageLoadUpload.Logics;
using ImageLoadUpload.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ImageLoadUpload.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CosmosController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;

        //Initializing the Cosmos DB logic using constructor

        public CosmosController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        }

        // Api to get list of all items from the azure cosmos container
        [HttpGet]
        public async Task<IActionResult> List()
        {
            try
            {
                return Ok(await _cosmosDbService.GetMultipleAsync("SELECT * FROM c"));
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return null;
            }
        }

        //  Api to get each item by id from the azure cosmos container
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                return Ok(await _cosmosDbService.GetAsync(id));
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return null;
            }
        }


        //  Api to add a item to the azure cosmos container
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CosmosModel item)
        {
            try
            {
                if (item.id is null)
                {
                    item.id = Guid.NewGuid().ToString();
                }

                await _cosmosDbService.AddAsync(item);
                return CreatedAtAction(nameof(Get), new { id = item.id }, item);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return null;
            }
        }

        //  Api to edit each item by id on the azure cosmos container
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromBody] CosmosModel item)
        { 
            try{
                await _cosmosDbService.UpdateAsync(item.id, item);
                return NoContent();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return null;
            }
        }

        //  Api to delete each item by id on the azure cosmos container
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _cosmosDbService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return null;
            }
        }
    }
}
