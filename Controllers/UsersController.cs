using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PEI_API.EF;
using PEI_API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;

namespace PEI_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        private peiDBContext _db;
        private readonly JwtService _jwtService;

        public UsersController(peiDBContext db, JwtService jwtService)
        {
            _db = db;
            _jwtService = jwtService;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(PeiUser user)
        {
            var userEmail = await _db.PeiUsers
                               .Where(u => u.UEmail == user.UEmail).ToListAsync();
            bool isEmpty = !userEmail.Any();

            if (isEmpty)
            {
                var newUser = new PeiUser
                {
                    UFirstName = textInfo.ToTitleCase(user.UFirstName),
                    ULastName = textInfo.ToTitleCase(user.ULastName),
                    UEmail = user.UEmail,
                    UPassword = BCrypt.Net.BCrypt.HashPassword(user.UPassword), // Hash the password
                    UStatus = true,
                    UAuthLevel = "Public"
                };
                await _db.PeiUsers.AddAsync(newUser);
                await _db.SaveChangesAsync();
               
                return Ok(new { message = "Registration Successfull" });
            }

            return BadRequest(new { message = "This email address is taken." });

        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> login(PeiUser user)
        {
            var attemptedUser = await _db.PeiUsers.FirstOrDefaultAsync(u => u.UEmail == user.UEmail);
            if (attemptedUser == null)
            {
                return BadRequest(new { message = "Invalid credentials" });
            }
            else
            {
                if (!BCrypt.Net.BCrypt.Verify(user.UPassword, attemptedUser.UPassword))
                {
                    return BadRequest(new { message = "Invalid credentials" });
                }
                else
                {
                    var jwt = _jwtService.Generate(attemptedUser.UId, attemptedUser.UAuthLevel); // Generate the Access token that expires in one day

                    Response.Cookies.Append("jwt", jwt, new CookieOptions //Save the JWT in the browser cookies, Key is "jwt"
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.None,
                        Secure = true
                    });

                    return Ok(new { message = "You are now logged in" });
                }
            }
        }


        [Route("getuser")]
        [HttpGet]
        public async Task<IActionResult> getuser()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);

                int userId = int.Parse(token.Issuer);

          

                //var attemptedUser = await _db.PeiUsers.FirstOrDefaultAsync(u => u.UId == userId);
                var attemptedUser = await (from user in _db.PeiUsers
                                          where user.UId == userId
                                          select new
                                          {
                                              uId = user.UId,
                                              uFirstName = user.UFirstName,
                                              uEmail = user.UEmail,
                                              uAuthLevel = user.UAuthLevel,
                                              uTimeStamp = user.UTimeStamp,
                                          }).ToListAsync();

                return Ok(attemptedUser);
            }
            catch (Exception)
            {
                return Unauthorized(new { message = "You are not authorized" });
            }

        }


        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> logout()
        {
            Response.Cookies.Delete("jwt", new CookieOptions 
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true
            });

            return Ok(new { message = "Success" });

        }
    }
}
