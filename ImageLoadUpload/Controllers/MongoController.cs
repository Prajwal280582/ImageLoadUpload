using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using ImageLoadUpload.Logics;
using ImageLoadUpload.Models;

namespace ImageLoadUpload.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MongoController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        //Initializing the mongodb logic using constructor
        public MongoController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        // Api to get list of all documents from the azure cosmos db for mongo api container
        [HttpGet]
        public ActionResult<List<MongoModel>> Get() => _mongoDbService.Get();

        // Api to get each document by id from the azure cosmos db for mongo api container
        [HttpGet("{id:length(24)}", Name = "GetImage")]
        public ActionResult<MongoModel> Get(string id)
        {
            try
            {
                var image = _mongoDbService.Get(id);

                if (image == null)
                {
                    return NotFound();
                }

                return image;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return null;
            }
        }

        // Api to add a document to the azure cosmos db for mongo api container
        [HttpPost]
        public ActionResult<MongoModel> Create(MongoModel image)
        {
            try
            {
                _mongoDbService.Create(image);
                return CreatedAtRoute("GetImage", new { id = image.id.ToString() }, image);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return null;
            }
        }

        // Api to update a document by id from the azure cosmos db for mongo api container
        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, MongoModel imageIn)
        {
            try
            {
                var image = _mongoDbService.Get(id);

                if (image == null)
                {
                    return NotFound();
                }

                _mongoDbService.Update(id, imageIn);

                return NoContent();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return null;
            }
        }

        // Api to delete a document by id from the azure cosmos db for mongo api container
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var image = _mongoDbService.Get(id);
                if (image == null)
                {
                    return NotFound();
                }

                _mongoDbService.Remove(image.id);

                return NoContent();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return null;
            }
        }

        // Api to delete a document by name from the azure cosmos db for mongo api container
        [HttpDelete]
        public IActionResult DeleteByName(string name)
        {
            try
            {
                var image = _mongoDbService.GetByName(name);
                if (image == null)
                {
                    return NotFound();
                }

                _mongoDbService.RemoveByName(image.ImageName);

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
