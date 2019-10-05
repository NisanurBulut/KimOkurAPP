using System;
using System.Threading.Tasks;
using KimOkur.API.Data;
using KimOkur.API.Models;
using Microsoft.EntityFrameworkCore;

namespace KimOkur.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dc;
        public AuthRepository(DataContext dc)
        {
            _dc = dc;
        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _dc.Users.FirstOrDefaultAsync(a => a.Username == username);
            if (user == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _dc.Users.AddAsync(user);
            await _dc.SaveChangesAsync();
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;//rastgele üretilir
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _dc.Users.AnyAsync(a => a.Username == username))
                return true;

            return false;

        }
    }

}