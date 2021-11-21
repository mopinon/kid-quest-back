namespace KidQquest.Services
{
    public interface IHashService
    {
        public string HashPassword(string password);
        public bool VerifyHashedPassword(string hashedPassword, string password);
    }
}