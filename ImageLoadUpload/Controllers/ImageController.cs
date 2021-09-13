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

        //Initializing the image logics using constructor
        public ImageController(IImageManagerLogic imageManagerLogic)
        {
            _imageManagerLogic = imageManagerLogic;
           
        }

        //Uploading Api for Multiple Image Upload
       [Route("Upload")]
       [HttpPost]
       public async Task<IActionResult> Upload([FromForm] ImageModel model)
       {
            try
            {
                if (model.ImageFile != null)
                {
                    await _imageManagerLogic.Upload(model);
                }
                return Ok();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return null;
            }
        }

        //Api to fetch one image by name       
        [Route("Get")]
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

        //Api to fetch all images present within the container
        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var listUri = await _imageManagerLogic.GetAll();
                return Ok(listUri);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return null;
            }

        }

        //Api to delete one image at a time
        [Route("Delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(string imageName)
        {
            try
            {
                await _imageManagerLogic.Delete(imageName);
                return Ok();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return null;
            }

        }
    }

}
