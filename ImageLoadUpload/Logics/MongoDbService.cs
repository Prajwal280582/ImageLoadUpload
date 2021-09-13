using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using ImageLoadUpload.Models;

namespace ImageLoadUpload.Logics
{
    public class MongoDbService
    {
        //Private variable for Mongo collection object
        private readonly IMongoCollection<MongoModel> _images;
        
        //Initializing the database, connectionstring and collection names
        public MongoDbService(MongoModel.IImageGalleryDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _images = database.GetCollection<MongoModel>(settings.ImageCollectionName);
        }

        //Fetching all the documents from Azure Cosmos DB - Mongo DB Api
        public List<MongoModel> Get() =>
            _images.Find(image => true).ToList();

        //Fetching document by id from Azure Cosmos DB - Mongo DB Api
        public MongoModel Get(string id) =>
            _images.Find<MongoModel>(image => image.id == id).FirstOrDefault();

        //Creating a document in Azure Cosmos DB - Mongo DB Api
        public MongoModel Create(MongoModel image)
        {
            _images.InsertOne(image);
            return image;
        }

        //Updating a document by Id in Azure Cosmos DB - Mongo DB Api
        public void Update(string id, MongoModel imageIn) =>
            _images.ReplaceOne(image => image.id == id, imageIn);

        //Deleting multiple documents by Id from Azure Cosmos DB - Mongo DB Api
        public void Remove(MongoModel imageIn) =>
            _images.DeleteOne(image => image.id == imageIn.id);

        //Deleting one document by Id from Azure Cosmos DB - Mongo DB Api
        public void Remove(string id) =>
            _images.DeleteOne(image => image.id == id);

        //Deleting one document by name from Azure Cosmos DB - Mongo DB Api
        public void RemoveByName(string name) =>
            _images.DeleteOne(image => image.ImageName == name);

        //Fetching document by id from Azure Cosmos DB - Mongo DB Api
        public MongoModel GetByName(string name) =>
            _images.Find<MongoModel>(image => image.ImageName == name).FirstOrDefault();
    }
}
