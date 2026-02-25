using Entities;

namespace Repositories
{
    public interface IUsersReposetory
    {
        Task<User> AddNewUsersRepositories(User user);
        Task<User?> GetByIDUsersRepositories(long id);
        Task<User?> LoginUsersRepositories(User LogInUser);
        Task UpdateUsersRepositories(long id, User user);
         Task<bool> CheckIfUsersInsistalrady(string user);

         Task<User?> SignInWithGoogleRepositories(User user);
    }
}