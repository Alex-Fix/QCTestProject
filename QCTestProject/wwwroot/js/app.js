$(function () {

    const UpdateTime = 10000; // ms

    let Books = [];
    let Categories = new Map();
    let Authors = new Map();
    let Languages = new Map();
    let Publishers = new Map();

    let dataWasChanged = false;

    // Initial

    $.ajax({
        url: "/Home/GetAllItems",
    }).then(function (result) {
        initItems(result);
    });

    // Function for filling data arrays
    function initItems(result) {
        AllItems = JSON.parse(result);

        JSON.parse(AllItems["Books"]).forEach(function (curValue, index, array) {
            Books.push(curValue);
        });
        Books.sort((a, b) => Number(b.Id) - Number(a.Id) );

        JSON.parse(AllItems["Categories"]).forEach(function (curValue, index, array) {
            Categories.set(curValue["Id"], curValue);
        });

        JSON.parse(AllItems["Authors"]).forEach(function (curValue, index, array) {
            Authors.set(curValue["Id"], curValue);
        });

        JSON.parse(AllItems["Languages"]).forEach(function (curValue, index, array) {
            Languages.set(curValue["Id"], curValue);
        });

        JSON.parse(AllItems["Publishers"]).forEach(function (curValue, index, array) {
            Publishers.set(curValue["Id"], curValue);
        });

        FillFormSelect();
        ShowItems();
    }

    //Fill form select
    function FillFormSelect() {
        //Categories
        let cat = [];
        for (let el of Categories.values()) {
            cat.push(el);
        }
        $("#formCategoryId").empty();
        $("#formCategoryId").append($('<option>', {value: -1, text: "New Category"}));
        cat.sort((a, b) => a.Name < b.Name);
        for (let category of cat) {
            $('<option>', { value: category.Id, text: category.Name }).appendTo("#formCategoryId");
        }

        //Publishers
        let pub = [];
        for (let el of Publishers.values()) {
            pub.push(el);
        }
        pub.sort((a, b) => a.Name < b.Name);
        $("#formPublisherId").empty();
        $("#formPublisherId").append($('<option>', { value: -1, text: "New Publisher" }));
        for (let publisher of pub) {
            $('<option>', { value: publisher.Id, text: publisher.Name }).appendTo("#formPublisherId");
        }

        //Languages
        let lan = [];
        for (let el of Languages.values()) {
            lan.push(el);
        }
        lan.sort((a, b) => a.Name < b.Name);
        $("#formLanguageId").empty();
        $("#formLanguageId").append($('<option>', { value: -1, text: "New Language" }));
        for (let language of lan) {
            $('<option>', { value: language.Id, text: language.Name }).appendTo("#formLanguageId");
        }

        //Authors
        let auth = [];
        for (let el of Authors.values()) {
            auth.push(el);
        }
        auth.sort((a, b) => `${a.FirstName}${a.LastName}` < `${b.FirstName}${b.LastName}`);
        $("#formAuthorId").empty();
        $("#formAuthorId").append($('<option>', { value: -1, text: "New Author" }));
        for (let author of auth) {
            $('<option>', { value: author.Id, text: `${author.FirstName} ${author.LastName}` }).appendTo("#formAuthorId");
        }
    }


    //Show all books on page
    function ShowItems() {
        
        for (let book of Books) {

            makeBookItem(book).appendTo("#AllBooks");
            
        }
    }

    function makeBookItem(book) {
        let element = $('<div>', {
            class: 'row pt-5 pb-5 align-items-center border-bottom',
            "data-book_item_id": book.Id
        }).append($('<div>', {
            class: "col mb-3 align-items-center",
            text: `Id: ${book.Id}`
        })).append($('<div>', {
            class: "col mb-3 align-items-center",
            text: `Title: ${book.Title}`
        })).append($('<div>', {
            class: "col mb-3 align-items-center",
            text: `Author: ${Authors.get(book.AuthorId).FirstName} ${Authors.get(book.AuthorId).LastName}`
        })).append($('<div>', {
            class: "w-100"
        })).append($('<div>', {
            class: "col mt-3 mb-3 align-items-center",
            text: `Count of pages: ${book.CountOfPages}`
        })).append($('<div>', {
            class: "col mt-3 mb-3 align-items-center",
            text: `Year of publishing: ${book.Year}`
        })).append($('<div>', {
            class: "col mt-3 mb-3 align-items-center",
            text: `Language: ${Languages.get(book.LanguageId).Name}`
        })).append($('<div>', {
            class: "w-100"
        })).append($('<div>', {
            class: "col mt-3 align-items-center",
            text: `Publisher: ${Publishers.get(book.PublisherId).Name}`
        })).append($('<div>', {
            class: "col mt-3 align-items-center",
            text: `Category: ${Categories.get(book.CategoryId).Name}`
        })).append($('<div>', {
            class: "col mt-3 align-items-center",
        }).append($('<a>', {
            class: "btn btn-primary w-50",
            "data-book_id": book.Id,
            text: "Delete",
        })));
        return element;
    }


    // Del book from page and array
    $("#AllBooks").on("click", "[data-book_id]", function (event) {
        event.preventDefault();

        let id = $(this).attr("data-book_id");

        let bookIndex = Books.findIndex(function (item, index, array) {
            return item.Id == id;
        });

        if (bookIndex != -1) {
            let book = Books[bookIndex];

            let countOfAuthors = 0;
            let countOfCategories = 0;
            let countOfPublishers = 0;
            let countOfLanguages = 0;
            for (let el of Books) {
                if (el.AuthorId == Books[bookIndex].AuthorId) {
                    countOfAuthors++;
                }
                if (el.LanguageId == Books[bookIndex].LanguageId) {
                    countOfLanguages++;
                }
                if (el.PublisherId == Books[bookIndex].PublisherId) {
                    countOfPublishers++;
                }
                if (el.CategoryId == Books[bookIndex].CategoryId) {
                    countOfCategories++;
                }
            }

            // If this book was the last, it is associated with a row of the following tables. Then this line is deleted
            if (countOfCategories === 1) {
                Categories.delete(book.CategoryId);
            }
            if (countOfLanguages === 1) {
                Languages.delete(book.LanguageId);
            }
            if (countOfAuthors === 1) {
                Authors.delete(book.AuthorId);
            }
            if (countOfPublishers === 1) {
                Publishers.delete(book.PublisherId);
            }

            //Delete book from array
            Books.splice(bookIndex, 1);


            //Delete book from page
            for (let el of $("[data-book_item_id]")) {
                if (el.getAttribute("data-book_item_id") == book.Id) {
                    el.remove();
                    break;
                }
            }

            FillFormSelect();
            dataWasChanged = true;
        }
    });


    //Add new book
    
    class Book {
        constructor(Id, Title, Year, CountOfPages, AuthorId, Author,
            PublisherId, Publisher, LanguageId, Language, CategoryId, Category) {
            this.Id = Id;
            this.Title = Title;
            this.Year = Year;
            this.CountOfPages = CountOfPages;
            this.AuthorId = AuthorId;
            this.Author = Author;
            this.PublisherId = PublisherId;
            this.Publisher = Publisher;
            this.LanguageId = LanguageId;
            this.Language = Language;
            this.CategoryId = CategoryId;
            this.Category = Category;
        }

    }

    class Author {
        constructor(Id, FirstName, LastName) {
            this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
        }
    }

    class Publisher {
        constructor(Id, Name) {
            this.Id = Id;
            this.Name = Name;
        }
    }

    class Language {
        constructor(Id, Name) {
            this.Id = Id;
            this.Name = Name;
        }
    }

    class Category {
        constructor(Id, Name) {
            this.Id = Id;
            this.Name = Name;
        }
    }

    // Add new book
    $("#formSubmit").on("click", function (event) {
        event.preventDefault();
        $("#fromErrors").empty();

        let ModelValid = true;
        let ModelErrors = [];

        ///////////////////////////////////////////
        //  Start validation
        //////////////////////////////////////////


        //Title
        let Title = $("#formTitle").val();
        if (Title.replace(/ +/g, ' ').trim().length < 2 || Title.replace(/ +/g, ' ').trim().length > 50) {
            ModelValid = false;
            ModelErrors.push("Title must be longer than 1 characters and less than 50");
        }

        //CountOfPages
        let CountOfPages = parseInt($("#formCountOfPages").val());
        if (isNaN(CountOfPages) || CountOfPages < 0) {
            ModelValid = false;
            ModelErrors.push("Count of pages must be greater than 0");
        }

        //Year
        let Year = parseInt($("#formYear").val());
        if (isNaN(CountOfPages) || Year < 0) {
            ModelValid = false;
            ModelErrors.push("Year of publication must be greater than 0");
        }

        //Author
        let AuthorId = parseInt($("#formAuthorId").val());
        if (isNaN(AuthorId)) {
            ModelValid = false;
            ModelErrors.push("AuthorId isn`t valid");
        }

        let AuthorFirstName = $("#formAuthorFirstName").val();
        if (AuthorId == -1 && (AuthorFirstName.replace(/ +/g, ' ').trim().length < 2 || AuthorFirstName.replace(/ +/g, ' ').trim().length > 30)) {
            ModelValid = false;
            ModelErrors.push("Author first name must be longer than 1 characters and less than 30");
        }

        let AuthorLastName = $("#formAuthorLastName").val();
        if (AuthorId == -1 && (AuthorLastName.replace(/ +/g, ' ').trim().length < 2 || AuthorLastName.replace(/ +/g, ' ').trim().length > 30)) {
            ModelValid = false;
            ModelErrors.push("Author last name must be longer than 1 characters and less than 30");
        }

        //Language
        let LanguageId = parseInt($("#formLanguageId").val());
        if (isNaN(LanguageId)) {
            ModelValid = false;
            ModelErrors.push("LanguageId isn`t valid");
        }

        let LanguageName = $("#formLanguageName").val();
        if (LanguageId == -1 && (LanguageName.replace(/ +/g, ' ').trim().length < 2 || LanguageName.replace(/ +/g, ' ').trim().length > 20)) {
            ModelValid = false;
            ModelErrors.push("Language name must be longer than 1 characters and less than 20");
        }

        //Category
        let CategoryId = parseInt($("#formCategoryId").val());
        if (isNaN(CategoryId)) {
            ModelValid = false;
            ModelErrors.push("CategoryId isn`t valid");
        }

        let CategoryName = $("#formCategoryName").val();
        if (CategoryId == -1 && (CategoryName.replace(/ +/g, ' ').trim().length < 2 || CategoryName.replace(/ +/g, ' ').trim().length > 30)) {
            ModelValid = false;
            ModelErrors.push("Category name must be longer than 1 characters and less than 30");
        }

        //Publisher
        let PublisherId = parseInt($("#formPublisherId").val());
        if (isNaN(PublisherId)) {
            ModelValid = false;
            ModelErrors.push("PublisherId isn`t valid");
        }

        let PublisherName = $("#formPublisherName").val();
        if (PublisherId == -1 && (PublisherName.replace(/ +/g, ' ').trim().length < 2 || PublisherName.replace(/ +/g, ' ').trim().length > 30)) {
            ModelValid = false;
            ModelErrors.push("Publisher name must be longer than 1 characters and less than 30");
        }


        ///////////////////////////////////////////
        //  End validation
        //////////////////////////////////////////

        // Show all errors
        if (!ModelValid) {
            for (let error of ModelErrors) {
                $('<lable>', { class: "text-danger", text: error }).append($('<br/>')).appendTo("#fromErrors");
            }
            return;
        }

        // Add book to arrays

        let category = null;
        if (CategoryId == -1) {
            let ids = [0];
            for (let key of Categories.keys()) {
                ids.push(parseInt(key));
            }
            category = new Category(Math.max.apply(null,ids) + 1, CategoryName);
            Categories.set(category.Id, category);
        } else {
            category = Categories.get(CategoryId);
        }

        let language = null;
        if (LanguageId == -1) {
            let ids = [0];
            for (let key of Languages.keys()) {
                ids.push(parseInt(key));
            }
            language = new Language(Math.max.apply(null, ids) + 1, LanguageName);
            Languages.set(language.Id, language);
        } else {
            language = Languages.get(LanguageId);
        }

        let publisher = null;
        if (PublisherId == -1) {
            let ids = [0];
            for (let key of Publishers.keys()) {
                ids.push(parseInt(key));
            }
            publisher = new Publisher(Math.max.apply(null, ids) + 1, PublisherName);
            Publishers.set(publisher.Id, publisher);
        } else {
            publisher = Publishers.get(PublisherId);
        }

        let author = null;
        if (AuthorId == -1) {
            let ids = [0];
            for (let key of Authors.keys()) {
                ids.push(parseInt(key));
            }
            author = new Author(Math.max.apply(null, ids) + 1, AuthorFirstName, AuthorLastName);
            Authors.set(author.Id, author);
        } else {
            author = Authors.get(AuthorId);
        }

        let ids = [0];
        for (let key of Books) {
            ids.push(parseInt(key.Id));
        }
        let book = new Book(Math.max.apply(null, ids) + 1, Title, Year, CountOfPages, author.Id, author,
            publisher.Id, publisher, language.Id, language, category.Id, category);
        Books.push(book);
        FillFormSelect();
        dataWasChanged = true;
        let bookItem = makeBookItem(book);
        let allBooks = $("[data-book_item_id]");
        if (allBooks.length > 0) {
            for (let el of allBooks) {
                bookItem.insertBefore(el);
                break;
            }
            return;
        }
        $("#AllBooks").append(bookItem);
    });

    // Update data from the server

    let timerId = setTimeout(function tick() {
        syncWithServer();
        timerId = setTimeout(tick, UpdateTime);
    }, UpdateTime);

    function syncWithServer() {

        if (!dataWasChanged) {
            return;
        }

        let books = JSON.stringify(Books);

        let authors = [];
        for (let el of Authors.values()) {
            authors.push(el);
        }
        authors = JSON.stringify(authors);

        let languages = [];
        for (let el of Languages.values()) {
            languages.push(el);
        }
        languages = JSON.stringify(languages);

        let categories = [];
        for (let el of Categories.values()) {
            categories.push(el);
        }
        categories = JSON.stringify(categories);

        let publishers = [];
        for (let el of Publishers.values()) {
            publishers.push(el);
        }
        publishers = JSON.stringify(publishers);

        let result = [books, authors, languages, categories, publishers];

        $.ajax({
            url: "/Home/PostAllItems",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(result),
            success: function () { dataWasChanged = false; },
            error: function () { dataWasChanged = true; }
        });
       
        
        
    }


    //Save Changes
    $("#saveChanges").on("click", function (event) {
        event.preventDefault();
        if (dataWasChanged) {
            syncWithServer();
        }
    });

    // Close page
    set_onbeforeunload = function () {
        return true;
    };

    $(document).ready(function () {
        
        window.onbeforeunload = set_onbeforeunload;
        
    });

});