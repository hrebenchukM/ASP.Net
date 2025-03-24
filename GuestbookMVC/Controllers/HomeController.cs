using System.Diagnostics;
using GuestbookMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using GuestbookMVC;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using GuestbookMVC.Repository;

namespace GuestbookMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IMessageRepository repo;
        private readonly IUserRepository _urepo;
        public HomeController(ILogger<HomeController> logger, IMessageRepository r, IUserRepository ur)
        {
            _logger = logger;
            repo = r;
            _urepo = ur;


        }

        public async Task<IActionResult> Index()//куки всегда передаются в составе шттп пакета по шттп протоколу 
        {
            string? login = HttpContext.Session.GetString("Login") ?? Request.Cookies["login"];
            if (login != null)
            {
                HttpContext.Session.SetString("Login", login); // Восстанавливаем сессию из куки
                var msgContext = await _urepo.GetList();
                ViewBag.UserId = new SelectList(msgContext, "Id", "Login");

                var messages = await repo.GetList();
                return View(messages);
            }
            //if (HttpContext.Session.GetString("Login") != null)
            //{         //если сессионные переменные есть то возвращаем индекс вьюшку
            //    ViewBag.UserId = new SelectList(_context.Users, "Id", "Login");
            //    return View(await msgContext.ToListAsync());
            //}
            else
            {
                return RedirectToAction("Login", "Account"); //если сессионная переменная такая отсуствует делаем редирект через браузер на екшн Login  контроллера Account
            }//после редиректа   return RedirectToAction("Login", "Account"); мы уже в else  не зайдем 

        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear(); // очищается сессия (и переменная и идентификатор )
                                         //HttpContext.Session.Remove("login");//удаляется конкретная сессионная переменная (без идентификатора)
            
            Response.Cookies.Delete("login"); // удаление куки не сразу ,это настройка параметров куки,заголовков . Браузер удалит куки когда получит ответ

            return RedirectToAction("Login", "Account");
        }
      
        [HttpPost]
        public async Task<IActionResult> AddMessage(string content)
        {
            var log = HttpContext.Session.GetString("Login");
            var user = await _urepo.GetUserByLogin(log);
            if (user != null)
            {
                var message = new Message
                {
                    UserId = user.Id,
                    Content = content,
                    Date = DateTime.Now
                };

                if (ModelState.IsValid)
                {
                    await repo.Create(message);
                    await repo.Save();

                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.UserId = new SelectList(await _urepo.GetList(), "Id", "Login");
            return RedirectToAction("Index");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
