using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAPP.API.Controllers
{
    [Authorize]
    [Route('api/[controller]')]
    [ApiController]
    public class UserController:ControllerBase
    {
        
    }
}