using System.Threading.Tasks;

namespace WebApp.API.Data.Interfaces
{
    public interface IRepositoryBase
    {
        void Add(object entity);
        void Delete(object entity);
        Task<bool> SaveAll();
    }
}