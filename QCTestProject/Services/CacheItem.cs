using QCTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCTestProject.Services
{
    public class CacheItem
    {
        public Book Book { get; set; }
        public Category Category { get; set; }
        public Author Author { get; set; }
        public Language Language { get; set; }
        public Publisher Publisher { get; set; }
    }
}
