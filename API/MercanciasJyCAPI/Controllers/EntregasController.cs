using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MercanciasJyCAPI.Data;
using MercanciasJyCAPI.Models;

namespace MercanciasJyCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntregasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EntregasController> _logger;

        public EntregasController(AppDbContext context, ILogger<EntregasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Entregas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entrega>>> GetEntregas()
        {
            _logger.LogInformation("Obteniendo todas las entregas");
            var entregas = await _context.Entregas.Include(e => e.Pedido).ToListAsync();
            _logger.LogInformation($"Se obtuvieron {entregas.Count} entregas");
            return entregas;
        }

        // GET: api/Entregas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Entrega>> GetEntrega(int id)
        {
            _logger.LogInformation($"Obteniendo entrega con ID: {id}");
            var entrega = await _context.Entregas
                .Include(e => e.Pedido)
                .FirstOrDefaultAsync(e => e.EntregaID == id);

            if (entrega == null)
            {
                _logger.LogWarning($"Entrega con ID: {id} no encontrada");
                return NotFound();
            }

            return entrega;
        }

        // POST: api/Entregas
        [HttpPost]
        public async Task<ActionResult<Entrega>> PostEntrega(Entrega entrega)
        {
            _logger.LogInformation("Creando nueva entrega");
            
            // Verificar si el pedido existe
            var pedidoExiste = await _context.Pedidos.AnyAsync(p => p.PedidoID == entrega.PedidoID);
            if (!pedidoExiste)
            {
                _logger.LogWarning($"Intento de crear entrega para un pedido inexistente: {entrega.PedidoID}");
                return BadRequest("El pedido especificado no existe.");
            }

            _context.Entregas.Add(entrega);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Entrega creada exitosamente con ID: {entrega.EntregaID}");
            return CreatedAtAction("GetEntrega", new { id = entrega.EntregaID }, entrega);
        }

        // PUT: api/Entregas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntrega(int id, Entrega entrega)
        {
            if (id != entrega.EntregaID)
            {
                _logger.LogWarning($"ID {id} no coincide con el ID de la entrega");
                return BadRequest();
            }

            _context.Entry(entrega).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Entrega con ID: {id} actualizada exitosamente");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntregaExists(id))
                {
                    _logger.LogWarning($"Entrega con ID: {id} no encontrada al intentar actualizar");
                    return NotFound();
                }
                else
                {
                    _logger.LogError($"Error de concurrencia al actualizar entrega con ID: {id}");
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Entregas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntrega(int id)
        {
            _logger.LogInformation($"Intentando eliminar entrega con ID: {id}");
            var entrega = await _context.Entregas.FindAsync(id);
            if (entrega == null)
            {
                _logger.LogWarning($"Entrega con ID: {id} no encontrada al intentar eliminar");
                return NotFound();
            }

            _context.Entregas.Remove(entrega);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Entrega con ID: {id} eliminada exitosamente");
            return NoContent();
        }

        // GET: api/Entregas/Pedido/5
        [HttpGet("Pedido/{pedidoId}")]
        public async Task<ActionResult<IEnumerable<Entrega>>> GetEntregasPorPedido(int pedidoId)
        {
            _logger.LogInformation($"Obteniendo entregas para el pedido con ID: {pedidoId}");
            var entregas = await _context.Entregas
                .Where(e => e.PedidoID == pedidoId)
                .ToListAsync();

            if (!entregas.Any())
            {
                _logger.LogWarning($"No se encontraron entregas para el pedido con ID: {pedidoId}");
                return NotFound($"No se encontraron entregas para el pedido con ID: {pedidoId}");
            }

            _logger.LogInformation($"Se encontraron {entregas.Count} entregas para el pedido con ID: {pedidoId}");
            return entregas;
        }

        // PUT: api/Entregas/5/CompletarEntrega
        [HttpPut("{id}/CompletarEntrega")]
        public async Task<IActionResult> CompletarEntrega(int id)
        {
            _logger.LogInformation($"Intentando completar entrega con ID: {id}");
            var entrega = await _context.Entregas.FindAsync(id);
            
            if (entrega == null)
            {
                _logger.LogWarning($"Entrega con ID: {id} no encontrada al intentar completar");
                return NotFound();
            }

            entrega.FechaEntregaReal = DateTime.Now;
            entrega.EstadoEntrega = "Completada";

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Entrega con ID: {id} completada exitosamente");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntregaExists(id))
                {
                    _logger.LogWarning($"Entrega con ID: {id} no encontrada al intentar completar (error de concurrencia)");
                    return NotFound();
                }
                else
                {
                    _logger.LogError($"Error de concurrencia al completar entrega con ID: {id}");
                    throw;
                }
            }

            return NoContent();
        }

        private bool EntregaExists(int id)
        {
            return _context.Entregas.Any(e => e.EntregaID == id);
        }
    }
}