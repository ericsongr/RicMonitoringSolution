
namespace RicEntityFramework.Interfaces
{
    public interface IImageService
    {
        string GetImageInBase64(int renterId);
        string GetImageInBase64(string filename, string path3 = "Profile");

        void Upload(int renterId, string base64);
        string Upload(string base64, string location);
    }
}
