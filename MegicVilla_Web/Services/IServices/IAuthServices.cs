using MegicVilla_Web.Models.Dto;

namespace MegicVilla_Web.Services.IServices
{
    public interface IAuthServices
    {
        Task<T> LoginAsync<T>(LoginRequestDTO objToCreate);
        Task<T> RegisterAsync<T>(RegisterationRequestDTO objToCreate);
    }
}
