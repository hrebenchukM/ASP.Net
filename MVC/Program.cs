
using Microsoft.EntityFrameworkCore;
using MVC;//�������� ������������ ���� ������� ����� , ���������� ����� ������ ������. ���� ��� ����������� � ������ �������� � StudentContext �������� ���������� ������������ ����

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// �������� ������ ����������� �� ����� ������������
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// ��������� �������� ApplicationContext � �������� ������� � ����������
builder.Services.AddDbContext<FilmContext>(options => options.UseSqlServer(connection));//������� ������ ������� �������������������� � ����� ����� ������� ��� ��� �����������. �������� ������������ 

// ��������� ������� MVC
builder.Services.AddControllersWithViews();//������� ������� ��� ��������� ������������ ������������� ������� ���



var app = builder.Build();//������ ��� ������� ������ ���������� �� �� ����� � ��������� �������� ������� ��� ������������ ��� �������

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();//��� ���� ���� �������� ��������� ���� ����������� ������ ����� wwwroot

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(//������������ url ������ � ����������. ������������ ������� �������������
    name: "default",
    pattern: "{controller=Films}/{action=Index}/{id?}");//�� ��������� �������
//�� ������ �� ����� ������ ����� ������� ���� �������, � ���� ����� ��������.
app.Run();













