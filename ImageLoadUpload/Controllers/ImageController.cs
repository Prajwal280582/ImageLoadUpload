using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ImageLoadUpload.Models;
using ImageLoadUpload.Logics;
using System.Collections.Generic;

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

        [Route("getall")]
        [HttpPost]
        public List<Uri> GetAll()
        {
            try
            {
                var listUri = _imageManagerLogic.GetAll();
                return listUri;
            }
            catch (Exception Ex)
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
