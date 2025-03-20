using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC;

namespace MVC.Controllers
{
    public class FilmsController : Controller
    {//Конроллер полностью обеспечивает нам реализацию выполнения крад операций 
        private readonly FilmContext _context;

        public FilmsController(FilmContext context)
        {
            _context = context;
        }

        // GET: Films
        public async Task<IActionResult> Index()
        {
            //передаем во вью дженерик коллекцию 
            //@model IEnumerable<StudentsMVC.Models.Student>
            //Теперь вьюшка читает модель напрямую
            //представление строго типизируется по модели(круто ибо компилятор ошибки замечает)

            return View(await _context.Films.ToListAsync());//Асинхронность нам всегда нужна  
        }

       

        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();//вернет 404 статус и отобразит 
            }

            var film = await _context.Films
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
            return View();
        }

        // POST: Films/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]//защита от поддельных запросов ( если токен не пришел-злоумышлиники ) 

        //[Bind("Id,Name,Surname,Age,GPA")]  задает к каакому свойству будет привязка
        public async Task<IActionResult> Create([Bind("Id,Name,Maker,Genre,Year,Poster,Description")] Film film)
        {
            if (ModelState.IsValid)//стандартная валидация всей модели в целом 
            {
                _context.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));//переход на стартовую страницу
            }
            return View(film);
        }

        // GET: Films/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);//передали заполненную модель и вьюшка будет типизироваться по этой модели
        }

        // POST: Films/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]//указываем явно атрибутом ибо по умолчанию GET запрос 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Maker,Genre,Year,Poster,Description")] Film film)
        {
            if (id != film.Id)
            {
                return NotFound();
            }

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
                        throw;//серверная ошибка 500-
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        // GET: Films/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films
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
            return _context.Films.Any(e => e.Id == id);
        }
    }
}
