using GuestbookMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace GuestbookMVC.Repository
{
    public interface IUserRepository:IRepository<User>
    {
        Task<bool> AnyUsers();


        Task<User?> GetUserByLogin(string login);
    }
}
