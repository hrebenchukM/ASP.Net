using System.Diagnostics;
using GuestbookMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using GuestbookMVC;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace GuestbookMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserContext _context;
        public HomeController(ILogger<HomeController> logger, UserContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()//���� ������ ���������� � ������� ���� ������ �� ���� ��������� 
        {
            var msgContext = _context.Messages.Include(m => m.User);
            string? login = HttpContext.Session.GetString("Login") ?? Request.Cookies["login"];
            if (login != null)
            {
                HttpContext.Session.SetString("Login", login); // ��������������� ������ �� ����
                ViewBag.UserId = new SelectList(_context.Users, "Id", "Login");
                return View(await msgContext.ToListAsync());
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
            var user = await _context.Users .FirstOrDefaultAsync(u => u.Login == log);
           
            var message = new Message
            {
                UserId = user.Id,
                Content = content,
                Date = DateTime.Now
            };

            if (ModelState.IsValid)
            {
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.UserId = new SelectList(_context.Users, "Id", "Login");
            return RedirectToAction("Index");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
