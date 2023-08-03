namespace PredescuAlexandru_API.Models.Authentification
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }

        public AuthenticationResponse(string token)
        {
            Token = token;
        }
    }
}
