using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;


namespace imageResizeFunctionApp
{
    public static class funImageResize
    {
        [FunctionName("Thumbnail")] //change the function-name to a more descriptive function name
        public static void Run([BlobTrigger("imageupload/{name}")] Stream myBlob, string name, [Blob("thumbnails/sm-{name}", FileAccess.Write)] Stream outputBlob, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            try
            {
                using (var image = Image.Load(myBlob))
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(150, 150),
                        Mode = ResizeMode.Crop
                    })
                    .Grayscale());

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
