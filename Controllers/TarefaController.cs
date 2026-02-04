using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null) return NotFound();
            return Ok(tarefa);
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var lista = await _context.Tarefas.ToListAsync();
            return Ok(lista);
        }

        [HttpGet("search")]
        public async Task<IActionResult> ObterPorTitulo([FromQuery] string titulo)
        {
            var tarefas = await _context.Tarefas.Where(x => x.Titulo.Contains(titulo)).ToListAsync();
            return Ok(tarefas);
        }

        [HttpGet("bydate")]
        public async Task<IActionResult> ObterPorData([FromQuery] DateTime data)
        {
            var tarefa = await _context.Tarefas.Where(x => x.Data.Date == data.Date).ToListAsync();
            return Ok(tarefa);
        }

        [HttpGet("bystatus")]
        public async Task<IActionResult> ObterPorStatus([FromQuery] EnumStatusTarefa status)
        {
            var tarefa = await _context.Tarefas.Where(x => x.Status == status).ToListAsync();
            return Ok(tarefa);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            await _context.Tarefas.AddAsync(tarefa);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Tarefa tarefa)
        {
            var tarefaBanco = await _context.Tarefas.FindAsync(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            _context.Tarefas.Update(tarefaBanco);
            await _context.SaveChangesAsync();
            return Ok(tarefaBanco);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var tarefaBanco = await _context.Tarefas.FindAsync(id);

            if (tarefaBanco == null)
                return NotFound();

            _context.Tarefas.Remove(tarefaBanco);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
