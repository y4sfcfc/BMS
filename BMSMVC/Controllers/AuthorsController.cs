using AutoMapper;
using BMS.DAL.Dtos;
using BMS.DAL.Models;
using BMS.DAL.Repository;
using BMS.DAL.Services.Interfaces;
using BMS.WEBUI.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace BMS.WEBUI.Controllers
{
	public class AuthorsController : Controller
	{

		private readonly IAuthorService _authorService;
		private readonly IAuthorContactService _authorContactService;
		private IMapper _mapper;
		public AuthorsController(IAuthorService authorService,
			IAuthorContactService authorContactService,
			IMapper mapper)
		{
			_authorService = authorService;
			_authorContactService = authorContactService;
			_mapper = mapper;
		}
		public async Task<IActionResult> Index()
		{
			var authors = await _authorService.GetAll("Books");
			List<AuthorViewModel> list = new List<AuthorViewModel>();
			var model = authors.Select(x => new AuthorViewModel
			{
				Id = x.Id,
				Name = x.Name,
				Surname = x.Surname,
				Books = x.Books.Select(y => new Book { Name = y.Name }).ToList(),

			});
			/*	foreach (var item in authors)
				{
					list.Add(new AuthorViewModel
					{
						Id = item.Id,
						Name = item.Name ,
						Surname=  item.Surname,
						Books = item.Books.ToList()
					});

				}*/
			;
			if (authors.Any())
			{
				ViewBag.HasData = true;

			}
			else
			{
				ViewBag.HasData = false;
				ViewData["Message"] = "No authors here";

			}
			return View(model.ToList());
		}
		public IActionResult Create()
		{

			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(AuthorDto dto)
		{
			await _authorService.Add(dto);
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Edit(int id)
		{
			var author = await _authorService.Get(id);
			AuthorDto authorDto = _mapper.Map<AuthorDto>(author);
			return View(authorDto);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(AuthorDto dto, int id)
		{
			await _authorService.Update(dto, id);
			return RedirectToAction("Index");
		}
			public async Task<IActionResult> Delete(int id)
		{
			var author = await _authorService.Get(id);
			AuthorViewModel authorViewModel= new AuthorViewModel
			{
				Id = id,
				Name=author.Name,
				Surname=author.Surname,
				
			};
			
			return View(authorViewModel);
		}
		[HttpPost]
		public async Task<IActionResult> DeletePost(int id)
		{
			 _authorService.Delete(id);
			return RedirectToAction("Index");
		}

	}
}
