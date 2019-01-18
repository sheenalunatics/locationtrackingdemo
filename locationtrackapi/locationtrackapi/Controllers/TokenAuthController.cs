using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using locationtrackapi.BAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace locationtrackapi.Controllers
{
    [Route("api/[controller]")]
    public class TokenAuthController : Controller
    {
        private ILoggerManager _logger;
        private IUserBAL _objUserBll;
        public TokenAuthController(IUserBAL objUserBll)
        {
            _objUserBll = objUserBll;
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
            // if ((principal != null) && (principal.Claims != null))
            // {
            //     var jwtSecurityToken = securityToken as JwtSecurityToken;
            //     Trace.WriteLine(jwtSecurityToken.Issuer);
            // }
            return Ok();
        }

        [HttpPost]
        [Route("tokens")]
        public IActionResult Post()
        {
            //  public static string CreateJsonWebToken(string username, IEnumerable<string> roles, string audienceUri, string issuerUri
            //, Guid applicationId, DateTime expires, string deviceId = null, bool isReAuthToken = false)
            var model = TokenBuilder.CreateJsonWebToken("ukrit.s", new List<string>() { "Administrator" } ,  "http://localhost:5000", "http://localhost:5000", "3995132E-22B0-493E-A4BF-2FF52509FAF9", DateTime.UtcNow.AddMinutes(30));
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
            
            try
            {
                if (ValidateUser(user))
                {
                    var model = TokenBuilder.CreateJsonWebToken(user.UserName, null, null, user.Issuer, user.ApplicationID, DateTime.UtcNow.AddDays(1));
                    return Ok(model);
                }
                else
                {
                    return Unauthorized();
                }
            } 
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside the LocationTracking action: {ex}");
                return StatusCode(500, "Internal server error");
            }  
           
        }
        
        private bool ValidateUser(LoginModel user)
        {
            if (string.IsNullOrEmpty(user.UserName))
            {
                throw new ArgumentException("Argument cannot be null or empty", "username");
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentException("Argument cannot be null or empty", "password");
            }
            if (user.UserType == "api")
            {
                //return false;
                LoginModel userModel = CheckUser(user.UserName,user.ApplicationID);
                if (user == null) return false;
                return userModel.Password == Helper.EncodePassword(user.Password, userModel.PasswordSalt);
            }
            else if (user.UserType == "rms")
            {
                return false;
            }
            else if (user.UserType == "pms")
            {
                return false;
            }
            else if (user.UserType == "ad")
            {
                return false;
            }

            return false;
        }

        public LoginModel CheckUser(string username,string appId)
        {         
                var users = _objUserBll.GetUsers() as List<LoginModel>;
                return users.Find(s => s.UserName == username && s.ApplicationID == appId);                 
        }
      

    }
}