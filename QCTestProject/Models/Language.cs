using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QCTestProject.Models
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<Book> Books { get; set; }
        public Language()
        {
            Books = new List<Book>();
        }
    }
}
