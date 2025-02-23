using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Api.Identity
{
    public class SeedIdentityData
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Bob",
                    Email = "ahmed@gmail.com",
                    UserName = "ahmed",
                    Address = new Address
                    {
                        FirstName = "Ahmed",
                        LastName = "Mohamed",
                        Street = "10 The Street",
                        City = "New York",
                        State = "NY",
                        Zipcode = "90210"
                    }
                };
                await userManager.CreateAsync(user,"P@ssw0rd");
            }
        }
    }
}

