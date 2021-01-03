using API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    //Each methods added here must be implemented within class implemented from this interface
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
