
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ModelsLibrary.Models;
using ServicesLibrary.Exceptions.UserManagerExceptions;
using SharedModelsLibrary.UserDTOs;


namespace ServicesLibrary.Generators
{
    public  class JwtProvider(IConfiguration config)
    {
        public TokenView Create(ApplicationUser user)
        {
            try
            {
                var secretKey = config["Jwt:Key"];
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(
                    [
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Email, user.Email)
                    ]),
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    SigningCredentials = credentials,
                    Issuer = config["Jwt:Issuer"],
                    Audience = config["Jwt:Audience"]
                };

                var expiration = tokenDescriptor.Expires;
                var handler = new JsonWebTokenHandler();
                var token = handler.CreateToken(tokenDescriptor);
                return new TokenView
                {
                    Token = token,
                    Expiration = expiration
                };
            }
            catch (Exception ex)
            {

                throw new TokenProviderException("Something went wrong during the creation of token!" , ex);
            }
            
        }
    }
}
