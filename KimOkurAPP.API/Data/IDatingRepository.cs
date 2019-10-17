using System.Collections.Generic;
using System.Threading.Tasks;
using KimOkur.API.Models;
using KimOkurAPP.API.Helpers;

namespace KimOkur.API.Data
{
    public interface IDatingRepository
    {
          void Add<T>(T entity) where T:class;
          void Delete<T>(T entity) where T:class;

          Task<bool> SaveAll();
          Task<PagedList<User>> GetUsers(UserParams userParams);

          Task<User> GetUser(int id);
          Task<Photo> GetUserPhoto(int id);
          Task<Photo> GetMainPhotoForUser(int userId);

}
    
}