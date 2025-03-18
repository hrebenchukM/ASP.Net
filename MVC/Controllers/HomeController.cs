using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC;

namespace MVC
{
    public class HomeController : Controller
    {

        // ContentResult: пишет указанный контент напрямую в ответ в виде строки
        // Если в качестве возвращаемого результата тип string, то фреймворк 
        // автоматически создаст объект ContentResult для возвращаемой строки.
        //С точки зрени мвс не всегда метод является action, но action єто всегда метод
       

        //public RedirectResult RedirectMethod()
        //{
        //    return Redirect("/Home/Index");//переадресация можно после изменения бд 
        //}

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
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
