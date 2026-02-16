using DTO;
using Entities;

namespace Services
{
    public interface IUsersService
    {
        Task<Resulte<UserDTO>> AddNewUsersService(RegisterUserDTO registerUser);
        Task<bool> CheckIfUsersInsistalradyServise(string user);
        Task<UserDTO> GetByIDUsersService(long id);
        Task<UserDTO> LoginUsersService(LoginUserDTO logInUser);
        Task<Resulte<UserDTO>> UpdateUsersService(long id, UpdateUserDTO userToUpdate);
        Task<UserDTO> SignInWithGoogleServise(RegisterUserDTO registerUser);
    }
}