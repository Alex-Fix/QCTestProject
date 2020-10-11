using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using QCTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCTestProject.Services
{
    public class CacheService
    {
        //The cache lasts () minutes
        private int OverridingCacheStorage = 10;

        private ApplicationContext _db;
        private IMemoryCache _cache;

        public CacheService(ApplicationContext db, IMemoryCache cache)
        {
            _db = db;
            _cache = cache;
        }

        public async Task<CacheItemsIds> GetCacheItemsIds()
        {
            CacheItemsIds ids = new CacheItemsIds();
            List<Book> books = await _db.Books.OrderByDescending(el => el.Id).ToListAsync();
            List<Author> authors = await _db.Authors.OrderByDescending(el => el.Id).ToListAsync();
            List<Language> languages = await _db.Languages.OrderByDescending(el => el.Id).ToListAsync();
            List<Publisher> publishers = await _db.Publishers.OrderByDescending(el => el.Id).ToListAsync();
            List<Category> categories = await _db.Categories.OrderByDescending(el => el.Id).ToListAsync();
            foreach (var book in books)
            {
                ids.BooksIds.Add(book.Id);
                _cache.Set($"Books{book.Id}", book, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                });
            }
            foreach (var author in authors)
            {
                ids.AuthorsIds.Add(author.Id);
                _cache.Set($"Authors{author.Id}", author, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                });
            }
            foreach (var category in categories)
            {
                ids.CategoriesIds.Add(category.Id);
                _cache.Set($"Categories{category.Id}", category, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                });
            }
            foreach (var language in languages)
            {
                ids.LanguagesIds.Add(language.Id);
                _cache.Set($"Languages{language.Id}", language, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                });
            }
            foreach (var publisher in publishers)
            {
                ids.PublishersIds.Add(publisher.Id);
                _cache.Set($"Publishers{publisher.Id}", publisher, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                });
            }
            return ids;
        }

        public async Task<List<Book>> GetBooks(List<int> bookIds){
            List<Book> books = new List<Book>();
            foreach(var id in bookIds)
            {
                Book book = new Book();
                if (!_cache.TryGetValue($"Books{id}", out book))
                {
                    book = await _db.Books.FirstOrDefaultAsync(el => el.Id == id);
                    if (book != null)
                    {
                        _cache.Set($"Books{id}", book, new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                        });
                    }
                }
                if(book!= null)
                {
                    books.Add(book);
                }
            }
            return books;
        }

        public async Task<List<Author>> GetAuthors(List<int> authorsIds)
        {
            List<Author> authors = new List<Author>();
            foreach(var id in authorsIds)
            {
                Author author = new Author();
                if(!_cache.TryGetValue($"Authors{id}", out author))
                {
                    author = await _db.Authors.FirstOrDefaultAsync(el => el.Id == id);
                    if(author != null)
                    {
                        _cache.Set($"Authors{id}", author, new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                        });
                    }
                }
                if(author!= null)
                {
                    authors.Add(author);
                }
            }
            return authors;
        }

        public async Task<List<Category>> GetCategories(List<int> categoriesIds)
        {
            List<Category> categories = new List<Category>();
            foreach (var id in categoriesIds)
            {
                Category category = new Category();
                if (!_cache.TryGetValue($"Categories{id}", out category))
                {
                    category = await _db.Categories.FirstOrDefaultAsync(el => el.Id == id);
                    if (category != null)
                    {
                        _cache.Set($"Categories{id}", category, new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                        });
                    }
                }
                if (category != null)
                {
                    categories.Add(category);
                }
            }
            return categories;
        }

        public async Task<List<Language>> GetLanguages(List<int> languagesIds)
        {
            List<Language> languages = new List<Language>();
            foreach (var id in languagesIds)
            {
                Language language = new Language();
                if (!_cache.TryGetValue($"Languages{id}", out language))
                {
                    language = await _db.Languages.FirstOrDefaultAsync(el => el.Id == id);
                    if (language != null)
                    {
                        _cache.Set($"Languages{id}", language, new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                        });
                    }
                }
                if (language != null)
                {
                    languages.Add(language);
                }
            }
            return languages;
        }

        public async Task<List<Publisher>> GetPublishers(List<int> publishersIds)
        {
            List<Publisher> publishers = new List<Publisher>();
            foreach (var id in publishersIds)
            {
                Publisher publisher = new Publisher();
                if (!_cache.TryGetValue($"Publishers{id}", out publisher))
                {
                    publisher = await _db.Publishers.FirstOrDefaultAsync(el => el.Id == id);
                    if (publisher != null)
                    {
                        _cache.Set($"Publishers{id}", publisher, new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                        });
                    }
                }
                if (publisher != null)
                {
                    publishers.Add(publisher);
                }
            }
            return publishers;
        }

        public  void DelCacheItem(int bookId)
        {
            Book book = null;
            Category category = null;
            Language language = null;
            Author author = null;
            Publisher publisher = null;

            if (_cache.TryGetValue($"Books{bookId}", out book))
            {
                _cache.Remove($"Books{bookId}");
            }
            book =  _db.Books.FirstOrDefault(el => el.Id == bookId);
           
            if (book != null)
            {
                if (_db.Books.Where(el => el.AuthorId == book.AuthorId).Count() == 1)
                {
                    if (_cache.TryGetValue($"Authors{book.AuthorId}", out author))
                    {
                        _cache.Remove($"Authors{book.AuthorId}");
                    }
                    author = _db.Authors.FirstOrDefault(el => book.AuthorId == el.Id);
                    if(author!= null)
                    {
                        _db.Authors.Remove(author);
                    }
                }
                if (_db.Books.Where(el => el.CategoryId == book.CategoryId).Count() == 1)
                {
                    if (_cache.TryGetValue($"Categories{book.CategoryId}", out category))
                    {
                        _cache.Remove($"Categories{book.CategoryId}");
                    }
                    category = _db.Categories.FirstOrDefault(el => book.CategoryId == el.Id);
                    if (category != null)
                    {
                        _db.Categories.Remove(category);
                    }
                }
                if (_db.Books.Where(el => el.LanguageId == book.LanguageId).Count() == 1)
                {
                    if (_cache.TryGetValue($"Languages{book.LanguageId}", out language))
                    {
                        _cache.Remove($"Languages{book.LanguageId}");
                    }
                    language = _db.Languages.FirstOrDefault(el => book.LanguageId == el.Id);
                    if (language != null)
                    {
                        _db.Languages.Remove(language);
                    }
                }
                if (_db.Books.Where(el => el.PublisherId == book.PublisherId).Count() == 1)
                {
                    if (_cache.TryGetValue($"Publishers{book.PublisherId}", out publisher))
                    {
                        _cache.Remove($"Publishers{book.PublisherId}");
                    }
                    publisher = _db.Publishers.FirstOrDefault(el => book.PublisherId == el.Id);
                    if (publisher != null)
                    {
                        _db.Publishers.Remove(publisher);
                    }
                }
                _db.Books.Remove(book);
                _db.SaveChanges();
            }
        }

        public void AddCacheItem(CacheItem cacheItem)
        {
            Author locAuthor = _db.Authors.FirstOrDefault(el => el.Id == cacheItem.Author.Id);
            if (locAuthor == null)
            {
                _cache.Set($"Authros{cacheItem.Author.Id}", cacheItem.Author, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                });
                cacheItem.Author.Id = 0;
                _db.Authors.Add(cacheItem.Author);
                locAuthor = cacheItem.Author;
            }
            cacheItem.Book.Author = locAuthor;

            Category locCategory = _db.Categories.FirstOrDefault(el => el.Id == cacheItem.Category.Id);
            if (locCategory == null)
            {
                _cache.Set($"Categories{cacheItem.Category.Id}", cacheItem.Category, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                });
                cacheItem.Category.Id = 0;
                _db.Categories.Add(cacheItem.Category);
                locCategory = cacheItem.Category;
            }
            cacheItem.Book.Category = locCategory;

            Language locLanguage = _db.Languages.FirstOrDefault(el => el.Id == cacheItem.Language.Id);
            if (locLanguage == null)
            {
                _cache.Set($"Languages{cacheItem.Language.Id}", cacheItem.Language, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                });
                cacheItem.Language.Id = 0;
                _db.Languages.Add(cacheItem.Language);
                locLanguage = cacheItem.Language;
            }
            cacheItem.Book.Language = locLanguage;

            Publisher locPublisher = _db.Publishers.FirstOrDefault(el => el.Id == cacheItem.Publisher.Id);
            if (locPublisher == null)
            {
                _cache.Set($"Publishers{cacheItem.Publisher.Id}", cacheItem.Publisher, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                });
                cacheItem.Publisher.Id = 0;
                _db.Publishers.Add(cacheItem.Publisher);
                locPublisher = cacheItem.Publisher;
            }
            cacheItem.Book.Publisher = locPublisher;

            cacheItem.Book.Id = 0;
            _db.Books.Add(cacheItem.Book);

            int n = _db.SaveChanges();
            if (n > 0)
            {
                _cache.Set($"Books{cacheItem.Book.Id}", cacheItem.Book, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                });
            }
        }

    }
}
