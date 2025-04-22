using GuestbookMVC.Models;
using Microsoft.EntityFrameworkCore;
using GuestbookMVC.Repository;
using GuestbookMVC.Services;
using GuestbookMVC.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ��� ������ �������� ������ ������� IDistributedCache, � ASP.NET Core 
// ������������� ���������� ���������� IDistributedCache
builder.Services.AddDistributedMemoryCache();// ��������� IDistributedMemoryCache
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(180); // ������������ ������ (����-��� ���������� ������)
    options.Cookie.Name = "Session"; // ������ ������ ����� ���� �������������, ������� ����������� � �����.

}); // ��������� ������� ������

// �������� ������ ����������� �� ����� ������������
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// ��������� �������� ApplicationContext � �������� ������� � ����������
builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connection));//��������� �������� StudentContext


// ��������� ������� MVC
builder.Services.AddControllersWithViews();

// Transient: ������ ������� ��������� ������ ���, ����� ��������� ��������� ������ �������. 
// �������� ������ ���������� ����� �������� �������� ��� ����������� ��������, ������� �� ������ ������ � ���������.

// Singleton: ������ ������� ��������� ��� ������ ��������� � ����, 
// ��� ����������� ������� ���������� ���� � ��� �� ����� ��������� ������ �������

// Scoped: ��� ������� http ������� ��������� ���� ������ �������.
builder.Services.AddScoped<IUserRepository, UserRepository>();//����������� ������� ���������� � IUserRepository � ������ � �������� ������� ����� ������������ ������
builder.Services.AddScoped<IMessageRepository, MessageRepository>();//����������� ������� MessageRepository ����� ������� ����� ����� ���� � ��� �������� ����� ����������  IMessageRepository
builder.Services.AddScoped<IPassword, PasswordService>();//����������� ������� MessageRepository ����� ������� ����� ����� ���� � ��� �������� ����� ����������  IMessageRepository


var app = builder.Build();
app.UseSession();   // ��������� middleware-��������� ��� ������ � ��������
app.UseStaticFiles(); // ������������ ������� � ������ � ����� wwwroot

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
