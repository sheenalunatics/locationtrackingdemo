using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace locationtrackapi.Controllers
{
    [Route("api/[controller]")]
    public class TokenAuthController : Controller
    {
       // private UserRepository userRepository;
        public TokenAuthController()
        {
          //  this.userRepository = new UserRepository();
        }

        [HttpGet]
        [Route("tokens2")]
        [AllowAnonymous]
        public IActionResult Get2()
        {
            var authenticationHeaders = Request.Headers["Authorization"].ToArray();
            if ((authenticationHeaders == null) || (authenticationHeaders.Length != 1))
            {
                return BadRequest();
            }
            var jwToken = authenticationHeaders[0].Split(' ')[1];
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = null;
            SecurityToken securityToken = null;
            try
            {
                principal = jwtSecurityTokenHandler.ValidateToken(jwToken, TokenBuilder.tokenValidationParams, out securityToken);
            }
            catch(SecurityTokenExpiredException ex)
            {
                return StatusCode(401,"The token is expired, please try to login again.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if ((principal != null) && (principal.Claims != null))
            {
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                Trace.WriteLine(jwtSecurityToken.Audiences.First());
                Trace.WriteLine(jwtSecurityToken.Issuer);
            }
            return Ok();
        }

        [HttpPost]
        [Route("tokens")]
        public IActionResult Post()
        {
            //  public static string CreateJsonWebToken(string username, IEnumerable<string> roles, string audienceUri, string issuerUri
            //, Guid applicationId, DateTime expires, string deviceId = null, bool isReAuthToken = false)
            var model = TokenBuilder.CreateJsonWebToken("ukrit.s", new List<string>() { "Administrator" } ,  "http://localhost:5000", "http://localhost:5000", Guid.NewGuid(), DateTime.UtcNow.AddMinutes(30));
            return Ok(model);
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]LoginModel user)
       // public IActionResult Login(string username)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }
    
            var model = TokenBuilder.CreateJsonWebToken(user.UserName, new List<string>() { "Administrator" } ,  "http://localhost:5000", "http://localhost:5000", Guid.NewGuid(), DateTime.UtcNow.AddMinutes(30));
            return Ok(model);
            // if (user.UserName == "johndoe" && user.Password == "def@123")
            // {
            //     var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            //     var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
    
            //     var tokeOptions = new JwtSecurityToken(
            //         issuer: "http://localhost:5000",
            //         audience: "http://localhost:5000",
            //         claims: new List<Claim>(),
            //         expires: DateTime.Now.AddMinutes(5),
            //         signingCredentials: signinCredentials
            //     );
    
            //     var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            //     return Ok(new { Token = tokenString });
            // }
            // else
            // {
            //     return Unauthorized();
            // }
        }
        
    // [HttpPost]
    // public string GetAuthToken([FromBody]LoginModel user)
    // {
    //     var existUser = new LoginModel{ UserName = user.UserName, Password = user.Password}; //new User(); //UserStorage.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

    //     if (existUser != null)
    //     {
    //         var requestAt = DateTime.Now;
    //         var expiresIn = requestAt.Add(TimeSpan.FromMinutes(30));
    //         var token = GenerateToken(existUser, expiresIn);

    //         return JsonConvert.SerializeObject(new
    //         {
    //             requestAt = requestAt,
    //             expiresIn = TimeSpan.FromMinutes(30).TotalSeconds,
    //             tokeyType = "Bearer",
    //             accessToken = token
    //         });
    //     }
    //     else
    //     {
    //         return JsonConvert.SerializeObject(new 
    //         {
    //             Msg = "Username or password is invalid"
    //         });
    //     }
    // }

    // private string GenerateToken(LoginModel user, DateTime expires)
    // {
    //     var handler = new JwtSecurityTokenHandler();

    //     ClaimsIdentity identity = new ClaimsIdentity(
    //         new GenericIdentity(user.UserName, "TokenAuth"),
    //         new[] {
    //             new Claim("ID", user.ID.ToString())
    //         }
    //     );

    //     var securityToken = handler.CreateToken(new SecurityTokenDescriptor
    //     {
            
    //         Issuer = "Issuer",
    //         Audience = "Audience",
    //         SigningCredentials = new SigningCredentials(new RsaSecurityKey(new RSACryptoServiceProvider(2048).ExportParameters(true)), SecurityAlgorithms.RsaSha256Signature),
    //         Subject = identity,
    //         Expires = expires,
    //         NotBefore = DateTime.Now.Subtract(TimeSpan.FromMinutes(30))
    //     });
    //     return handler.WriteToken(securityToken);
    // }

    // [HttpGet]
    // [Authorize("Bearer")]
    // public string GetUserInfo()
    // {
    //     var claimsIdentity = User.Identity as ClaimsIdentity;

    //     return JsonConvert.SerializeObject(new 
    //     {
    //         UserName = claimsIdentity.Name
    //     });
    // }
    }
}