using System.Threading.Tasks;
using DatingAPP.API.Models;

namespace DatingAPP.API.Data
{
    public interface IAuthRepository
    {
         public Task<User> Register(User user,string password);
         public Task<User> Login(User user,string password);

         public Task<bool> UserExists(string username);
    }
}