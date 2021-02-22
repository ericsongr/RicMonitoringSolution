using RicModel.CloudProvider;

namespace RicCommon.Interface.CloudProvider
{
    public interface IDeleteService
    {
        bool DeleteImage(FileDetails fileDetail);
    }
}
