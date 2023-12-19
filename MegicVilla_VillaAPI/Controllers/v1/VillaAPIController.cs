using AutoMapper;
using MegicVilla_VillaAPI.Data;
using MegicVilla_VillaAPI.Models;
using MegicVilla_VillaAPI.Models.Dto;
using MegicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace MegicVilla_VillaAPI.Controllers.v1
{
	[Route("api/V{version:apiVersion}/VillaAPI")]
	[ApiController]
	public class VillaAPIController : ControllerBase
	{
		protected APIResponse _response;

		private readonly ILogger<VillaAPIController> _logger;

		private readonly IVillaRepository _dbVilla;

		private readonly IMapper _mapper;
		public VillaAPIController(ILogger<VillaAPIController> logger, IVillaRepository dbVilla, IMapper mapper)
		{
			_logger = logger;
			_mapper = mapper;
			_dbVilla = dbVilla;
			_response = new();
		}

		[HttpGet]
		//[ResponseCache(Duration = 30)]
		[ResponseCache(CacheProfileName = "Default30")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<ActionResult<APIResponse>> GetVillas([FromQuery(Name = "filterOccupancy")] int? occupancy, [FromQuery(Name = "searchName")] string? search, int pageSize = 0, int pageNumber = 1)
		{
			try
			{
				_logger.LogInformation("Get all villlasf");
				IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
				if (occupancy > 0)
				{
					villaList = await _dbVilla.GetAllAsync(u => u.Occupancy == occupancy, pageSize: pageSize, pageNumber: pageNumber);
				}
				else
				{
					villaList = await _dbVilla.GetAllAsync(pageSize: pageSize, pageNumber: pageNumber);
				}

				if (!string.IsNullOrEmpty(search))
				{
					villaList = villaList.Where(u => u.Name.ToLower().Contains(search.ToLower()));
				}

				Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

				Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
				_response.Result = _mapper.Map<List<VillaDTO>>(villaList);
				_response.StatusCode = HttpStatusCode.OK;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages
					 = new List<string>() { ex.ToString() };
			}
			return _response;
		}

		[HttpGet("{id}", Name = "GetVilla")]
		//[HttpGet("{id:int}")]
		[ResponseCache(CacheProfileName = "Default30")]
		[Authorize(Roles = "admin")]

		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<APIResponse>> GetVilla(int id)
		{
			try
			{
				if (id == 0)
				{
					_logger.LogError("Get Villa Error With Id " + id);
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				var villa = await _dbVilla.GetAsync(u => u.Id == id);

				if (villa == null)
				{
					_logger.LogError("Get Villa Error With Id " + id);
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}

				_logger.LogInformation("Get Villa Detail With Id " + id);

				_response.Result = _mapper.Map<VillaDTO>(villa);
				_response.StatusCode = HttpStatusCode.OK;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages
					 = new List<string>() { ex.ToString() };
			}
			return _response;
		}

		[HttpPost]
		[ResponseCache(CacheProfileName = "Default30")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]

		public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO createDTO)
		{
			try
			{

				if (await _dbVilla.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
				{
					ModelState.AddModelError("ErrorMessages", "Villa Alredy Exists !");
					return BadRequest(ModelState);
				}

				if (createDTO == null)
				{
					return BadRequest(createDTO);
				}

				Villa villa = _mapper.Map<Villa>(createDTO);

				await _dbVilla.CreateAsync(villa);

				_response.Result = _mapper.Map<VillaDTO>(villa);
				_response.StatusCode = HttpStatusCode.Created;
				return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages
					 = new List<string>() { ex.ToString() };
			}
			return _response;
		}

		[HttpDelete("{id}", Name = "DeleteVilla")]
		[Authorize(Roles = "admin")]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
		{
			try
			{
				if (id == 0)
				{
					return BadRequest();
				}

				var villa = await _dbVilla.GetAsync(u => u.Id == id);

				if (villa == null)
				{
					return NotFound();
				}

				await _dbVilla.RemoveAsync(villa);

				_response.StatusCode = HttpStatusCode.NoContent;
				_response.IsSuccess = true;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages
					 = new List<string>() { ex.ToString() };
			}
			return _response;
		}

		[HttpPut("{id}", Name = "UpdateVilla")]
		[ResponseCache(CacheProfileName = "Default30")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]

		public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
		{
			try
			{
				if (updateDTO == null || id != updateDTO.Id)
				{
					return BadRequest();
				}

				Villa model = _mapper.Map<Villa>(updateDTO);

				await _dbVilla.UpdateAsync(model);

				_response.StatusCode = HttpStatusCode.NoContent;
				_response.IsSuccess = true;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.ErrorMessages
					 = new List<string>() { ex.ToString() };
			}
			return _response;
		}

		[HttpPatch("id", Name = "UpdatePartialVilla")]
		[ResponseCache(CacheProfileName = "Default30")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]

		public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
		{
			if (patchDTO == null || id == 0)
			{
				return BadRequest();
			}

			var villa = await _dbVilla.GetAsync(u => u.Id == id, tracked: false);

			if (villa == null)
			{
				return BadRequest();
			}

			VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

			patchDTO.ApplyTo(villaDTO, ModelState);

			Villa model = _mapper.Map<Villa>(villaDTO);

			await _dbVilla.UpdateAsync(model);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			return NoContent();
		}
	}
}
