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

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("Header")]
        public string ImageTimeline { set; get; }
        
        [JsonPropertyName("Description")]
        public string ImageComments { set; get; }

        [JsonPropertyName("UploadDate")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime ImageUploadDate { set; get; }
    }
}
