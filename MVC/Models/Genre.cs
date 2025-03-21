using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;//для валидации чтоб работать с метаданными
namespace MVC
{
    public class Genre
    {
        public Genre()
        {
            this.Films = new HashSet<Film>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле должно быть установлено")]//атрибутивный подход клиентской  валиидацаии  аннотации span asp-validation-for
        [Display(Name = "Title of genre")]//просто визуализация пригодится если использовать таблицу ресурсов для перевода языка исходя из ОС
        [Remote(action: "CheckName", controller: "Genres", ErrorMessage = "Жанр опасен для психики")]//отправляет аякс запрос серверу и на сервере отработает логика . Валидация тоже клиенская но с нашей уже логикой

        public string? Name { get; set; }
        public ICollection<Film>? Films { get; set; }
    }


    /*
      [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]//указываем регулярное выражение
     
      [DataType(DataType.Password)]
      public string Password { get; set; }

      [Compare("Password",ErrorMessage="Пароли не совпадают")]
      [DataType(DataType.Password)]
      public  string PasswordConfirm { get; set; }

      Currency Отображает текст в виде валюты
      DateTime Отображает дату и время
      Date Отображает только дату, без времени
      Time Отображает только время
      Text Отображает однострочный текст
      MultilineText Отображает многострочный текст (элемент textarea)
      Password Отображает символы с использованием маски
      Url  Отображает строку URL
      EmailAddress Отображает электронный адрес
       * */
}
