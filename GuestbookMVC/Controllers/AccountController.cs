using GuestbookMVC.Models;
using Microsoft.AspNetCore.Mvc;
using GuestbookMVC.Models;
using System.Security.Cryptography;//для захешированого пароля
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using GuestbookMVC.Repository;

namespace GuestbookMVC.Controllers
{//контроллер совершенно не знает с кем он работает (с субд или с эмулятором)

    public class AccountController : Controller
    {
        private readonly IUserRepository repo;

        public AccountController(IUserRepository r)//в интерфейсную ссылку будет передан обьект сервиса StudentRepository
        {
            repo = r;
        }
        //любой редирект это гет запрос всегда
        public IActionResult LoginAsGuest()
        {
            HttpContext.Session.SetString("Login", "Guest");// создание сессионной переменной моментально в отличии от куков 
            HttpContext.Session.SetString("FirstName", "Guest");
            //уникальный идентификатор сессии создается автоматически при первом заходе на сайт
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Login()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//атребут требует чтобы токен был перефицирован

        public async Task<IActionResult> Login(LoginModel logon)//пришли данные с формы авторизации. назвали logon а не login чтоб не было проблемы с привязкой
        {//так как хеширование необратимо то мы вводим пароль соеденяем с солью которая в базе данных и снова хешируем по новой наш пароль и сверяем только тогда
            if (ModelState.IsValid)
            {
                if (!await repo.AnyUsers())//есть ли вообще в бд пользователи 
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }
                var user = await repo.GetUserByLogin(logon.Login);
                if (user == null)//есть ли такой логин
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }
                string? salt = user.Salt;//получили соль из бд

                //переводим пароль в байт-массив  
                byte[] password = Encoding.Unicode.GetBytes(salt + logon.Password);//соеденяем вновь введенный пароль что пришел с солью

                //вычисляем хеш-представление в байтах  
                byte[] byteHash = SHA256.HashData(password);//хешируем пароль с солью
                //полученный  хеш переводим в строку
                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                if (user.Password != hash.ToString())//сравниваем два пароля , два хеша 
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }

                //если все хорошо то чтобы запомнить юзера добавляем в сессию его данные(лучше имейл) 
                HttpContext.Session.SetString("Login", user.Login);
                HttpContext.Session.SetString("FirstName", user.FirstName);
                if (logon.RememberMe)
                {
                    CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddDays(10); // срок хранения куки - 10 дней
                    Response.Cookies.Append("login", logon.Login, option); // создание куки. Заполнение служебных заголовков. Сохранение в куки будет уже после того как отдаем ответ браузеру 
                }
                return RedirectToAction("Index", "Home");//делаем редирект на стартовую страницу
            }
            return View(logon);
        }

        public IActionResult Register()
        {
            return View();//гет запрос при клике на гиперссылке 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel reg)//входной параметр обьект RegisterModel (валидируется) 
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.FirstName = reg.FirstName;
                user.LastName = reg.LastName;
                user.Login = reg.Login;

                byte[] saltbuf = new byte[16];
                //соль для усиления защиты, (создается рандомное уникальное солевое число длинное и склеивается с паролем потом хешируется необатимым алгоритом и потом в бд сохраняется )
                RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
                randomNumberGenerator.GetBytes(saltbuf);//заполняем байтовый массив солью

                StringBuilder sb = new StringBuilder(16);//лучше чем способ канкатенация 
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));

                //получили соль которую сканвертнули из байтового массива в стринг  
                string salt = sb.ToString();

                //переводим пароль соеденный с солью в байт-массив  
                byte[] password = Encoding.Unicode.GetBytes(salt + reg.Password);//соеденям соль с паролем и получаем байтовый массив 

                //вычисляем хеш-представление в байтах  
                byte[] byteHash = SHA256.HashData(password);//хешируем
                //опять переводим в строку 
                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                user.Password = hash.ToString();//сюда уже присваиваем пароль соедененный с солью и захешируемый
                user.Salt = salt;//соль рекомендуют отдельно хранить
                await repo.Create(user);
                await repo.Save();
                return RedirectToAction("Login");
            }

            return View(reg);
        }
    }
}