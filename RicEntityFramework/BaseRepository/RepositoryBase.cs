
namespace RicEntityFramework.BaseRepository
{
    public abstract class RepositoryBase
    {
        public RicDbContext Context { get; }

        public RepositoryBase(RicDbContext context)
        {
            Context = context;
        }
    }
}
