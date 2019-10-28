using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using KimOkur.API.Data;
using KimOkurAPP.API.Dtos;
using KimOkurAPP.API.Helpers;
using KimOkurAPP.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KimOkurAPP.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public MessagesController(IDatingRepository repository, IMapper mapper)
        {
            this._repo = repository;
            this._mapper = mapper;
        }
        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.GetMessage(id);

            if (messageFromRepo == null)

                return NotFound();

            return Ok(messageFromRepo);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto messageforCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageforCreationDto.SenderId = userId;
            var recipient = await _repo.GetUser(messageforCreationDto.RecipientId);
            if (recipient == null)
                return BadRequest("Kullanıcı bulunamadı.");
            var message = _mapper.Map<Message>(messageforCreationDto);
            _repo.Add(message);
            var messageToReturn=_mapper.Map<MessageForCreationDto>(message);
            if (await _repo.SaveAll())
                return CreatedAtRoute("GetMessage", new { id = message.Id }, messageToReturn);
            throw new Exception("Mesaj oluşturma işlemi başarısız oldu.");
 
        }
    }
}