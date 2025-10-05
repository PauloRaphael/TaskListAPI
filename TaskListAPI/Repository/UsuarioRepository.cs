using TaskListAPI.Model.Entities;
using TaskListAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BCrypt.Net;

namespace TaskListAPI.Repository
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(TaskListDbContext context) : base(context) { }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUserWithHashedPasswordAsync(Usuario user)
        {
            // Criptografa a senha antes de salvar
            user.Senha = BCrypt.Net.BCrypt.HashPassword(user.Senha);
            user.CriadoEm = System.DateTime.UtcNow;
            await _dbSet.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async override Task UpdateAsync(Usuario user)
        {
            // Opcionalmente, você pode lidar com a atualização da senha aqui, 
            // mas é melhor ter um método específico para isso.
            // Para este CRUD simples, apenas atualiza outros campos.
            var existingUser = await GetByIdAsync(user.Id);

            if (existingUser != null)
            {
                existingUser.Nome = user.Nome;
                existingUser.Email = user.Email;
                // NÃO atualiza a senha aqui
                _dbSet.Update(existingUser);
                await _context.SaveChangesAsync();
            }
        }
    }
}