namespace KidQquest.Services
{
    public interface IJwtService
    {
        public string Encode(string email);
        public string Decode(string jwt);
        public bool IsJwtToken(string jwt);
    }
}