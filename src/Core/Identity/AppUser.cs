using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace E_commerce_Api.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }
    }
}
