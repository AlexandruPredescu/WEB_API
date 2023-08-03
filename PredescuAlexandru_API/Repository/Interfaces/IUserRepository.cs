using PredescuAlexandru_API.Models.Authentification;

namespace PredescuAlexandru_API.Repository.Interfaces
{
    public interface IUserRepository
    {
       Task<AuthenticationResponse> Authenticate(AuthentiticateRequest request);
    }
}
