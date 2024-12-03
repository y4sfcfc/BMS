using AutoMapper;
using BMS.DAL.Dtos;
using BMS.DAL.Models;
using BMS.DAL.Services.Interfaces;
using BMS.WEBUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BMS.WEBUI.Controllers
{
	public class AuthorContactController : Controller
	{
		private readonly IAuthorService _authorService;
		private readonly IAuthorContactService _authorContactService;
        private readonly IMapper _mapper;
        public AuthorContactController(IAuthorService authorService,
			IAuthorContactService authorContactService,
            IMapper mapper)
		{
			_authorService = authorService;
			_authorContactService = authorContactService;
			_mapper = mapper;
		}
		public async Task<IActionResult> Index()
		{
			var authorContacts = await _authorContactService.GetAll();
			List<AuthorContactViewModel> list = new List<AuthorContactViewModel>();
			if (authorContacts.Any())
			{
				ViewBag.HasData = true;
				foreach (var item in authorContacts)
				{
					var author = await _authorService.Get(item.AuthorId);
					list.Add(new AuthorContactViewModel
                    {
						Id = item.Id,
						FullName = author.Name + " " + author.Surname,
						Address = item.Address,
						ContactNumber = item.ContactNumber,
					});
				}
			}
			else
			{
				ViewBag.HasData = false;
				ViewData["Message"] = "No author contacts here";

			}
			return View(list);
		}
		public async Task<IActionResult> Create()
		{
			ViewBag.Authors = await _authorService.GetAll();
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(AuthorContactDto dto)
		{
			ViewBag.Authors = await _authorService.GetAll();
			await _authorContactService.Add(dto);
			return RedirectToAction("Index");
		}
	
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Authors = await _authorService.GetAll();
            var authorContact = await _authorContactService.Get(id);
			AuthorContactDto dto = _mapper.Map<AuthorContactDto>(authorContact);
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AuthorContactDto dto,int id)
        {
           await _authorContactService.Update(dto,id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var authorContact = await _authorContactService.Get(id,"Author");
            AuthorContactViewModel authorVM = new AuthorContactViewModel
            {
                Id = authorContact.Id,
                FullName = authorContact.Author.Name + " " + authorContact.Author.Surname,
                ContactNumber = authorContact.ContactNumber,
				Address = authorContact.Address,
            };
            return View(authorVM);
        }
        [HttpPost]
        public IActionResult DeletePost(int id)
        {

            var authorContact = _authorContactService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
