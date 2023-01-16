using MessagePack;
using Microsoft.Build.Framework;

namespace WebAPIInterface.Models
{
    public class UserEntity
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public string Emailaddress { get; set; }
        public string Mobilenumber { get; set; }
        public string Password { get; set; }

    }
}
