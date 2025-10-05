using TaskListAPI.Model.Entities;
using System.Threading.Tasks;

namespace TaskListAPI.Repository
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<Usuario> GetByEmailAsync(string email);
        Task AddUserWithHashedPasswordAsync(Usuario user);
    }
}