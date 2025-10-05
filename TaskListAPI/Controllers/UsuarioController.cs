using Microsoft.AspNetCore.Mvc;
using TaskListAPI.Model.Entities;
using TaskListAPI.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioController(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return Ok(await _repository.GetAllAsync());
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // POST: api/Usuario
        // A senha será criptografada dentro do repositório
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            await _repository.AddUserWithHashedPasswordAsync(usuario);
            // Retorna 201 Created e o usuário, excluindo a senha
            usuario.Senha = null;
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        // PUT: api/Usuario/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            var existingUser = await _repository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            // Apenas permite a atualização de Nome e Email neste endpoint
            existingUser.Nome = usuario.Nome;
            existingUser.Email = usuario.Email;
            // Não atualiza Senha e CriadoEm aqui.

            await _repository.UpdateAsync(existingUser);
            return NoContent();
        }

        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}