using System.Diagnostics;
using GuestbookMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using GuestbookMVC;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using GuestbookMVC.Repository;
using System.Text;
using GuestbookMVC.Interfaces;
using Newtonsoft.Json;

namespace GuestbookMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IMessageRepository repo;
        private readonly IUserRepository _urepo;
        private readonly IPassword _passwordService;

        public HomeController(ILogger<HomeController> logger, IMessageRepository r, IUserRepository ur, IPassword passwordService)
        {
            _logger = logger;
            repo = r;
            _urepo = ur;
            _passwordService = passwordService;
        }


        public async Task<IActionResult> Index()
        {
            string? login = HttpContext.Session.GetString("Login") ?? Request.Cookies["login"];
            if (login != null)
            {
                HttpContext.Session.SetString("Login", login);
                var msgContext = await _urepo.GetList();
                ViewBag.UserId = new SelectList(msgContext, "Id", "Login");
            }
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            List<Message> list = await repo.GetList();
            var viewModelList = list.Select(m => new MessageViewModel
            {
                Id = m.Id,
                Content = m.Content,
                Date = m.Date,
                AuthorLogin = m.User.Login
            }).ToList();

            string response = JsonConvert.SerializeObject(viewModelList);// конвертируем в джейсон
            return Json(response);//возвращает джейсон данные джесонрезалт - наследник екшенрезалта.
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
                    string response = "Сообщение успешно добавлено!";
                return Json(response);
                }
            }
            return Problem("Проблемы при добавлении сообщения!");
        }







        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("login");
            return Json( "Пользователь успешно разлогинился!" );

        }







        /////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPost]
        public IActionResult LoginAsGuest(bool rememberMe)
        {
            HttpContext.Session.SetString("Login", "Guest");
            HttpContext.Session.SetString("FirstName", "Guest");
            
            return Json(new { success = true, message = "Пользователь успешно вошел как гость!" });
        }


        [HttpGet]
        public async Task<IActionResult> LoginP()
        {
            return Json("Авторизация");
        }

        [HttpPost]

        public async Task<IActionResult> Login(string logon,string pswd,bool rememberMe)
        {
            if (ModelState.IsValid)
            {
                if (!await _urepo.AnyUsers())//есть ли вообще в бд пользователи 
                {
                    return Json(new { success = false, message = "Нет пользователей!"});
                }
                var user = await _urepo.GetUserByLogin(logon);
                if (user == null)//есть ли такой логин
                {
                    return Json(new { success = false, message = "Неверный логин или пароль!" });
                }

                string hash = _passwordService.HashPassword(user.Salt, pswd);

                if (user.Password != hash)
                {
                    return Json(new { success = false, message = "Неверный логин или пароль!" });
                }

                //если все хорошо то чтобы запомнить юзера добавляем в сессию его данные(лучше имейл) 
                HttpContext.Session.SetString("Login", user.Login);
                HttpContext.Session.SetString("FirstName", user.FirstName);
                if (rememberMe)
                {
                    CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddDays(10); 
                    Response.Cookies.Append("login", logon, option); 
                }
                return Json(new { success = true, message = "Пользователь успешно вошел!"});
            }
            return Json(new { success = false, message = "Пользователь не вошел!" });
        }
        

        [HttpGet]
        public async Task<IActionResult> AddUserP()
        {
            return Json("Регистрация");
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(string logon, string pswd, string firstName, string lastName, string pswdConfirm)
        {

            if (ModelState.IsValid)
            {
                if (pswd != pswdConfirm)
                {
                    return Json(new { success = false, message = "Неверное подтверждение пароля!" });
                }
                User user = new User();
                user.FirstName = firstName;
                user.LastName = lastName;
                user.Login = logon;

                string salt = _passwordService.GenerateSalt();
                string hashedPassword = _passwordService.HashPassword(salt, pswd);


                user.Password = hashedPassword;
                user.Salt = salt;


                await _urepo.Create(user);
                await _urepo.Save();
                string response = "Пользователь успешно зарегистрирован!";
                return Json(new { success = true, message = response });
            }

            return Problem("Проблемы при добавлении пользователя!");
        }



    }
}
