using RicModel.CloudProvider;

namespace RicCommon.Interface.CloudProvider
{
    public interface IGetService
    {
        void CopyImage(FileDetails fileSourceDetails, FileDetails fileDestDetails);
        string GetImageUrl(FileDetails fileDetails, bool includePreSignedUrl = true);
        string GetVideoUrl(FileDetails fileDetails);
        byte[] GetObjectAsByte(string bucketName, string key);
    }
}
