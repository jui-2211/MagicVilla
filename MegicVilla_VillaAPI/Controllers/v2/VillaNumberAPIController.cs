using AutoMapper;
using MegicVilla_VillaAPI.Data;
using MegicVilla_VillaAPI.Models;
using MegicVilla_VillaAPI.Models.Dto;
using MegicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MegicVilla_VillaAPI.Controllers.v2
{
	[Route("api/V{version:apiVersion}/VillaNumberAPI")]
	[ApiController]
	[ApiVersion("2.0")]
	public class VillaNumberAPIController : ControllerBase
	{
		protected APIResponse _response;

		private readonly ILogger<VillaNumberAPIController> _logger;

		private readonly IVillaNumberRepository _dbVillaNumber;

		private readonly IVillaRepository _dbVilla;

		private readonly IMapper _mapper;
		public VillaNumberAPIController(ILogger<VillaNumberAPIController> logger, IVillaNumberRepository dbVillaNumber, IVillaRepository dbVilla, IMapper mapper)
		{
			_logger = logger;
			_mapper = mapper;
			_dbVillaNumber = dbVillaNumber;
			_dbVilla = dbVilla;
			_response = new();
		}


		//[MapToApiVersion("2.0")]
		[HttpGet("GetString")]
		public IEnumerable<string> Get()
		{
			return new string[] { "Raj", "Shailraj" };
		}

		[HttpGet]
		public IEnumerable<string> GetName()
		{
			return new string[] { "Soham", "Bhalodi" };
		}

	}
}
