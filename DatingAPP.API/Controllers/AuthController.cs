using System.Threading.Tasks;
using DatingAPP.API.Data;
using DatingAPP.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingAPP.API.Controllers
{
    [Route("/api/[Controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _rp;
        public AuthController(IAuthRepository rp)
        {
            _rp = rp;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string password)
        {
            //isteği doğrula
            username = username.ToLower();
            if (await _rp.UserExists(username))
                return BadRequest("Kullanıcı mevcutta vardır.");
            var userToCreate = new User()
            {
                Username = username
            };
            var createdUser=await _rp.Register(userToCreate,password);
            
            return StatusCode(201);
        }
    }
}

