
namespace RicEntityFramework.Interfaces
{
    public interface IImageService
    {
        string GetImageInBase64(int renterId);

        void Upload(int renterId, string base64);
        string Upload(string base64, string location);
    }
}
