
using Microsoft.EntityFrameworkCore;
using MVC;//основное пространство имен которое везде , контроллер будет видеть модель. Чтоб тут подключится к модели допустим к StudentContext придется подключить пространство имен

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Получаем строку подключения из файла конфигурации
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<FilmContext>(options => options.UseSqlServer(connection));//обьекты класса серисес будутинжектироваться в любом месте проекта где они потребуются. Инверсия зависимостей 

// Добавляем сервисы MVC
builder.Services.AddControllersWithViews();//сервисы которые нам позволяют использовать архитектурный паттерн мвс



var app = builder.Build();//прежде чем создать обьект приложения мы до этого в коллекцию сервисов создали все необходиммые нам сервисы

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();//для того чтоб включить обработку всех статических файлов папки wwwroot

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(//сопоставляет url адреса с маршрутами. Регистрируем таблицу маршрутизации
    name: "default",
    pattern: "{controller=Film}/{action=Index}/{id?}");//по умолчанию маршрут
//мы больше не будем писать явным образом пути страниц, а лишь будут маршруты.
app.Run();













