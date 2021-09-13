using Microsoft.AspNetCore.Http;


namespace ImageLoadUpload.Models
{
    public class ImageModel
    {
        //Property to store image file on Azure Storage container
        public IFormFile ImageFile { set; get; }
    }
}
