using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace KimOkurAPP.API.Controllers
{
    public class FallbackController:Controller
    {
        public IActionResult Index(){
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/HTML");
        }
    }
}