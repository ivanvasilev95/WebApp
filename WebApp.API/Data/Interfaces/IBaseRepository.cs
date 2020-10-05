using System.Threading.Tasks;

namespace WebApp.API.Data.Interfaces
{
    public interface IBaseRepository<T> where T: class
    {
        Task Add(T entity);
        void Delete(T entity);
        Task<bool> SaveAll();
    }
}