using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MercanciasJyCAPI.Data;
using MercanciasJyCAPI.Models;

namespace MercanciasJyCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(AppDbContext context, ILogger<ClientesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            _logger.LogInformation("Obteniendo todos los clientes");
            return await _context.Clientes.ToListAsync();
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            _logger.LogInformation($"Obteniendo cliente con ID: {id}");
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                _logger.LogWarning($"Cliente con ID: {id} no encontrado");
                return NotFound();
            }

            return cliente;
        }

        // POST: api/Clientes
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            _logger.LogInformation("Creando nuevo cliente");
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Cliente creado con ID: {cliente.ClienteID}");
            return CreatedAtAction(nameof(GetCliente), new { id = cliente.ClienteID }, cliente);
        }

        // PUT: api/Clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.ClienteID)
            {
                _logger.LogWarning($"ID {id} no coincide con el ID del cliente");
                return BadRequest();
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Cliente con ID: {id} actualizado exitosamente");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                {
                    _logger.LogWarning($"Cliente con ID: {id} no encontrado al intentar actualizar");
                    return NotFound();
                }
                else
                {
                    _logger.LogError($"Error de concurrencia al actualizar cliente con ID: {id}");
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            _logger.LogInformation($"Intentando eliminar cliente con ID: {id}");
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                _logger.LogWarning($"Cliente con ID: {id} no encontrado al intentar eliminar");
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Cliente con ID: {id} eliminado exitosamente");
            return NoContent();
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.ClienteID == id);
        }
    }
}