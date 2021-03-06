using System;
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
    [ApiController]
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
        [FromForm] PhotoForCreationDto PhotoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _rp.GetUser(userId);

            var file = PhotoForCreationDto.File;

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
            PhotoForCreationDto.Url = uploadResult.Uri.ToString();
            PhotoForCreationDto.PublicId = uploadResult.PublicId;
            PhotoForCreationDto.DateAdded = DateTime.Now;
            //Mapping
            var photo = _mp.Map<Photo>(PhotoForCreationDto);
            photo.IsMain=true;
            if (userFromRepo.Photos.Any(async => async.IsMain))
            {
                photo.IsMain = false;
            }
            
            userFromRepo.Photos.Add(photo);


            if (await _rp.SaveAll())
            {
                var photoForReturn = _mp.Map<PhotoForReturnDto>(photo);
                return CreatedAtAction("GetUserPhoto", new { id = photo.Id }, photoForReturn);
            }
            return BadRequest("Fotoğraf yüklemesi başarısız oldu");
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _rp.GetUser(userId);
            if (!user.Photos.Any(async => async.Id == id))
                return Unauthorized();

            var photoFromRepo = await _rp.GetUserPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("Fotoğraf zaten profil fotoğrafıdır.");

            var currentMainPhoto = await _rp.GetMainPhotoForUser(userId);
            currentMainPhoto.IsMain = false;
            photoFromRepo.IsMain = true;

            if (await _rp.SaveAll())
                return NoContent();

            return BadRequest("Fotoğraf bulunamadı.");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _rp.GetUser(userId);
            if (!user.Photos.Any(async => async.Id == id))
                return Unauthorized();

            var photoFromRepo = await _rp.GetUserPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("Profil fotoğrafı silinemez.");

            if (photoFromRepo.PublicId != null)
            {
                //Clouidinary apiden fotoğraflar silinebilmeli
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);

                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                {
                    _rp.Delete(photoFromRepo);
                }
            }
            else
            {
                _rp.Delete(photoFromRepo);
            }
            if (await _rp.SaveAll())
            {
                return Ok();
            }


            return BadRequest("Fotoğraf silme işlemi başarısız oldu.");
        }
        //Kitap fotoğraflarıda eklenecek
    }


}
