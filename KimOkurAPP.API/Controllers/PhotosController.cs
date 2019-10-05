using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using KimOkur.API.Data;
using KimOkur.API.Helpers;
using KimOkurAPP.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KimOkur.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository _rp;
        private readonly IMapper _mp;

        private readonly Cloudinary _cloudinary;
        private readonly IOptions<CloudinarySettings> _cc;

        public PhotosController(IDatingRepository repo,
                                IMapper mapper,
                                IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _rp = repo;
            _mp = mapper;
            _cc = cloudinaryConfig;


            Account acc = new Account(_cc.Value.CloudName, _cc.Value.ApiKey, _cc.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId,
         PhotoForCreationDto photoForCreation)
        {

        }
        //Kitap fotoğraflarıda eklenecek
    }
}