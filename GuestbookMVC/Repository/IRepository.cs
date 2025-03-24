using GuestbookMVC.Models;

namespace GuestbookMVC.Repository
{
    public interface IRepository<T>
    {
        Task<List<T>> GetList();
        Task<T> Get(int id);
        Task Create(T item);
        void Update(T item);
        Task Delete(int id);
        Task Save();
    }
}
