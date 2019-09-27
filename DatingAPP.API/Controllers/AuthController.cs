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
    }
}