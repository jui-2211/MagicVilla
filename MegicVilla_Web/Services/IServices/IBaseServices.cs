using MegicVilla_Web.Models;

namespace MegicVilla_Web.Services.IServices
{
	public interface IBaseServices
	{
		APIResponse responseModel { get; set; }

		Task<T> SendAsync<T>(APIRequest apiRequest);
	}
}
