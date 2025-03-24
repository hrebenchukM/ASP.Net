using Microsoft.EntityFrameworkCore;
using GuestbookMVC.Models;

namespace GuestbookMVC.Repository
{
    public class MessageRepository : IMessageRepository 
    {
        private readonly UserContext _context;

        public MessageRepository(UserContext context)
        {
            _context = context;
        }
        public async Task<List<Message>> GetList()
        {
            return await _context.Messages.Include(m => m.User).ToListAsync();
        }
        public async Task<Message?> Get(int id)
        {
            return await _context.Messages.Include(m => m.User).FirstOrDefaultAsync(m => m.Id == id);
        }


        public async Task Create(Message u)
        {
            await _context.Messages.AddAsync(u);
        }

        public void Update(Message m)
        {
            _context.Messages.Update(m);
        }

        public async Task Delete(int id)
        {
            Message? m = await _context.Messages.FindAsync(id);
            if (m != null)
                _context.Messages.Remove(m);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }



    }
}
