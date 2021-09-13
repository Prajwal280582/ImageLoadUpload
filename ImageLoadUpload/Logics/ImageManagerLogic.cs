using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using ImageLoadUpload.Models;
using System;
using Azure.Storage.Blobs.Specialized;
using System.Collections.Generic;

namespace ImageLoadUpload.Logics
{
    public class ImageManagerLogic : IImageManagerLogic
    {
        //private variables Azure Storage and Mongo DB
        private readonly BlobServiceClient _blobServiceClient;
        private readonly MongoDbService _mongoDbService;

        //Requires to set the mongo db properties
        MongoModel image = new MongoModel();

        //Constructor for intiallzing the private variables
        public ImageManagerLogic(BlobServiceClient blobServiceClient, MongoDbService mongoDbService)
        {
            _blobServiceClient = blobServiceClient;
            _mongoDbService = mongoDbService;
        }

        //Uploading a image to a azure storage service
        public async Task Upload(ImageModel model)
        {
            try
            {
                //uploading functionality
                var blobContainer = _blobServiceClient.GetBlobContainerClient("imagegallery");
                var blobClient = blobContainer.GetBlobClient(model.ImageFile.FileName);

                //If image already exists, delete it from the Azure storage services and database entry
                if (await blobClient.ExistsAsync())
                {
                    await this.Delete(model.ImageFile.FileName);                 
                }
               //Uploading to Azure Storage
               await blobClient.UploadAsync(model.ImageFile.OpenReadStream());
                                 
                image.ImageName = blobClient.Name;
                image.ImageUploadDate = DateTime.Now;
                image.ImageUri = blobClient.Uri;
                image.ImageLength = model.ImageFile.Length.ToString();
                image.ImageType = model.ImageFile.ContentType.ToString();
                image.ImageThumbnailUri = new Uri(blobClient.Uri.ToString().Replace("imagegallery/", "imagegallerythumbnail/sm-"));

                //Inserting the details in to  mongo DB
                 _mongoDbService.Create(image);
                
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
        }

        //Fetching a image by name from azure storage container
        public async Task<byte[]> Get(string ImageName)
        {
            try
            {
                var blobContainer = _blobServiceClient.GetBlobContainerClient("imagegallery");
                var blobClient = blobContainer.GetBlobClient(ImageName);     

                //Downloading to memory stream and return image as memory stream
                using (MemoryStream ms = new MemoryStream())
                {
                    await blobClient.DownloadToAsync(ms);
                    return ms.ToArray();
                }
            }catch(Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                    return null;
            }
        }

        //Fetching all images from azure storage container
        public async Task<List<Uri>> GetAll()
        {
            try
            {
                //Fetching the image from the thumbnails container
                var blobContainer = _blobServiceClient.GetBlobContainerClient("imagegallerythumbnail");
                var blobClient = blobContainer.GetBlobs();

                //Object for URi storage
                List<Uri> uriList = new List<Uri>();

                //Getting each blob details from azure storage service and storing in generic list object
                await Task.Run(() =>
                {
                    foreach (var item in blobClient)
                    {
                        string name = item.Name;
                        BlockBlobClient blockBlob = blobContainer.GetBlockBlobClient(name);
                        uriList.Add(blockBlob.Uri);
                    }
                });
                return uriList;        
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return null;
            }
        }

        //Deleting a image by name from azure storage container
        public async Task Delete(string ImageName)
        {
            try
            {
                var blobContainer = _blobServiceClient.GetBlobContainerClient("imagegallery");
                var blobClient = blobContainer.GetBlobClient(ImageName);

                await blobClient.DeleteAsync();

                var thumbImageName = "sm-" + ImageName;
                var blobThumbContainer = _blobServiceClient.GetBlobContainerClient("imagegallerythumbnail");
                var blobThumbClient = blobThumbContainer.GetBlobClient(thumbImageName);

                await blobThumbClient.DeleteAsync();

                
                _mongoDbService.RemoveByName(ImageName);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);

            }
        }


    }
}
