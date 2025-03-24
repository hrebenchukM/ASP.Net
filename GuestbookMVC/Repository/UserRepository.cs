using Microsoft.EntityFrameworkCore;
using GuestbookMVC.Models;

namespace GuestbookMVC.Repository
{
    public class UserRepository : IUserRepository//реализация нашей абстракции
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetList()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> Get(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task Create(User u)
        {
            await _context.Users.AddAsync(u);
        }

        public void Update(User u)
        {
            _context.Users.Update(u);
        }

        public async Task Delete(int id)
        {
            User? u = await _context.Users.FindAsync(id);
            if (u != null)
                _context.Users.Remove(u);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }



        public async Task<bool> AnyUsers()
        {
            return await _context.Users.AnyAsync();
        }


        public async Task<User?> GetUserByLogin(string login)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        }
    }
}