using TaskListAPI.Model.Entities;
using System.Threading.Tasks;

namespace TaskListAPI.Repository
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        // Fetches a user by their unique email for login
        Task<Usuario> GetByEmailAsync(string email);

        // Handles the insertion of a new user, including password hashing
        Task AddUserWithHashedPasswordAsync(Usuario user);

        // ** (COMPLETION) **
        // Method to securely verify the plain-text password against the stored hash.
        bool VerifyPassword(Usuario user, string providedPassword);
    }
}