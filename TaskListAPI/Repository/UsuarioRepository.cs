using TaskListAPI.Model.Entities;
using TaskListAPI.Data;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Threading.Tasks;

namespace TaskListAPI.Repository
{
    // Assuming GenericRepository<T> has a TaskListDbContext _context and DbSet<T> _dbSet
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(TaskListDbContext context) : base(context) { }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            // Note: Include .AsNoTracking() if you're not planning to update the user immediately after fetching
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUserWithHashedPasswordAsync(Usuario user)
        {
            // Criptografa a senha antes de salvar
            // Assuming 'user.Senha' holds the plain-text password on input
            user.Senha = BCrypt.Net.BCrypt.HashPassword(user.Senha);
            user.CriadoEm = System.DateTime.UtcNow;
            await _dbSet.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        // ** (COMPLETION) **
        // Implementation of the secure password verification method.
        public bool VerifyPassword(Usuario user, string providedPassword)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Senha))
            {
                // Cannot verify if user is null or if the hash is missing
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(providedPassword, user.Senha);
        }

        public async override Task UpdateAsync(Usuario user)
        {
            var existingUser = await GetByIdAsync(user.Id);

            if (existingUser != null)
            {
                existingUser.Nome = user.Nome;
                existingUser.Email = user.Email;
                // NÃO atualiza a senha aqui (this ensures security)

                // Ensure the entry is tracked for changes
                _dbSet.Update(existingUser);
                await _context.SaveChangesAsync();
            }
            // Note: Consider throwing an exception or returning a boolean if user is not found
        }
    }
}