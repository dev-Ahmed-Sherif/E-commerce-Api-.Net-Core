using E_commerce_Api.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_Api.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
