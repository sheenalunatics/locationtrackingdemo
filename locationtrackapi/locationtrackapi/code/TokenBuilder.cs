using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace locationtrackapi
{
    public static class TokenBuilder
    {
        private const string keyString = "DA7zgKy5ilF26BhFonC9ZlkW0FwNMFWUL+K76/VqVRk=";
         // private const string keyString = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed";
        public static readonly byte[] symmetricKeyBytes = Encoding.ASCII.GetBytes(keyString);
        public static readonly SymmetricSecurityKey symmetricKey = new SymmetricSecurityKey(symmetricKeyBytes);
        public static readonly SigningCredentials signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
        internal static TokenValidationParameters tokenValidationParams;

        private static string GenerateHmacKey(string secretkey,string message)
        {       
            byte[] keyByte = new ASCIIEncoding().GetBytes(secretkey);
            byte[] messageBytes = new ASCIIEncoding().GetBytes(message);            
            byte[] hashmessage = new HMACSHA256(keyByte).ComputeHash(messageBytes);    
            // to lowercase hexits
            String.Concat(Array.ConvertAll(hashmessage, x => x.ToString("x2")));            
            // to base64
            return Convert.ToBase64String(hashmessage);
        }

        //Construct our JWT authentication paramaters then inject the parameters into the current TokenBuilder instance
        // If injecting an RSA key for signing use this method
        // Be weary of common jwt trips: https://trustfoundry.net/jwt-hacking-101/ and https://www.sjoerdlangkemper.nl/2016/09/28/attacking-jwt-authentication/
        //public static void ConfigureJwtAuthentication(this IServiceCollection services, RSAParameters rsaParams)
        public static void ConfigureJwtAuthentication(this IServiceCollection services,IConfiguration _config)
        {
            tokenValidationParams = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidateLifetime = true,
                    ValidAudience = _config["Jwt:Audience"],
                    ValidateAudience = true,
                    RequireSignedTokens = true,                
                    // Use our signing credentials key here
                    // optionally we can inject an RSA key as
                    //IssuerSigningKey = new RsaSecurityKey(rsaParams),
                    IssuerSigningKey = signingCredentials.Key,
                    ClockSkew = TimeSpan.FromMinutes(0)
                };

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParams;
#if PROD || UAT
                options.IncludeErrorDetails = false;
#elif DEBUG
                options.RequireHttpsMetadata = false;
#endif
            });
        }
        public static string CreateJsonWebToken(
               string username,
               IEnumerable<string> roles,
               string audienceUri,
               string issuerUri,
               Guid applicationId,
               DateTime expires,
               string deviceId = null,
               bool isReAuthToken = false)
        {
            var claims = new List<Claim>();
            if(!String.IsNullOrEmpty(username))
            {
                 claims.Add(new Claim(ClaimTypes.Name, username));
            }
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }
            var head = new JwtHeader();
            var payload = new JwtPayload(claims.ToArray());
            var jwt = new JwtSecurityToken(issuerUri, audienceUri, claims, DateTime.UtcNow, expires, signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}