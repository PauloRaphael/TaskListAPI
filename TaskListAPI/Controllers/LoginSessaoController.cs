using Microsoft.AspNetCore.Mvc;
using TaskListAPI.Model.Entities;
using TaskListAPI.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginSessaoController : ControllerBase
    {
        private readonly IGenericRepository<LoginSessao> _repository;

        public LoginSessaoController(IGenericRepository<LoginSessao> repository)
        {
            _repository = repository;
        }

        // GET: api/LoginSessao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoginSessao>>> GetLoginSessoes()
        {
            return Ok(await _repository.GetAllAsync());
        }

        // GET: api/LoginSessao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoginSessao>> GetLoginSessao(int id)
        {
            var sessao = await _repository.GetByIdAsync(id);

            if (sessao == null)
            {
                return NotFound();
            }

            return Ok(sessao);
        }

        // POST: api/LoginSessao
        // NOTA: Em uma API de autenticação, este método seria ativado por um POST em /api/Login/ ou /api/Auth/
        [HttpPost]
        public async Task<ActionResult<LoginSessao>> PostLoginSessao(LoginSessao sessao)
        {
            sessao.CriadoEm = System.DateTime.UtcNow;
            await _repository.AddAsync(sessao);
            return CreatedAtAction(nameof(GetLoginSessao), new { id = sessao.Id }, sessao);
        }

        // PUT: api/LoginSessao/5
        // Normalmente usado para estender o tempo de expiração do token.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoginSessao(int id, LoginSessao sessao)
        {
            if (id != sessao.Id)
            {
                return BadRequest("O ID na URL não corresponde ao ID no corpo da Sessão.");
            }

            var existingSessao = await _repository.GetByIdAsync(id);
            if (existingSessao == null)
            {
                return NotFound();
            }

            // Apenas permite a atualização do ExpiraEm
            existingSessao.ExpiraEm = sessao.ExpiraEm;

            await _repository.UpdateAsync(existingSessao);
            return NoContent();
        }

        // DELETE: api/LoginSessao/5
        // Usado para fazer logout ou invalidar o token.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoginSessao(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}