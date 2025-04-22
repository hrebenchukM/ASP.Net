
namespace GuestbookMVC.Interfaces
{
    public interface IPassword
    {
        string GenerateSalt();
        string HashPassword(string salt, string password);
    }
}
