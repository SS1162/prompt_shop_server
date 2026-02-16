using Entities;
using Repositories;
using DTO;
using AutoMapper;
namespace Services
{
    public class UsersService : IUsersService
    {//get by id
        private IUsersReposetory _usersReposetory;
        private IMapper _mapper;
        private IPasswordsService _passwordsService;
        public UsersService(IUsersReposetory repositoriesUsers, IMapper mapper, IPasswordsService passwordsService)
        {
            this._usersReposetory = repositoriesUsers;
            this._mapper = mapper;
            this._passwordsService = passwordsService;
        }
        public async Task<UserDTO> GetByIDUsersService(int id)
        {

            User? user = await _usersReposetory.GetByIDUsersRepositories(id);
            UserDTO userToController = _mapper.Map<UserDTO>(user);
            return userToController;
        }
        //post new user
        public async Task<Resulte<UserDTO>> AddNewUsersService(RegisterUserDTO registerUser)
        {
            PasswordDTO passwordForCheckStrength = new PasswordDTO();
            passwordForCheckStrength.UserPassward = registerUser.UserPassword;
            if (_passwordsService.CheckPasswordStrength(passwordForCheckStrength).Data < 2)
            {
                return Resulte<UserDTO>.Failure("The paasword is not strong enghth");
            }

            User userToReposetory = _mapper.Map<User>(registerUser);
            userToReposetory.UserName = userToReposetory.UserName.ToLower();
            bool flag = await _usersReposetory.CheckIfUsersInsistalrady(userToReposetory.UserName);
            if (!flag)
                return Resulte<UserDTO>.Failure("The user insist alrady");

            User userFromReposetory = await _usersReposetory.AddNewUsersRepositories(userToReposetory);

            UserDTO userToController = _mapper.Map<UserDTO>(userFromReposetory);
            return Resulte<UserDTO>.Success(userToController);
        }

        public async Task<bool> CheckIfUsersInsistalradyServise(string user)
        {

            user = user.ToLower();
            return await _usersReposetory.CheckIfUsersInsistalrady(user);

        }




        //post login user
        public async Task<UserDTO> LoginUsersService(LoginUserDTO logInUser)
        {
            User userToRposetory = _mapper.Map<User>(logInUser);
            userToRposetory.UserName = userToRposetory.UserName.ToLower();

            User? userFromRposetory = await _usersReposetory.LoginUsersRepositories(userToRposetory);

            UserDTO userToConroller = _mapper.Map<UserDTO>(userFromRposetory);

            return userToConroller;
        }

        async public Task<Resulte<UserDTO>> UpdateUsersService(int id, UpdateUserDTO userToUpdate)
        {


            PasswordDTO passwordForCheckStrength = new PasswordDTO();
            passwordForCheckStrength.UserPassward = userToUpdate.Password;
            if (passwordForCheckStrength.UserPassward!=null||_passwordsService.CheckPasswordStrength(passwordForCheckStrength).Data < 2)
            {
                Resulte<UserDTO>.Failure("The password is not strong enough");
            }
            if (id != userToUpdate.UserId)
                Resulte<UserDTO>.Failure("The id'es are diffrent");
            User? checkIfUserExist = await _usersReposetory.GetByIDUsersRepositories(id);
            if(checkIfUserExist==null)
            {
                return Resulte<UserDTO>.Failure("The user ide's is incorect");
            }
            User userToRposetory = _mapper.Map<User>(userToUpdate);
            if(userToUpdate.Password==null)
            {
                userToRposetory.Password = checkIfUserExist.Password;
            }
            User? checkUserValidtion = await _usersReposetory.GetByIDUsersRepositories(id);
            if (checkUserValidtion == null)
                Resulte<UserDTO>.Failure("The user id dont exist");

            if (checkUserValidtion.UserName != userToRposetory.UserName)
                Resulte<UserDTO>.Failure("The user name make diifrent");

            await _usersReposetory.UpdateUsersRepositories(id, userToRposetory);
            return Resulte<UserDTO>.Success(null);

        }


        public async Task<UserDTO> SignInWithGoogleServise(RegisterUserDTO registerUser)
        {
            User userToReposetory = _mapper.Map<User>(registerUser);
            userToReposetory.UserName = userToReposetory.UserName.ToLower();
            User userForReturn = await _usersReposetory.SignInWithGoogleRepositories(userToReposetory);
            return _mapper.Map<UserDTO>(userForReturn);
        }

    }
}
