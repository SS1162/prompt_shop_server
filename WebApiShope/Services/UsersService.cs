using Entities;
using Repositories;
using DTO;
using AutoMapper;
namespace Services
{
    public class UsersService : IUsersService
    {//get by id
        private readonly IUsersReposetory _usersReposetory;
        private readonly IMapper _mapper;
        private readonly IPasswordsService _passwordsService;
        private readonly IPasswordHashingService _passwordHashingService;
        public UsersService(IUsersReposetory repositoriesUsers, IMapper mapper, IPasswordsService passwordsService,
            IPasswordHashingService passwordHashingService)
        {
            this._usersReposetory = repositoriesUsers;
            this._mapper = mapper;
            this._passwordsService = passwordsService;
            this._passwordHashingService = passwordHashingService;
        }
        public async Task<UserDTO> GetByIDUsersService(long id)
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
            userToReposetory.Password = _passwordHashingService.HashPassword(registerUser.UserPassword);
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
            if (userFromRposetory == null)
            {
                return null;
            }

            bool isPasswordValid = _passwordHashingService.VerifyPassword(logInUser.UserPassward, userFromRposetory.Password)
                || userFromRposetory.Password == logInUser.UserPassward;

            if (!isPasswordValid)
            {
                return null;
            }

            UserDTO userToConroller = _mapper.Map<UserDTO>(userFromRposetory);

            return userToConroller;
        }

        async public Task<Resulte<UserDTO>> UpdateUsersService(long id, UpdateUserDTO userToUpdate)
        {
          


            if (id != userToUpdate.UserId)
                return Resulte<UserDTO>.Failure("The id'es are diffrent");
            User? checkIfUserExist = await _usersReposetory.GetByIDUsersRepositories(id);
            if(checkIfUserExist==null)
            {
                return Resulte<UserDTO>.Failure("The user ide's is incorect");
            }
            if (userToUpdate.Password != null)
            {
                PasswordDTO passwordForCheckStrength = new PasswordDTO();
                passwordForCheckStrength.UserPassward = userToUpdate.Password;
                if (passwordForCheckStrength.UserPassward != null && _passwordsService.CheckPasswordStrength(passwordForCheckStrength).Data < 2)
                {
                    return Resulte<UserDTO>.Failure("The password is not strong enough");
                }
            }
            else
            {
            userToUpdate = new UpdateUserDTO(userToUpdate.UserId, checkIfUserExist.Password, userToUpdate.UserName,
            userToUpdate.FirstName,userToUpdate.LastName,userToUpdate.Phone,userToUpdate.BasicID);     
            }
            User userToRposetory = _mapper.Map<User>(userToUpdate);
            if(userToUpdate.Password==null)
            {
                userToRposetory.Password = checkIfUserExist.Password;
            }
            else
            {
                userToRposetory.Password = _passwordHashingService.HashPassword(userToUpdate.Password);
            }
            User? checkUserValidtion = await _usersReposetory.GetByIDUsersRepositories(id);
            if (checkUserValidtion == null)
                return Resulte<UserDTO>.Failure("The user id dont exist");

            if (checkUserValidtion.UserName != userToRposetory.UserName)
                return Resulte<UserDTO>.Failure("The user name make diifrent");

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
