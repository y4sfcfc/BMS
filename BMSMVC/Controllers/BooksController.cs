using AutoMapper;
using BMS.DAL.Data;
using BMS.DAL.Dtos;
using BMS.DAL.Models;
using BMS.DAL.Repository;
using BMS.DAL.Services.Interfaces;
using BMS.WEBUI.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BMS.WEBUI.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IGenericService<Category> _categoryService;
        private readonly IGenericService<Author> _authorsService;
        private readonly IGenericService<Publisher> _publisherService;
        private readonly IGenericService<BookCategory> _bookCategoryService;
        private readonly IMapper _mapper;
        public BooksController(IBookService bookService,
            IGenericService<Category> categoryService,
            IGenericService<Author> authorsService,
            IGenericService<Publisher> publisherService,
            IGenericService<BookCategory> bookCategoryService,
            IMapper mapper
            )
        {
            _bookService = bookService;
            _categoryService = categoryService;
            _authorsService = authorsService;
            _publisherService = publisherService;
            _bookCategoryService = bookCategoryService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            List<BookViewModel> list = new List<BookViewModel>();

            var books = await _bookService.GetAll("Publisher", "Author", "BookCategories.Category");
            var model = books.Select(x => new BookViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Categories = x.BookCategories.Select(y => new Category { Name = y.Category.Name }).ToList(),
                PublisherName = x.Publisher.Name,
                AuthorFullName = x.Author.Name + " " + x.Author.Surname
            });
            if (books.Any())
            {
                ViewBag.HasData = true;

                /*    foreach (var item in books)
                    {
                        *//* var publisherName = _publisherService.Get(item.PublisherId).Result.Name;*/
                /*  List<string> categoryList = new List<string>();*//*


                var author = _authorsService.Get(item.AuthorId).Result;

            }*/

            }
            else
            {
                ViewBag.HasData = false;
                ViewData["Message"] = "No books here";

            }
            return View(model.ToList());
        }
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAll();
            var authors = await _authorsService.GetAll();
            var publishers = await _publisherService.GetAll();
            ViewBag.Categories = categories;
            ViewBag.Authors = authors;
            ViewBag.Publishers = publishers;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BookDto dto)
        {
            await _bookService.Add(dto);
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _bookService.Get(id);
            BookDto dto = _mapper.Map<BookDto>(book);
            var categories = await _categoryService.GetAll();
            var authors = await _authorsService.GetAll();
            var publishers = await _publisherService.GetAll();
            ViewBag.Categories = categories;
            ViewBag.Authors = authors;
            ViewBag.Publishers = publishers;

            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(BookDto dto, int id)
        {

            await _bookService.Update(dto, id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookService.Get(id, "Publisher", "Author", "BookCategories.Category");
            BookViewModel bookVM = new BookViewModel
            {
                Id = book.Id,
                Name = book.Name,
                Categories = book.BookCategories.Select(y => new Category { Name = y.Category.Name }).ToList(),
                AuthorFullName = book.Author.Name + " " + book.Author.Surname,
                PublisherName = book.Publisher.Name,
            };
            return View(bookVM);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(BookViewModel viewModel)
        {
            _bookService.Delete(viewModel.Id);
           return RedirectToAction("Index");
        }
    }
}
