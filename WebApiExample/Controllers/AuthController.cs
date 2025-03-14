using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiExample.Models;
using WebApiExample.Models.Rest;

namespace WebApiExample.Controllers;

[Route("api/v1/auth")]
[ApiController]
public class AuthController(UserManager<User> userManager, IConfiguration configuration) : ControllerBase
{
    [HttpPost("login")]
    public async Task<AuthResult> Login(LoginRest loginRest)
    {
        var user = await userManager.FindByEmailAsync(loginRest.Email);

        if (user == null)
        {
            Response.StatusCode = 400;
            return new AuthResult
            {
                Success = false,
                Error = "User not found"
            };
        }

        if (!await userManager.CheckPasswordAsync(user, loginRest.Password))
        {
            Response.StatusCode = 400;
            return new AuthResult
            {
                Success = false,
                Error = "Wrong password"
            };
        }

        return new AuthResult
        {
            Success = true,
            Token = GenerateToken(user)
        };
    }

    [HttpPost("register")]
    public async Task<AuthResult> Register(RegisterRest registerRest)
    {
        if (await userManager.FindByEmailAsync(registerRest.Email) != null)
        {
            Response.StatusCode = 400;
            return new AuthResult
            {
                Success = false,
                Error = "User already exists"
            };
        }

        var user = new User()
        {
            Email = registerRest.Email,
            UserName = registerRest.Email,
        };


        var result = await userManager.CreateAsync(user, registerRest.Password);

        if (!result.Succeeded)
        {
            Response.StatusCode = 400;
            return new AuthResult
            {
                Success = false,
                Error = "Error .... (password)"
            };
        }

        //await userManager.AddToRoleAsync(user, "User");

        return new AuthResult
        {
            Success = true,
            Token = GenerateToken(user)
        };
    }


    private string GenerateToken(User user)
    {
        var secret = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
        var key = new SymmetricSecurityKey(secret);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(10),
            signingCredentials: creds
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}


