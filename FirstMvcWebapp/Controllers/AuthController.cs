using FirstMvcWebapp.Data;
using FirstMvcWebapp.Dto;
using FirstMvcWebapp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FirstMvcWebapp.Controllers
{
    public class AuthController(AppDbContext _context) : Controller
    {
      
        public IActionResult Login()
        {
            ViewBag.SuccessMessagge = TempData["SuccessMessagge"];
            return View();
        }

        public IActionResult Register() 
        {
            return View();
        }

        public async Task<IActionResult> CreateUser(UserDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password)) 
            {
                ViewBag.ErrorMessage = "Please kindly fill all the details.. ";
                return View("Register");
            }
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (existingUser == null)
            {
                var user = new User
                {
                    Email = dto.Email,
                    Password = dto.Password,
                    Name = dto.Name
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            else 
            {
                ViewBag.ErrorMessage = "User with this Email already exist. ";
                return View("Register");
            }
            TempData["SuccessMessagge"] = "User created successfully..Please login";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> LoginUser(UserDto dto) 
        {
            if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password)) 
            {
                ViewBag.ErrorMessage = "Kindly fill all the details";
                return View("Login");
            }
            var IsUserExist = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (IsUserExist == null)
            {
                ViewBag.ErrorMessage = "User with this Email doesn't Exist";
                return View("Login");
            }
            else 
            {
                if (IsUserExist.Password == dto.Password)
                {
                   var token = GenerateJwtToken(dto);

                    Response.Cookies.Append("jwt_key", token , new CookieOptions { 
                    
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                    });
                    return RedirectToAction("Index", "Dashboard");
                }
                else 
                {
                    ViewBag.ErrorMessage = "Incorrect Password";
                    return View("Login");
                }
            }
        }

        private string GenerateJwtToken(UserDto dto) 
        {   
            var jwthandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("TtmgnjKppWXOqkQNK8P7ejKDXF4iynwpc1neoKxzxef");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name , dto.Email),
            }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwthandler.CreateToken(tokenDescriptor);

            return jwthandler.WriteToken(token);
        }

        public IActionResult LogoutUser() 
        {
            Response.Cookies.Delete("jwt_key");
            return RedirectToAction("Login");
        }
    }
}
