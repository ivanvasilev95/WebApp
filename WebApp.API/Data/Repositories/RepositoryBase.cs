using System.Threading.Tasks;

namespace WebApp.API.Data.Repositories
{
    public abstract class RepositoryBase
    {
        protected readonly DataContext _context;

        public RepositoryBase(DataContext context)
        {
            _context = context;
        }

        public void Add(object entity)
        {
            _context.Add(entity);
        }

        public void Delete(object entity)
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}