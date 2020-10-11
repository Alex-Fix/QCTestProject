using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCTestProject.Services
{
    public class CacheItemsIds
    {
        public List<int> BooksIds { get; set; } = new List<int>();
        public List<int> AuthorsIds { get; set; } = new List<int>();
        public List<int> CategoriesIds { get; set; } = new List<int>();
        public List<int> PublishersIds { get; set; } = new List<int>();
        public List<int> LanguagesIds { get; set; } = new List<int>();
    }
}
