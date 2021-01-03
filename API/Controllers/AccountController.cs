using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context; 

        private readonly ITokenService _tokenService;
        public AccountController(DataContext context,ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        //Parameter set to define whats coming
        //RegisterDto guarantees the type of incoming body
        //ActionResult makes us able to return different status codes
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto) 
        {

            if (await UserExists(registerDto.Username))
            {
                //400
                return BadRequest("Username is taken");
            }

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            //To begin tracking
            _context.Users.Add(user);
            //To save all changes to db
            await _context.SaveChangesAsync();

            return new UserDto {
                Username = registerDto.Username.ToLower(),
                Token = _tokenService.CreateToken(user)
            };

        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null) return Unauthorized("Invalid username");

            //In the constructor takes Salt to encrypt
            using var hmac = new HMACSHA512(user.PasswordSalt);
            //Hash generated from salt
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            //Each element in bytearray are compared
            for (int i= 0; i <= (computedHash.Length) - 1; i++) {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            return new UserDto
            {
                Username = loginDto.Username.ToLower(),
                Token = _tokenService.CreateToken(user)
            };
        }
        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(el => el.UserName == username.ToLower());
        }
    }
}
