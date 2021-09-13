using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ImageLoadUpload.Models
{
    public class MongoModel
    {
        //Image(Document) properties to store in Cosmos Db for Mongo DB Api 

        #region Properties
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] 
        public string id { get; set; }     //Unique Id defined. At the moment automatically created in the mongo db

        [BsonElement("imageName")]
        public string ImageName { set; get; }

        [BsonElement("imageUri")]
        public Uri ImageUri { set; get; }

        [BsonElement("imageThumbnailUri")]
        public Uri ImageThumbnailUri { set; get; }

        [BsonElement("imageLength")]
        public string ImageLength { set; get; }

        [BsonElement("imageType")]
        public string ImageType { set; get; }

        [BsonElement("imageUploadDate")]
        public DateTime ImageUploadDate { set; get; }

        #endregion

        //Mongo DB Connection properties in below class and Interface

        #region Class and Interface
        public class ImageGalleryDatabaseSettings : IImageGalleryDatabaseSettings
        {
            public string ImageCollectionName { get; set; }
            public string ConnectionString { get; set; }
            public string DatabaseName { get; set; }
        }

        public interface IImageGalleryDatabaseSettings
        {
            string ImageCollectionName { get; set; }
            string ConnectionString { get; set; }
            string DatabaseName { get; set; }
        }

        #endregion
    }
}
