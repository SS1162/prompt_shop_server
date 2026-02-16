using Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace Repositories
{

    public class UsersReposetory : IUsersReposetory
    {
        private readonly MyShop330683525Context _MyShop330683525Context;

        public UsersReposetory(MyShop330683525Context _MyShop330683525Context)
        {
            this._MyShop330683525Context = _MyShop330683525Context;
        }

        //M:\web api\project_from_git\git-dot.net-project\WebApiShope\Repositories\users.txt
        //Post new user 
        async public Task<User> AddNewUsersRepositories(User user)
        {
                await _MyShop330683525Context.Users.AddAsync(user);
                await _MyShop330683525Context.SaveChangesAsync();
                return user;
          
        }
        //Get by ID  new user 
        public async Task<User?> GetByIDUsersRepositories(long id)
        {
          
            return  await _MyShop330683525Context.Users.FirstOrDefaultAsync(x=>x.UserId==id);

        }


        public async Task<User?> SignInWithGoogleRepositories(User user)
        {

         User? checkIfUserExist=  await _MyShop330683525Context.Users.FirstOrDefaultAsync(x=>x.UserName==user.UserName);
            if (checkIfUserExist != null)
                return checkIfUserExist;
            return await AddNewUsersRepositories(user);


        }
        public async Task<bool> CheckIfUsersInsistalrady(string user)
        {
           var resulte =await _MyShop330683525Context.Users.FirstOrDefaultAsync(x => x.UserName == user.ToLower());
            if(resulte == null)
            {
                return true;
            }
            return false;
        }

        ////Post login user

        public async Task<User?> LoginUsersRepositories(User LogInUser)
        {
            var user = await _MyShop330683525Context.Users.FirstOrDefaultAsync(x => LogInUser.UserName == x.UserName &&
            LogInUser.Password == x.Password);
            return user;
        }

        //Put 
        public async Task UpdateUsersRepositories(long id, User user)
        { 
            _MyShop330683525Context.Users.Update(user);
           await _MyShop330683525Context.SaveChangesAsync();
        }

    }
}
