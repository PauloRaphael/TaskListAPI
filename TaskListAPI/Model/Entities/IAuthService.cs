using TaskListAPI.Model.Entities;

namespace TaskListAPI.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(Usuario user);
        // bool VerifyPassword(string providedPassword, string storedHash); // Optionally put here
    }
}