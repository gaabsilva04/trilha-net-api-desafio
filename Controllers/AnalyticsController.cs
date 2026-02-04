using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public AnalyticsController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("status-counts")]
        public async Task<IActionResult> GetStatusCounts()
        {
            var counts = await _context.Tarefas.GroupBy(t => t.Status)
                .Select(g => new { status = g.Key, count = g.Count() })
                .ToListAsync();
            return Ok(counts);
        }

        [HttpGet("tasks-per-day")]
        public async Task<IActionResult> GetTasksPerDay([FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            var query = _context.Tarefas.AsQueryable();
            if (start.HasValue) query = query.Where(t => t.Data.Date >= start.Value.Date);
            if (end.HasValue) query = query.Where(t => t.Data.Date <= end.Value.Date);

            var result = await query.GroupBy(t => t.Data.Date)
                .Select(g => new { date = g.Key, count = g.Count() })
                .OrderBy(r => r.date)
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var total = await _context.Tarefas.CountAsync();
            var byStatus = await _context.Tarefas.GroupBy(t => t.Status).Select(g => new { status = g.Key, count = g.Count() }).ToListAsync();
            var last30 = DateTime.UtcNow.Date.AddDays(-30);
            var recent = await _context.Tarefas.Where(t => t.Data.Date >= last30).CountAsync();
            return Ok(new { total, byStatus, recentLast30Days = recent });
        }
    }
}




































