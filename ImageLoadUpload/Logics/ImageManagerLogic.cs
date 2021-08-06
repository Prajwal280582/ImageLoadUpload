using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using ImageLoadUpload.Models;
using System;

namespace ImageLoadUpload.Logics
{
    public class ImageManagerLogic : IImageManagerLogic
    {
        private readonly BlobServiceClient _blobServiceClient;

        public ImageManagerLogic(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
           
        }

        public async Task Upload(ImageModel model)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("imageupload");
            var blobClient = blobContainer.GetBlobClient(model.ImageFile.FileName);
            await blobClient.UploadAsync(model.ImageFile.OpenReadStream());
        }


        public async Task<byte[]> Get(string ImageName)
        {
            try
            {
                var blobContainer = _blobServiceClient.GetBlobContainerClient("imageupload");
                var blobClient = blobContainer.GetBlobClient(ImageName);

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

        public async Task Delete(string ImageName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("imageupload");
            var blobClient = blobContainer.GetBlobClient(ImageName);

            await blobClient.DeleteAsync();
        }


    }
}
