using RicModel.CloudProvider;

namespace RicCommon.Interface.CloudProvider
{
    public interface IUploadService
    {
        UploadResponse Upload(UploadRequest uploadRequest);
        bool DeleteAttachment(string awsSubFolder, string attachmentLocation);
    }
}
