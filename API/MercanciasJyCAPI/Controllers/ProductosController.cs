using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MercanciasJyCAPI.Data;
using MercanciasJyCAPI.Models;

namespace MercanciasJyCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(AppDbContext context, ILogger<ProductosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            _logger.LogInformation("Obteniendo todos los productos");
            return await _context.Productos.ToListAsync();
        }

        // GET: api/Productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            _logger.LogInformation($"Obteniendo producto con ID: {id}");
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                _logger.LogWarning($"Producto con ID: {id} no encontrado");
                return NotFound();
            }

            return producto;
        }

        // POST: api/Productos
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            _logger.LogInformation("Creando nuevo producto");
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Producto creado con ID: {producto.ProductoID}");
            return CreatedAtAction(nameof(GetProducto), new { id = producto.ProductoID }, producto);
        }

        // PUT: api/Productos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.ProductoID)
            {
                _logger.LogWarning($"ID {id} no coincide con el ID del producto");
                return BadRequest();
            }

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Producto con ID: {id} actualizado exitosamente");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
                {
                    _logger.LogWarning($"Producto con ID: {id} no encontrado al intentar actualizar");
                    return NotFound();
                }
                else
                {
                    _logger.LogError($"Error de concurrencia al actualizar producto con ID: {id}");
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Productos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            _logger.LogInformation($"Intentando eliminar producto con ID: {id}");
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                _logger.LogWarning($"Producto con ID: {id} no encontrado al intentar eliminar");
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Producto con ID: {id} eliminado exitosamente");
            return NoContent();
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.ProductoID == id);
        }
    }
}