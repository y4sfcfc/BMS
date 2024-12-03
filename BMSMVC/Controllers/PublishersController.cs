using BMS.DAL.Dtos;
using BMS.DAL.Models;
using BMS.DAL.Services.Implementations;
using BMS.DAL.Services.Interfaces;
using BMS.WEBUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace BMS.WEBUI.Controllers
{
	public class PublishersController : Controller
	{
		private readonly IPublisherService _publisherService;
		public PublishersController(IPublisherService publisherService)
		{
			_publisherService = publisherService;
		}
		public async Task<IActionResult> Index()
		{
			var publishers = await _publisherService.GetAll("Books");
			if (publishers.Any())
			{
				ViewBag.HasData = true;
			}
			else
			{
				ViewBag.HasData = false;
				ViewData["Message"] = "No publishers here";

			}
			return View(publishers.ToList());
		}
		public async Task<IActionResult> Create()
		{
			return View();

		}
		[HttpPost]
		public async Task<IActionResult> Create(PublisherDto dto)
		{
			await _publisherService.Add(dto);
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Edit(int id)
		{
			var publisher = await _publisherService.Get(id);
			PublisherDto publisherDto = new PublisherDto();
			return View(publisherDto);

		}
		[HttpPost]
		public async Task<IActionResult> Edit(PublisherDto dto, int id)
		{
			await _publisherService.Update(dto,id);
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Delete(int id)
		{
			var publisher = await _publisherService.Get(id,"Books");
			PublisherViewModel publisherVM = new PublisherViewModel
			{
				Id = publisher.Id,
				Name = publisher.Name,
				Books = publisher.Books.ToList()

			};
			return View(publisherVM);

		}
		[HttpPost]
		public IActionResult DeletePost(int id)
		{
			_publisherService.Delete(id);
			return RedirectToAction("Index");
		}
	}
}
