using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ImageLoadUpload.Models
{
    public class CosmosModel
    {
        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("imageTimeline")]
        public string ImageTimeline { set; get; }

        [JsonPropertyName("imageComments")]
        public string ImageComments { set; get; }

        [JsonPropertyName("imageUploadDate")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime ImageUploadDate { set; get; }

    }
}
