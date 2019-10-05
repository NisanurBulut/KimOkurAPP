using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using KimOkur.API.Data;
using KimOkur.API.Helpers;
using KimOkur.API.Models;
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



        [HttpGet("{id}", Name = "GetUserPhoto")]
        public async Task<IActionResult> GetUserPhoto(int id)
        {
            var photoFromRepo = await _rp.GetUserPhoto(id);
            var photo = _mp.Map<PhotoForReturnDto>(photoFromRepo);
            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId,
         PhotoForCreationDto photoForCreation)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _rp.GetUser(userId);

            var file = photoForCreation.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                        .Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            photoForCreation.Url = uploadResult.Uri.ToString();
            photoForCreation.PublicId = uploadResult.PublicId;
            //Mapping
            var photo = _mp.Map<Photo>(photoForCreation);
            if (userFromRepo.Photos.Any(async => async.IsMain))
            {
                photo.IsMain = true;
            }
            userFromRepo.Photos.Add(photo);
          

            if (await _rp.SaveAll())
            {  
                 var photoForReturn=_mp.Map<PhotoForReturnDto>(photo);
                return CreatedAtAction("GetUserPhoto", new { id = photo.Id },photoForReturn);
            }
            return BadRequest("Fotoğraf yüklemesi başarısız oldu");
        }

    }
    //Kitap fotoğraflarıda eklenecek
}
