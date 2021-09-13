using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageLoadUpload.Models;

namespace ImageLoadUpload.Logics
{
    public interface IImageManagerLogic
    {
        // Method defination for Image storage on a Azure storage service

        #region Methods for Image storage
        //Uploading a image to a azure storage service
        Task Upload(ImageModel model);
       
        //Fetching a image by name from azure storage container
        Task<byte[]> Get(string ImageName);

        //Fetching all images from azure storage container
        Task<List<Uri>> GetAll();
       
        //Deleting a image by name from azure storage container
        Task Delete(string ImageName);
        #endregion
    }
}
