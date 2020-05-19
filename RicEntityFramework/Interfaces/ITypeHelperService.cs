namespace RicEntityFramework.Interfaces
{
    public interface ITypeHelperService
    {
        bool TypeHasProperties<T>(string fields);
    }
}
