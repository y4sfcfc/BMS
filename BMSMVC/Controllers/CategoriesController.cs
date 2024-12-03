using AutoMapper;
using BMS.DAL.Dtos;
using BMS.DAL.Models;
using BMS.DAL.Services.Interfaces;
using BMS.WEBUI.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace BMS.WEBUI.Controllers
{
	public class CategoriesController : Controller
	{
		private readonly ICategoryService _categoryService;
		private readonly IMapper _mapper;

		public CategoriesController(ICategoryService categoryService,
			IMapper mapper)
		{
			_categoryService = categoryService;
			_mapper = mapper;
		}
		public async Task<IActionResult> Index()
		{
			var categories = _categoryService.GetAll().Result.ToList();
			var model = categories.Select(x => new CategoryViewModel
			{
				Id = x.Id,
				Name = x.Name,
			});
			if (categories.Any())
			{
				ViewBag.HasData = true;
			}
			else
			{
				ViewBag.HasData = false;
				ViewData["Message"] = "No books here";
			}
			return View(model.ToList());
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(CategoryDto dto)
		{
			await _categoryService.Add(dto);
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Edit(int id)
		{
			var category = await _categoryService.Get(id);
			CategoryDto dto = _mapper.Map<CategoryDto>(category);
			return View(dto);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(CategoryDto dto, int id)
		{
			await _categoryService.Update(dto, id);
			return RedirectToAction("Index");
		}	
		public async Task<IActionResult> Delete(int id)
		{
			var category = await _categoryService.Get(id);
			CategoryViewModel categoryVM=new CategoryViewModel
			{
				Id=category.Id,
				Name=category.Name,
			};
			return View(categoryVM);
		}
		[HttpPost]
		public  IActionResult DeletePost(int id)
		{
			 _categoryService.Delete(id);
			return RedirectToAction("Index");
		}
	}
}
