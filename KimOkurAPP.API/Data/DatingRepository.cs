using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KimOkur.API.Data;
using KimOkur.API.Models;
using KimOkurAPP.API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace KimOkur.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _dc;

        public DatingRepository(DataContext dc)
        {
            _dc = dc;
        }


        public void Add<T>(T entity) where T : class
        {
            _dc.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _dc.Remove(entity);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
          return await  _dc.Photos.Where(u=>u.UserId==userId)
                        .FirstOrDefaultAsync(p=>p.IsMain);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _dc.Users.Include(p => p.Photos)
            .FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
         public async Task<Photo> GetUserPhoto(int id)
        {
            var photo = await _dc.Photos.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users =  _dc.Users.Include(p => p.Photos);

            return await PagedList<User>.CreateAsync(users,userParams.PageNumber,userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await _dc.SaveChangesAsync() > 0;
        }
    }
}