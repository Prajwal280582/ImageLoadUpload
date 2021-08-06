using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ImageLoadUpload.Models;
using ImageLoadUpload.Logics;

namespace ImageLoadUpload.Controllers
{
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageManagerLogic _imageManagerLogic;
      
        public ImageController(IImageManagerLogic imageManagerLogic)
        {
            _imageManagerLogic = imageManagerLogic;
        }

       [Route("upload")]
       [HttpPost]
       public async Task<IActionResult> Upload([FromForm] ImageModel model)
       {
            if(model.ImageFile != null)
            {
                await _imageManagerLogic.Upload(model);
            }
            return Ok();
       }
       
        [Route("get")]
        [HttpPost]
        public async Task<IActionResult> Get(string imageName)
        {
            try
            {
                var imgBytes = await _imageManagerLogic.Get(imageName);
                return File(imgBytes, "image/webp");
            }
            catch(Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return null;
            }
           
        }


        [Route("delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(string imageName)
        {
            await _imageManagerLogic.Delete(imageName);
            return Ok();
        }
    }

}
