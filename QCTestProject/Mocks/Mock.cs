using QCTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCTestProject.Mocks
{
    public static class Mock
    {

        /// <summary>
        /// Function for test filling of a database.
        /// </summary>
        /// <param name="db"></param>
        public static void FillDB(ApplicationContext db)
        {
            db.Books.RemoveRange(db.Books.ToList());
            db.Authors.RemoveRange(db.Authors.ToList());
            db.Categories.RemoveRange(db.Categories.ToList());
            db.Languages.RemoveRange(db.Languages.ToList());
            db.Publishers.RemoveRange(db.Publishers.ToList());
            


            List<Author> Authors = new List<Author>
            {
                new Author
                {
                    FirstName = "Луїза",
                    LastName = "Олкотт"
                },
                new Author
                {
                    FirstName = "Лоран",
                    LastName = "Гунель"
                },
                new Author
                {
                    FirstName = "Джон",
                    LastName = "Стрелекі"
                },
                new Author
                {
                    FirstName = "Люко",
                    LastName = "Дашвар"
                },
                new Author
                {
                    FirstName = "Віктор",
                    LastName = "Пелевін"
                }
            };


            List<Language> Languages = new List<Language>
            {
                new Language
                {
                    Name = "Російська"
                },
                new Language
                {
                    Name = "Українська"
                }
            };


            List<Publisher> Publishers = new List<Publisher>
            {
                new Publisher
                {
                    Name = "Азбука"
                },
                new Publisher
                {
                    Name = "Книжковий клуб \"Сімейного дозвілля\""
                },
                new Publisher
                {
                    Name = "Vivat"
                },
                new Publisher
                {
                    Name = "Форс"
                }
            };


            List<Category> Categories = new List<Category>
            {
                new Category
                {
                    Name = "Класика дитячої літератури"
                },
                new Category
                {
                    Name = "Сучасна проза"
                }
            };


            List<Book> Books = new List<Book>
            {
                new Book
                {
                    Title = "Маленькие женщины",
                    Year = 2017,
                    CountOfPages = 384,
                    Author = Authors[0],
                    Publisher = Publishers[0],
                    Language = Languages[0],
                    Category = Categories[0]
                },

                new Book
                {
                    Title = "Бог завжди погрожує інкогніто",
                    Year = 2016,
                    CountOfPages = 416,
                    Author = Authors[1],
                    Publisher = Publishers[1],
                    Language = Languages[1],
                    Category = Categories[1]
                },

                new Book
                {
                    Title = "Кафе на краю світу",
                    Year = 2019,
                    CountOfPages = 128,
                    Author = Authors[2],
                    Publisher = Publishers[2],
                    Language = Languages[1],
                    Category = Categories[1]
                },

                new Book
                {
                    Title = "#ГАЛЯБЕЗГОЛОВИ",
                    Year = 2020,
                    CountOfPages = 400,
                    Author = Authors[3],
                    Publisher = Publishers[1],
                    Language = Languages[1],
                    Category = Categories[1]
                },

                new Book
                {
                    Title = "Непобедимое солнце",
                    Year = 2020,
                    CountOfPages = 704,
                    Author = Authors[4],
                    Publisher = Publishers[3],
                    Language = Languages[0],
                    Category = Categories[1]
                }
            };


            db.Books.AddRange(Books);
            db.Authors.AddRange(Authors);
            db.Categories.AddRange(Categories);
            db.Languages.AddRange(Languages);
            db.Publishers.AddRange(Publishers);
            db.SaveChanges();
        }
    }
}
