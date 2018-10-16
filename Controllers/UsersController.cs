using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieApp.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        DbAccess dataObject = new DbAccess();

        [HttpGet, Authorize]
        public IEnumerable<TblModels> Get() { return dataObject.GetModels(); }

        [HttpPost, Route("Login")]
        public IActionResult Login([FromBody]  LoginDetails login)
        {
            if (login == null) { return BadRequest("Invalid client request"); }

            if (dataObject.LoginDataCheck(login))
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim> {
                 new Claim(ClaimTypes.Email, login.Email),
                  new Claim(ClaimTypes.Name, login.Email) };

                var tokeOptions = new JwtSecurityToken(
                    issuer: "http://localhost:63269",
                    audience: "http://localhost:63269",
                    claims: claims,
                    //expires: DateTime.Now.AddMinutes(525600),
                    expires: DateTime.Now.AddDays(60),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString });
            }
            else { return Unauthorized(); }
        }

        [HttpPost, Route("Register")]
        public bool Register([FromBody] TblUsers user)
        {
            bool AddedUser = false;
            AddedUser = dataObject.AddUser(user);
            return AddedUser;
        }


        [HttpGet, Route("MyProfile"), Authorize]
        public TblUsers MyProfile()
        {
            string name = HttpContext.User.Identity.Name.ToString();
            return dataObject.GetUserById(name);
        }

        [HttpPost, Route("SaveMyProfile"), Authorize]
        public TblUsers SaveMyProfile([FromBody] TblUsers user)
        {
            return dataObject.SaveChangesToProfile(user);
        }
    }
}
