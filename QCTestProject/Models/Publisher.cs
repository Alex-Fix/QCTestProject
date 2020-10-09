﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QCTestProject.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<Book> Books { get; set; }
        public Publisher()
        {
            Books = new List<Book>();
        }
    }
}
