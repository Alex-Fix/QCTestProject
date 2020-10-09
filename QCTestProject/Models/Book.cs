using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCTestProject.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public int CountOfPages { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
