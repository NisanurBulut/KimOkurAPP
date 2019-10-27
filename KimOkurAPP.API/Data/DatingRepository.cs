using System;
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
  public async Task<Like> GetLike(int userId, int recipientId)
        {
            return await _dc.Likes.FirstOrDefaultAsync(u =>
                u.LikerId == userId && u.LikeeId == recipientId);
        }
        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _dc.Photos.Where(u => u.UserId == userId)
                          .FirstOrDefaultAsync(p => p.IsMain);
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
            var users = _dc.Users.Include(p => p.Photos)
                .OrderByDescending(u => u.LastActive)
                .AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);

            users = users.Where(u => u.Gender == userParams.Gender);

            if(userParams.Likees)
            {
                var userLikers=await GetUserLikes(userParams.UserId,userParams.Likers);
                users=users.Where(u=>userLikers.Contains(u.Id));
            }
            if(userParams.Likers)
            {
                var userLikees=await GetUserLikes(userParams.UserId,userParams.Likees);
                 users= users.Where(u=>userLikees.Contains(u.Id));
            }

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

                users = users.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }
            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }
        private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers)
        {
            var user = await _dc.Users
            .Include(a=>a.Likees)
            .Include(a=>a.Likers).FirstOrDefaultAsync(u => u.Id == id);

            if (likers)
            {
                return user.Likers.Where(u => u.LikerId == id).Select(i => i.LikerId);
            }
            else
            {
                return user.Likees.Where(u => u.LikeeId == id).Select(i => i.LikeeId);
            }
        }
        public async Task<bool> SaveAll()
        {
            return await _dc.SaveChangesAsync() > 0;
        }
    }
}