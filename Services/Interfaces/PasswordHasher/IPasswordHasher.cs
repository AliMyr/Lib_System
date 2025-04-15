namespace Lib_System.Services.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string input);
    }
}
