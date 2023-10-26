using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Models
{
    public class User : IdentityUser
    {
        public User() : base() { }
    }
}
