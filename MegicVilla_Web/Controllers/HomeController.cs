using AutoMapper;
using MegicVilla_Utility;
using MegicVilla_Web.Models;
using MegicVilla_Web.Models.Dto;
using MegicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MegicVilla_Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly IVillaServices _villaService;
		private readonly IMapper _mapper;
		public HomeController(IVillaServices villaService, IMapper mapper)
		{
			_villaService = villaService;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
		{
			List<VillaDTO> list = new();

			var response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
			}
			return View(list);
		}

	}
}