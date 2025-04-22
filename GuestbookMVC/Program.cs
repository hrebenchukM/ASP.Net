using GuestbookMVC.Models;
using Microsoft.EntityFrameworkCore;
using GuestbookMVC.Repository;
using GuestbookMVC.Services;
using GuestbookMVC.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Все сессии работают поверх объекта IDistributedCache, и ASP.NET Core 
// предоставляет встроенную реализацию IDistributedCache
builder.Services.AddDistributedMemoryCache();// добавляем IDistributedMemoryCache
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(180); // Длительность сеанса (тайм-аут завершения сеанса)
    options.Cookie.Name = "Session"; // Каждая сессия имеет свой идентификатор, который сохраняется в куках.

}); // Добавляем сервисы сессии

// Получаем строку подключения из файла конфигурации
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connection));//считается сервисом StudentContext


// Добавляем сервисы MVC
builder.Services.AddControllersWithViews();

// Transient: объект сервиса создается каждый раз, когда требуется экземпляр класса сервиса. 
// Подобная модель жизненного цикла наиболее подходит для легковесных сервисов, которые не хранят данных о состоянии.

// Singleton: объект сервиса создается при первом обращении к нему, 
// все последующие запросы используют один и тот же ранее созданный объект сервиса

// Scoped: для каждого http запроса создается один объект сервиса.
builder.Services.AddScoped<IUserRepository, UserRepository>();//регистрация сервиса связанного с IUserRepository и работа с обьектом сервиса через интерфейсную ссылку
builder.Services.AddScoped<IMessageRepository, MessageRepository>();//регистрация сервиса MessageRepository таким образом чтобы можно было с ним работать через абстракцию  IMessageRepository
builder.Services.AddScoped<IPassword, PasswordService>();//регистрация сервиса MessageRepository таким образом чтобы можно было с ним работать через абстракцию  IMessageRepository


var app = builder.Build();
app.UseSession();   // Добавляем middleware-компонент для работы с сессиями
app.UseStaticFiles(); // обрабатывает запросы к файлам в папке wwwroot

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
