using System.Threading.Tasks;
using DatingAPP.API.Data;
using DatingAPP.API.Dtos;
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
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto ufrDto)
        {
            //isteği doğrula
            ufrDto.Username = ufrDto.Username.ToLower();
            if (await _rp.UserExists(ufrDto.Username))
                return BadRequest("Kullanıcı mevcutta vardır.");
            var userToCreate = new User()
            {
                Username = ufrDto.Username
            };
            var createdUser=await _rp.Register(userToCreate,ufrDto.Password);
            
            return StatusCode(201);
        }
    }
}

