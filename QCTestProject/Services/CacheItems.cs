using QCTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCTestProject.Services
{
    public class CacheItems
    {
        public List<Book> Books { get; set; }
        public List<Author> Authors { get; set; }
        public List<Category> Categories { get; set; }
        public List<Language> Languages { get; set; }
        public List<Publisher> Publishers { get; set; }
    }
}
