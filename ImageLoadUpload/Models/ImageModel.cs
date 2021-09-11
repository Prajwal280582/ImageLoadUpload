using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ImageLoadUpload.Models
{
    public class ImageModel
    {
        public IFormFile ImageFile { set; get; }
    }
}
