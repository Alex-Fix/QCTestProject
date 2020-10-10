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
        private int OverridingCacheStorage = 5;

        private ApplicationContext _db;
        private IMemoryCache _cache;

        public CacheService(ApplicationContext db, IMemoryCache cache)
        {
            _db = db;
            _cache = cache;
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
            if(locCategory == null)
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
            if(locLanguage == null)
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
            if(locPublisher == null)
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

            _db.Books.Add(cacheItem.Book);

            int n = _db.SaveChanges();
            if(n > 0)
            {
                _cache.Set($"Books{cacheItem.Book.Id}", cacheItem.Book, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                });
            }
        }

        public async Task<CacheItem> GetCacheItem(int bookId)
        {
            Book book = null;
            Category category = null;
            Language language = null;
            Author author = null;
            Publisher publisher = null;
            if (!_cache.TryGetValue($"Books{bookId}", out book))
            {
                book = await _db.Books.FirstOrDefaultAsync(el => el.Id == bookId);
                if(book != null)
                {
                    _cache.Set($"Books{bookId}", book, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                    });
                }
            }
            if (!_cache.TryGetValue($"Authors{book.AuthorId}", out author))
            {
                author = await _db.Authors.FirstOrDefaultAsync(el => el.Id == book.AuthorId);
                if (author != null)
                {
                    _cache.Set($"Authors{book.AuthorId}", author, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                    });
                }
            }
            if (!_cache.TryGetValue($"Languages{book.LanguageId}", out language))
            {
                language = await _db.Languages.FirstOrDefaultAsync(el => el.Id == book.LanguageId);
                if (language != null)
                {
                    _cache.Set($"Languages{book.LanguageId}", language, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                    });
                }
            }
            if (!_cache.TryGetValue($"Publishers{book.PublisherId}", out publisher))
            {
                publisher = await _db.Publishers.FirstOrDefaultAsync(el => el.Id == book.PublisherId);
                if (publisher != null)
                {
                    _cache.Set($"Publishers{book.PublisherId}", publisher, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                    });
                }
            }
            if (!_cache.TryGetValue($"Categories{book.CategoryId}", out category))
            {
                category = await _db.Categories.FirstOrDefaultAsync(el => el.Id == book.CategoryId);
                if (category != null)
                {
                    _cache.Set($"Categories{book.CategoryId}", category, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OverridingCacheStorage)
                    });
                }
            }


            return new CacheItem
            {
                Category = category,
                Book = book,
                Language = language,
                Publisher = publisher,
                Author = author
            };
        }

        public async Task<CacheItems> GetCacheItems()
        {
            List<Book> books = await _db.Books.ToListAsync();
            List<Author> authors = await _db.Authors.ToListAsync();
            List<Language> languages = await _db.Languages.ToListAsync();
            List<Publisher> publishers = await _db.Publishers.ToListAsync();
            List<Category> categories = await _db.Categories.ToListAsync();
            return new CacheItems
            {
                Books = books,
                Authors = authors,
                Languages = languages,
                Publishers = publishers,
                Categories = categories
            };
        }

        public async Task DelCacheItem(int bookId)
        {
            Book book = null;
            Category category = null;
            Language language = null;
            Author author = null;
            Publisher publisher = null;

            if(_cache.TryGetValue($"Books{bookId}", out book))
            {
                _cache.Remove($"Books{bookId}");
            }
            book = await _db.Books.FirstOrDefaultAsync(el => el.Id == bookId);
            if(book != null)
            {
                if (_db.Books.Where(el => el.AuthorId == book.AuthorId).Count() == 1)
                {
                    if (_cache.TryGetValue($"Authors{book.AuthorId}", out author))
                    {
                        _cache.Remove($"Authors{book.AuthorId}");
                    }
                }
                if (_db.Books.Where(el => el.CategoryId == book.CategoryId).Count() == 1)
                {
                    if (_cache.TryGetValue($"Categories{book.CategoryId}", out category))
                    {
                        _cache.Remove($"Categories{book.CategoryId}");
                    }
                }
                if (_db.Books.Where(el => el.LanguageId == book.LanguageId).Count() == 1)
                {
                    if (_cache.TryGetValue($"Languages{book.LanguageId}", out language))
                    {
                        _cache.Remove($"Languages{book.LanguageId}");
                    }
                }
                if (_db.Books.Where(el => el.PublisherId == book.PublisherId).Count() == 1)
                {
                    if (_cache.TryGetValue($"Publishers{book.PublisherId}", out publisher))
                    {
                        _cache.Remove($"Publishers{book.PublisherId}");
                    }
                }
                _db.Books.Remove(book);
                _db.SaveChanges();
            }
        }

    }
}
