using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC;

//Создавались с помощью средств вижуал студии Add Controller MVC Controller with views using EntityFrameWork по классам модели и используя мастер страницу
namespace MVC.Controllers
{
    public class FilmsController : Controller
    {//Конроллер полностью обеспечивает нам реализацию выполнения крад операций 
        private readonly FilmContext _context;

        // IWebHostEnvironment предоставляет информацию об окружении, в котором запущено приложение
        IWebHostEnvironment _appEnvironment;//нам нужно будет получить абсолютный путь к корневой папке сайта

        public FilmsController(FilmContext context,IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: Films
        public async Task<IActionResult> Index()
        {
            var filmContext = _context.Films.Include(f => f.Genre);

            //передаем во вью дженерик коллекцию 
            //@model IEnumerable<StudentsMVC.Models.Student>
            //Теперь вьюшка читает модель напрямую
            //представление строго типизируется по модели(круто ибо компилятор ошибки замечает)

            return View(await filmContext.ToListAsync());//Асинхронность нам всегда нужна  
        }

       

        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Films == null)
            {
                return NotFound();//вернет 404 статус и отобразит 
            }

            var film = await _context.Films
                .Include(f => f.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);//делаем запрос с фильтром 
            if (film == null)
            {
                return NotFound();
            }
            //@model StudentsMVC.Models.Student
            //@* вьюшка теперь типизируется по модели студента по одному студенту а не коллекции *@
            return View(film);//опять строгая типизация 
        }
        //с точки зрения асп нет это один екшн Create из двух методов, потому что один маршрут

        // GET: Films/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");//SelectList своего рода коллекция к которой будет привязан комбобокс 
            return View();//передавать не можем сюда коллекцию команд потомучто вьюшки все не типизируются по командам
        }





        // Все загружаемые файлы в ASP.NET Core представлены типом IFormFile
        // из пространства имен Microsoft.AspNetCore.Http

        // POST: Films/Create
        [HttpPost]
        [ValidateAntiForgeryToken]//защита от поддельных запросов ( если токен не пришел-злоумышлиники ) 
        [RequestSizeLimit(1000000000)]//размер обьема данных отправляемый пост запросом
        //[Bind("Id,Name,Surname,Age,GPA")]  задает к каакому свойству будет привязка
        public async Task<IActionResult> Create([Bind("Id,Name,Maker,Genre,Year,Poster,Description,GenreId")] Film film, IFormFile uploadedFile)//uploadedFile название должно как в вьюшке совпадать
        {    //собственная проверка своя серверная логика , серверная валидация джаваскрипт уже не задействован
            // Пустая строка, передаваемая первому параметру метода, указывает,
            // что данная ошибка относится ко всей модели в целом, а не к отдельному свойству.

            if (uploadedFile == null)
                ModelState.AddModelError("", "Необходимо загрузить постер.");

            if (uploadedFile != null)
            {
                // Путь к папке Files
                string path = "/Files/" + uploadedFile.FileName; // имя файла

                // Сохраняем файл в папку Files в каталоге wwwroot
                // Для получения полного пути к каталогу wwwroot
                // применяется свойство WebRootPath объекта IWebHostEnvironment
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream); // копируем файл в поток
                }
                film.Poster = path;
            }
            //собственная проверка своя серверная логика , серверная валидация джаваскрипт уже не задействован

            if (film.Year <= 0)
                ModelState.AddModelError("Year", "Год должен быть больше нуля");//Year ключ ошибка связана с конкретным свойством а не с моделью в целом , появится ошибка под текстовым полем возраста а не над формой
            if (ModelState.IsValid)//в целом проверяет каждое свойство соттвествие требованийм
            {
                _context.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            return View(film);

          
        }

        // GET: Films/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Films == null)
            {
                return NotFound();
            }

            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", film.GenreId);
            return View(film);//передали заполненную модель и вьюшка будет типизироваться по этой модели
                              //передача данных будет в представлении поэтому передаем обьект
        }

        // POST: Films/Edit/5
        [HttpPost]//указываем явно атрибутом ибо по умолчанию GET запрос 
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(1000000000)]//размер обьема данных отправляемый пост запросом
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Maker,Genre,Year,Poster,Description,GenreId")] Film film, IFormFile uploadedFile)//Players players идентификатор внешний ключ который указывает на команду этого игрока. Чтоб выбран не первый в бд
        {//uploadedFile название должно как в вьюшке совпадать
            if (id != film.Id)
            {
                return NotFound();
            }


            if (uploadedFile == null)
                ModelState.AddModelError("", "Необходимо загрузить постер.");

            if (uploadedFile != null)
            {
                // Путь к папке Files
                string path = "/Files/" + uploadedFile.FileName; // имя файла

                // Сохраняем файл в папку Files в каталоге wwwroot
                // Для получения полного пути к каталогу wwwroot
                // применяется свойство WebRootPath объекта IWebHostEnvironment
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream); // копируем файл в поток
                }
                film.Poster = path;
            }
            if (film.Year <= 0)
                ModelState.AddModelError("Year", "Год должен быть больше нуля");//Year ключ ошибка связана с конкретным свойством а не с моделью в целом , появится ошибка под текстовым полем возраста а не над формой

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(film);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", film.GenreId);
            return View(film);

        }

        // GET: Films/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Films == null)
            {
                return NotFound();
            }

            var film = await _context.Films
               .Include(f => f.Genre)
               .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]//но не хочется менять название маршрута на DeleteConfirmed поэтому нужен доп атрибут
        [ValidateAntiForgeryToken]//проверяет что токен пришел нормальный а не от злоумышлинника
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Films == null)
            {
                return Problem("Entity set 'FilmContext.Films'  is null.");
            }
            var film = await _context.Films.FindAsync(id);
            if (film != null)
            {
                _context.Films.Remove(film);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return (_context.Films?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
