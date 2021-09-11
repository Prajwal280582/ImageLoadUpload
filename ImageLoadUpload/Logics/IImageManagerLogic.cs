using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageLoadUpload.Models;

namespace ImageLoadUpload.Logics
{
    public interface IImageManagerLogic
    {
        Task Upload(ImageModel model);
        Task<byte[]> Get(string ImageName);

        List<Uri> GetAll();
        Task Delete(string ImageName);
    }
}
