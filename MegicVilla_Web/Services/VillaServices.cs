using MegicVilla_Utility;
using MegicVilla_Web.Models;
using MegicVilla_Web.Models.Dto;
using MegicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
	public class VillaService : BaseService, IVillaServices
	{
		private readonly IHttpClientFactory _clientFactory;
		private string villaUrl;

		public VillaService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
		{
			_clientFactory = clientFactory;
			villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");

		}

		public Task<T> CreateAsync<T>(VillaCreateDTO dto, string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.POST,
				Data = dto,
				Url = villaUrl + "/api/v1/villaAPI",
				Token = token
			});
		}

		public Task<T> DeleteAsync<T>(int id, string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.DELETE,
				Url = villaUrl + "/api/v1/villaAPI/" + id,
				Token = token
			});
		}

		public Task<T> GetAllAsync<T>(string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = villaUrl + "/api/v1/villaAPI",
				Token = token
			});
		}

		public Task<T> GetAsync<T>(int id, string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = villaUrl + "/api/v1/villaAPI/" + id,
				Token = token
			});
		}

		public Task<T> UpdateAsync<T>(VillaUpdateDTO dto, string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.PUT,
				Data = dto,
				Url = villaUrl + "/api/v1/villaAPI/" + dto.Id,
				Token = token
			});
		}
	}
}