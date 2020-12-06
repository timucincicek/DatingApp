using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    //ApiController attribute required to define the class as a controller
    //Route is a handler and [controller] will be replaced with Users
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            //Injection needed to access db through Dbcontext
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            //DB values stored in variable
            return await _context.Users.ToListAsync();
  
        }
        //with id
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            //DB values stored in variable
            return await _context.Users.FindAsync(id);
 
        }
    }
}
