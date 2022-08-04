using JwtAuthentication.Models;
using JwtAuthentication.Models.BaseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace JwtAuthentication.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly CustomerDbContext _context;
        private readonly JwtSetting _settings;
        private readonly RefreshTokenGenerator _refreshTokengenerator;
        public UserController(CustomerDbContext context, RefreshTokenGenerator refreshTokengenerator, IOptions<JwtSetting> options)
        {
            _context = context;
            _settings = options.Value;
            _refreshTokengenerator = refreshTokengenerator;
        }


        // THREE
        // For RefreshToken
        public TokenResponse Authenticate(string username, Claim[] claimss)
        {
            TokenResponse tokenResponse = new TokenResponse();
            var tokenkey = Encoding.UTF8.GetBytes(_settings.securitykey);  // converted tokenkey to bytes
            var tokenhandler = new JwtSecurityToken(
                claims: claimss,
                expires: DateTime.Now.AddMinutes(3),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
                );
            tokenResponse.JWTToken = new JwtSecurityTokenHandler().WriteToken(tokenhandler);
            tokenResponse.RefreshToken = _refreshTokengenerator.GeneratorToken(username);
            return tokenResponse;
        }


        // ONE
        [Route("Authenticate")]
        [HttpPost]
        public IActionResult Authenticate([FromBody] UserCred user)
        {
            // For Refresh Token
            TokenResponse tokenResponse = new TokenResponse();
            // End

            var _user = _context.TblUserr.FirstOrDefault(o => o.Userid == user.UserName && o.Password == user.Password);
            if (_user == null)
                return Unauthorized();

            // Step 3 JWTAuthentication

            // If Unauthorized -> Create Token
            var tokenhandler = new JwtSecurityTokenHandler();  
            
            var tokenkey = Encoding.UTF8.GetBytes(_settings.securitykey); /// from appsettings.json -> vvv-Imp
            
            var tokenDescriptor = new SecurityTokenDescriptor  
            {
                // item 1
                Subject = new ClaimsIdentity(  
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, _user.Userid),
                        new Claim(ClaimTypes.Role, _user.Role)
                    }),
                // item 2
                Expires = DateTime.Now.AddMinutes(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenhandler.CreateToken(tokenDescriptor);
            string finaltoken = tokenhandler.WriteToken(token);

            //return Ok(finaltoken);  // before refresh token

            // response token
            tokenResponse.JWTToken = finaltoken;
            tokenResponse.RefreshToken = _refreshTokengenerator.GeneratorToken(user.UserName);
            return Ok(tokenResponse);
        }


        // TWO
        // Refresh Token Method
        [Route("Refresh")]
        [HttpPost]
        public IActionResult Refresh([FromBody] TokenResponse token)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenhandler.ValidateToken(token.JWTToken, new TokenValidationParameters 
            {
                /// from startup.cs 
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.securitykey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            }, out securityToken);

            var _token = securityToken as JwtSecurityToken;
            
            if (_token != null && !_token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                return Unauthorized();
            }

            var userName = principal.Identity.Name;
            var _refTable = _context.TblRefreshtokenn.FirstOrDefault(o => o.UserId == userName && o.RefreshToken == token.RefreshToken);
            
            if(_refTable == null)
            {
                return Unauthorized();
            }
            TokenResponse _result = Authenticate(userName, principal.Claims.ToArray());
            return Ok(_result);
        }
    }
}
