using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC;

namespace MVC
{
    public class HomeController : Controller
    {

        // ContentResult: ����� ��������� ������� �������� � ����� � ���� ������
        // ���� � �������� ������������� ���������� ��� string, �� ��������� 
        // ������������� ������� ������ ContentResult ��� ������������ ������.
        //� ����� ����� ��� �� ������ ����� �������� action, �� action ��� ������ �����
       

        public RedirectResult RedirectMethod()
        {
            return Redirect("/Home/Index");//������������� ����� ����� ��������� �� 
        }

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            ViewData["Head"] = "Welcome to the Home Page";
            ViewBag.Name = "ASP.NET MVC Example";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
