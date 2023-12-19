using MegicVilla_VillaAPI.Models;
using MegicVilla_VillaAPI.Models.Dto;

namespace MegicVilla_VillaAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);

        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);

        Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO);
    }
}
