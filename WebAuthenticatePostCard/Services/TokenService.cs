using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.IdentityModel.Tokens.Jwt;
using WebAuthenticatePostCard.Model;
namespace WebAuthenticatePostCard.Services
{
    public class TokenService
    {
        //Secret is randomly chosen from https://www.grc.com/passwords.htm
        private static string Secret = "7239FF9E67D4AA428B717AA4DC134E78B2D9505227BA0526C8BEBC3A27EF5358";
        private static JwtSecurityTokenHandler JWTTokenHandler = new JwtSecurityTokenHandler();
        private static Microsoft.IdentityModel.Tokens.SymmetricSecurityKey SecurityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(Secret));

        public static string CreateToken(UserModel user)
        {
            try
            {
                var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                SecurityKey,
                Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

                //Add Claim to the token
                List<Claim> claimList = new List<Claim>();
                claimList.Add(new Claim(ClaimTypes.Name, user.Name));
                claimList.Add(new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()));
                //foreach (var role in user.Roles)
                //    claimList.Add(new Claim(ClaimTypes.Role, role));

                var claimsIdentity = new ClaimsIdentity(claimList, "Custom");

                var securityTokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
                {
                    Audience = "WebAuthenticatePostCard",
                    Issuer = "WebAuthenticatePostCard",
                    IssuedAt = DateTime.Now,
                    NotBefore = DateTime.Now,
                    Expires = DateTime.Now.AddMinutes(6000),
                    Subject = claimsIdentity,
                    SigningCredentials = signingCredentials,
                };
                var plainToken = JWTTokenHandler.CreateToken(securityTokenDescriptor);
                var signedAndEncodedToken = JWTTokenHandler.WriteToken(plainToken);
                return signedAndEncodedToken;
            }

            catch (Exception ex)
            {
                throw;
            }
        }

        public static ClaimsPrincipal GetPrincipleFromToken(string token)
        {
            try
            {
                var tokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidAudiences = new string[] { "WebAuthenticatePostCard" },
                    ValidIssuers = new string[] { "WebAuthenticatePostCard" },
                    IssuerSigningKey = SecurityKey
                };
                Microsoft.IdentityModel.Tokens.SecurityToken validatedToken;
                return JWTTokenHandler.ValidateToken(token,
                        tokenValidationParameters, out validatedToken);
            }

            catch (Exception ex)
            {
                throw;
            }
        }
    }
}