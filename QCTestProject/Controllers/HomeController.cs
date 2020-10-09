using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using QCTestProject.Mocks;
using QCTestProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace QCTestProject.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext _db;
        private readonly IWebHostEnvironment _env;

        public HomeController(ApplicationContext db, IWebHostEnvironment env, IMemoryCache cache)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FillDB()
        {
            if (_env.EnvironmentName.Equals("Development"))
            {
                Mock.FillDB(_db);
            }
            return RedirectToAction("Index");
        }

        public string GetAllItems()
        {
            List<Book> Books = _db.Books.ToList();
            List<Author> Authors = _db.Authors.ToList();
            List<Category> Categories = _db.Categories.ToList();
            List<Publisher> Publishers = _db.Publishers.ToList();
            List<Language> Languages = _db.Languages.ToList();

            Dictionary<string, string> Items = new Dictionary<string, string>
            {
                {"Books", JsonSerializer.Serialize<List<Book>>(Books) },
                {"Authors", JsonSerializer.Serialize<List<Author>>(Authors) },
                {"Categories", JsonSerializer.Serialize<List<Category>>(Categories) },
                {"Publishers", JsonSerializer.Serialize<List<Publisher>>(Publishers) },
                {"Languages", JsonSerializer.Serialize<List<Language>>(Languages) }
            };

            string result = JsonSerializer.Serialize<Dictionary<string, string>>(Items);

            return result;
        }

        [HttpPost]
        public  void PostAllItems([FromBody] List<string> result)
        {
            
            _db.Books.RemoveRange(_db.Books.ToList());
            _db.Categories.RemoveRange(_db.Categories.ToList());
            _db.Languages.RemoveRange(_db.Languages.ToList());
            _db.Authors.RemoveRange(_db.Authors.ToList());
            _db.Publishers.RemoveRange(_db.Publishers.ToList());

            List<Language> languages = new List<Language>();
            foreach (var el in JsonSerializer.Deserialize<List<Language>>(result[2])) 
            {
                languages.Add(new Language
                {
                    Name = el.Name
                }); 
            }

            List<Category> categories = new List<Category>();
            foreach (var el in JsonSerializer.Deserialize<List<Category>>(result[3]))
            {
                categories.Add(new Category
                {
                    Name = el.Name
                });
            }

            List<Publisher> publishers = new List<Publisher>();
            foreach (var el in JsonSerializer.Deserialize<List<Publisher>>(result[4]))
            {
                publishers.Add(new Publisher
                {
                    Name = el.Name
                });
            }

            List<Author> authors = new List<Author>();
            foreach (var el in JsonSerializer.Deserialize<List<Author>>(result[1]))
            {
                authors.Add(new Author
                {
                    FirstName = el.FirstName,
                    LastName = el.LastName
                });
            }

            


            _db.Categories.AddRange(categories);
            _db.Languages.AddRange(languages);
            _db.Publishers.AddRange(publishers);
            _db.Authors.AddRange(authors);

            _db.SaveChanges();

            List<Book> books = new List<Book>();
            foreach (var el in JsonSerializer.Deserialize<List<Book>>(result[0]))
            {
                books.Add(new Book
                {
                    Title = el.Title,
                    Year = el.Year,
                    CountOfPages = el.CountOfPages,
                    Author = _db.Authors.FirstOrDefault(p => p.FirstName.Equals(el.Author.FirstName)&&p.LastName.Equals(el.Author.LastName)),
                    Language = _db.Languages.FirstOrDefault(p => p.Name.Equals(el.Language.Name)),
                    Category =  _db.Categories.FirstOrDefault(p => p.Name.Equals(el.Category.Name)),
                    Publisher = _db.Publishers.FirstOrDefault(p => p.Name.Equals(el.Publisher.Name))
                });
            }


            
            
           _db.Books.AddRange(books);
           _db.SaveChanges();
        }

    }

    
}
