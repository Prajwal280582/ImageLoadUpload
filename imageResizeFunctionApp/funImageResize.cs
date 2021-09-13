using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;


namespace imageResizeFunctionApp
{
    //Class for resizing images in the Azure Storage service using Nuget - Image Sharp Library
    public static class funImageResize
    {
        //Function Name Defined - Azure Function Service
        [FunctionName("Thumbnail")] 
        public static void Run([BlobTrigger("imagegallery/{name}")] Stream myBlob, string name, [Blob("imagegallerythumbnail/sm-{name}", FileAccess.Write)] Stream outputBlob, ILogger log)
        {
            //Log Information Captured
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            try
            {
                //Loading image from Azure Storage area
                using (var image = Image.Load(myBlob))
                {
                    //Resizing Image
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(150, 150),
                        Mode = ResizeMode.Crop
                    })
                    .Grayscale()); //Images are Gray - scaled
                    
                    //Saving images as png in output azure storage area
                    using (var ms = new MemoryStream())
                    {
                        image.SaveAsPng(outputBlob);
                    }
                }

                log.LogInformation("Image resized", null);
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.Message, null);
            }
        }
    }
}
