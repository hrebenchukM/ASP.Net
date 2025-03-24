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

        public async Task<IActionResult> Index()//���� ������ ���������� � ������� ���� ������ �� ���� ��������� 
        {
            string? login = HttpContext.Session.GetString("Login") ?? Request.Cookies["login"];
            if (login != null)
            {
                HttpContext.Session.SetString("Login", login); // ��������������� ������ �� ����
                var msgContext = await _urepo.GetList();
                ViewBag.UserId = new SelectList(msgContext, "Id", "Login");

                var messages = await repo.GetList();
                return View(messages);
            }
            //if (HttpContext.Session.GetString("Login") != null)
            //{         //���� ���������� ���������� ���� �� ���������� ������ ������
            //    ViewBag.UserId = new SelectList(_context.Users, "Id", "Login");
            //    return View(await msgContext.ToListAsync());
            //}
            else
            {
                return RedirectToAction("Login", "Account"); //���� ���������� ���������� ����� ���������� ������ �������� ����� ������� �� ���� Login  ����������� Account
            }//����� ���������   return RedirectToAction("Login", "Account"); �� ��� � else  �� ������ 

        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear(); // ��������� ������ (� ���������� � ������������� )
                                         //HttpContext.Session.Remove("login");//��������� ���������� ���������� ���������� (��� ��������������)
            
            Response.Cookies.Delete("login"); // �������� ���� �� ����� ,��� ��������� ���������� ����,���������� . ������� ������ ���� ����� ������� �����

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
