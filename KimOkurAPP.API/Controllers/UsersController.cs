using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using KimOkur.API.Data;
using KimOkur.API.Dtos;
using KimOkur.API.Helpers;
using KimOkur.API.Models;
using KimOkurAPP.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KimOkur.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromrepo = await _repo.GetUser(currentUserId);

            userParams.UserId = currentUserId;

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userFromrepo.Gender=="kadin"?"erkek":"kadin";
            }
            var users = await _repo.GetUsers(userParams);
            var usersToReturn = _mapper.Map<IEnumerable<UserListForDto>>(users);
            Response.AddPagination(users.currentPage, users.PageSize,
                users.TotalCount, users.TotalPages);
            return Ok(usersToReturn);
        }
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }
        [Route("[action]/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUserIdentity(int id)
        {
            var user = await _repo.GetUser(id);
            var userToReturn = _mapper.Map<UserIdentityForUpdateDto>(user);
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _repo.SaveAll())
                return NoContent();
            throw new Exception($"{id} nolu kişinin bilgileri güncellenemedi.");
        }
        [Route("[action]/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateIdentityUser(int id, UserIdentityForUpdateDto userIdentityForUpdate)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userIdentityForUpdate, userFromRepo);

            if (await _repo.SaveAll())
                return NoContent();
            throw new Exception($"{id} nolu kişinin bilgileri güncellenemedi.");
        }
          [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var like = await _repo.GetLike(id, recipientId);

            if (like != null)
                return BadRequest("Bu kullanıcı zaten favorileriniz arasındadır.");
            
            if (await _repo.GetUser(recipientId) == null)
                return NotFound();

            like = new Like
            {
                LikerId = id,
                LikeeId = recipientId
            };

            _repo.Add<Like>(like);

            if (await _repo.SaveAll())
                return Ok();
            
            return BadRequest("İstenmeyen bir hata ile karşılaşıldı.");
        }
    }
}