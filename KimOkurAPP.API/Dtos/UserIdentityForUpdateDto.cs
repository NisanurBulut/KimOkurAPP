using System;

namespace KimOkur.API.Dtos
{
    public class UserIdentityForUpdateDto
    {
        public string Username { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}