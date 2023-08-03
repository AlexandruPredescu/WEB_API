using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PredescuAlexandru_API.DataContext;
using PredescuAlexandru_API.Models.Authentification;
using PredescuAlexandru_API.Repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PredescuAlexandru_API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ClubLibraDataContext _context;
        private readonly IConfiguration _configuration;

        public UserRepository(ClubLibraDataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthenticationResponse> Authenticate(AuthentiticateRequest request)
        {
            var user =  await _context.Members.SingleOrDefaultAsync(x => x.Username == request.Username && x.Password == request.Password);
            if (user == null)
            {
                return null;
            }
            var token = await GenerateJwtToken(request);

            return new AuthenticationResponse(token);
        }

        private async Task<string> GenerateJwtToken (AuthentiticateRequest request)
        {
            var securityKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                (_configuration.GetValue<string>("Autentication:Secret")));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration.GetValue<string>("Autentication:Domain"),
                _configuration.GetValue<string>("Autentication:Audience"),
                null,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials); 

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
