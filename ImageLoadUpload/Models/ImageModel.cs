using Microsoft.AspNetCore.Http;

namespace ImageLoadUpload.Models
{
    public class ImageModel
    {
        public IFormFile ImageFile { set; get; }
    }
}
