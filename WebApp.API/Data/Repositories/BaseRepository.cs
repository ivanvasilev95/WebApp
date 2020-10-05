using System.Threading.Tasks;

namespace WebApp.API.Data.Repositories
{
    public abstract class BaseRepository<T> where T: class
    {
        protected readonly DataContext _context;

        public BaseRepository(DataContext context)
        {
            _context = context;
        }

        public async Task Add(T entity)
        {
            await _context.AddAsync<T>(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove<T>(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}