using System;
using Microsoft.AspNetCore.Http;

namespace KimOkurAPP.API.Dtos
{
    public class PhotoForCreationDto
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public int PublicId { get; set; }


        PhotoForCreationDto()
        {
            DateAdded = DateTime.Now;
        }
    }
}