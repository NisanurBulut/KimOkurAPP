using DatingAPP.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace DatingAPP.API.Controllers
{
    [Route("/api/[Controller]")]
    [ApiController]
    public class AuthController
    {
        private readonly IAuthRepository _rp;
        public AuthController(IAuthRepository rp)
        {
            _rp = rp;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string password) {
            //isteği doğrula
            username=username.ToLower();
         }
    }
}