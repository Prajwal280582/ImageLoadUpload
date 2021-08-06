using System.Threading.Tasks;
using ImageLoadUpload.Models;

namespace ImageLoadUpload.Logics
{
    public interface IImageManagerLogic
    {
        Task Upload(ImageModel model);
        Task<byte[]> Get(string ImageName);
        Task Delete(string ImageName);
    }
}
