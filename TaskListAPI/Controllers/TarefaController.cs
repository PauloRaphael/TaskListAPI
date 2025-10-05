using Microsoft.AspNetCore.Mvc;
using TaskListAPI.Model.Entities;
using TaskListAPI.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly IGenericRepository<Tarefa> _repository;

        public TarefaController(IGenericRepository<Tarefa> repository)
        {
            _repository = repository;
        }

        // GET: api/Tarefa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetTarefas()
        {
            return Ok(await _repository.GetAllAsync());
        }

        // GET: api/Tarefa/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tarefa>> GetTarefa(int id)
        {
            var tarefa = await _repository.GetByIdAsync(id);

            if (tarefa == null)
            {
                return NotFound();
            }

            return Ok(tarefa);
        }

        // POST: api/Tarefa
        [HttpPost]
        public async Task<ActionResult<Tarefa>> PostTarefa(Tarefa tarefa)
        {
            tarefa.CriadoEm = System.DateTime.UtcNow;
            // Garante que o status padrão seja Pendente ao criar.
            if (!System.Enum.IsDefined(typeof(StatusEnum), tarefa.Status))
            {
                tarefa.Status = StatusEnum.Pendente;
            }

            await _repository.AddAsync(tarefa);
            return CreatedAtAction(nameof(GetTarefa), new { id = tarefa.Id }, tarefa);
        }

        // PUT: api/Tarefa/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarefa(int id, Tarefa tarefa)
        {
            if (id != tarefa.Id)
            {
                return BadRequest("O ID na URL não corresponde ao ID no corpo da Tarefa.");
            }

            var existingTarefa = await _repository.GetByIdAsync(id);
            if (existingTarefa == null)
            {
                return NotFound();
            }

            // Atualiza os campos editáveis
            existingTarefa.UsuarioId = tarefa.UsuarioId;
            existingTarefa.CategoriaId = tarefa.CategoriaId;
            existingTarefa.Titulo = tarefa.Titulo;
            existingTarefa.Descricao = tarefa.Descricao;
            existingTarefa.DataVencimento = tarefa.DataVencimento;
            existingTarefa.HoraVencimento = tarefa.HoraVencimento;
            existingTarefa.Status = tarefa.Status;

            await _repository.UpdateAsync(existingTarefa);
            return NoContent();
        }

        // DELETE: api/Tarefa/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarefa(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}