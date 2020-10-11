using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using QCTestProject.Mocks;
using QCTestProject.Models;
using QCTestProject.Services;
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
        private CacheService _cache;

        public HomeController(ApplicationContext db, IWebHostEnvironment env, CacheService cache)
        {
            _db = db;
            _env = env;
            _cache = cache;
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

        public async Task<string> GetItemIds()
        {
            CacheItemsIds ids = await _cache.GetCacheItemsIds();
            return JsonSerializer.Serialize<CacheItemsIds>(ids);
        }

        public async Task<string> GetBooks([FromBody] List<int> ids)
        {
            List<Book> books = await _cache.GetBooks(ids);
            return JsonSerializer.Serialize<List<Book>>(books);
        }

        public async Task<string> GetAuthors([FromBody] List<int> ids)
        {
            List<Author> authors = await _cache.GetAuthors(ids);
            return JsonSerializer.Serialize<List<Author>>(authors);
        }

        public async Task<string> GetLanguages([FromBody] List<int> ids)
        {
            List<Language> languages = await _cache.GetLanguages(ids);
            return JsonSerializer.Serialize<List<Language>>(languages);
        }

        public async Task<string> GetPublishers([FromBody] List<int> ids)
        {
            List<Publisher> publishers = await _cache.GetPublishers(ids);
            return JsonSerializer.Serialize<List<Publisher>>(publishers);
        }

        public async Task<string> GetCategories([FromBody] List<int> ids)
        {
            List<Category> categories = await _cache.GetCategories(ids);
            return JsonSerializer.Serialize<List<Category>>(categories);
        }

        
        [HttpPost]
        public void SyncWithServerForAdd([FromBody] List<string> result)
        {
            List<Book> addBooks = JsonSerializer.Deserialize<List<Book>>(result[0]);
            List<Author> addAuthors = JsonSerializer.Deserialize<List<Author>>(result[1]);
            List<Category> addCategories = JsonSerializer.Deserialize<List<Category>>(result[2]);
            List<Publisher> addPublishers = JsonSerializer.Deserialize<List<Publisher>>(result[3]);
            List<Language> addLanguages = JsonSerializer.Deserialize<List<Language>>(result[4]);

            foreach(var el in addBooks)
            {
                CacheItem cacheItem = new CacheItem
                {
                    Book = el,
                    Author = addAuthors.FirstOrDefault(p => p.Id == el.AuthorId),
                    Category = addCategories.FirstOrDefault(p => p.Id == el.CategoryId),
                    Publisher = addPublishers.FirstOrDefault(p => p.Id == el.PublisherId),
                    Language = addLanguages.FirstOrDefault(p => p.Id == el.LanguageId),
                };
                
                _cache.AddCacheItem(cacheItem);
            }
        }

        [HttpPost]
        public void SyncWithServerForDell([FromBody] List<int> ids)
        {
            foreach (var el in ids)
            {
                _cache.DelCacheItem(el);
            }
        }


    }

    
}
