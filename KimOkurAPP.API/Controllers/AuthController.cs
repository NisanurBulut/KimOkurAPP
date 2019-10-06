using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KimOkur.API.Data;
using KimOkur.API.Dtos;
using KimOkur.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace KimOkur.API.Controllers
{
    [Route("/api/[Controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _rp;
        private readonly IConfiguration _config;
        private readonly IMapper _mp;
        public AuthController(IAuthRepository rp, IConfiguration config, IMapper mapper)
        {
            _mp = mapper;
            _config = config;
            _rp = rp;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto ufrDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //isteği doğrula
            ufrDto.Username = ufrDto.Username.ToLower();
            if (await _rp.UserExists(ufrDto.Username))
                return BadRequest("Kullanıcı mevcutta vardır.");
            var userToCreate = new User()
            {
                Username = ufrDto.Username
            };
            var createdUser = await _rp.Register(userToCreate, ufrDto.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto uflDto)
        {

            var userFromRepo = await _rp.Login(uflDto.Username, uflDto.Password);
            if (userFromRepo == null)
                return Unauthorized();
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userFromRepo.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(_config.GetSection("Appsettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var user=_mp.Map<UserListForDto>(userFromRepo);
            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user=
            });
        }
    }
}

