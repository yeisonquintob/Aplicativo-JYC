using Microsoft.AspNetCore.Mvc;
using MercanciasJyCAPI.Models;
using MercanciasJyCAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MercanciasJyCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly PedidoService _pedidoService;
        private readonly ILogger<PedidosController> _logger;

        public PedidosController(PedidoService pedidoService, ILogger<PedidosController> logger)
        {
            _pedidoService = pedidoService;
            _logger = logger;
        }

        // GET: api/Pedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            _logger.LogInformation("Obteniendo todos los pedidos");
            var pedidos = await _pedidoService.GetPedidosAsync();
            return Ok(pedidos);
        }

        // GET: api/Pedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            _logger.LogInformation($"Obteniendo pedido con ID: {id}");
            var pedido = await _pedidoService.GetPedidoAsync(id);

            if (pedido == null)
            {
                _logger.LogWarning($"Pedido con ID: {id} no encontrado");
                return NotFound();
            }

            return pedido;
        }

        // POST: api/Pedidos
        [HttpPost]
        public async Task<ActionResult<int>> CreatePedido(Pedido pedido)
        {
            _logger.LogInformation("Creando nuevo pedido");
            try
            {
                if (!_pedidoService.ValidarFechaEntrega(pedido.FechaPedido, pedido.FechaEntregaProgramada))
                {
                    return BadRequest("La fecha de entrega programada no es válida.");
                }

                var pedidoId = await _pedidoService.CreatePedidoAsync(pedido);
                _logger.LogInformation($"Pedido creado exitosamente con ID: {pedidoId}");
                return CreatedAtAction(nameof(GetPedido), new { id = pedidoId }, pedidoId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear pedido: {ex.Message}");
                return StatusCode(500, "Error interno del servidor al crear el pedido.");
            }
        }

        // POST: api/Pedidos/{pedidoId}/DetallePedido
        [HttpPost("{pedidoId}/DetallePedido")]
        public async Task<IActionResult> AgregarDetallePedido(int pedidoId, [FromBody] DetallePedido detalle)
        {
            try
            {
                await _pedidoService.AgregarDetallePedidoAsync(pedidoId, detalle.ProductoID, detalle.Cantidad);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al agregar detalle al pedido {pedidoId}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor al agregar detalle al pedido.");
            }
        }

        // POST: api/Pedidos/{pedidoId}/ProgramarEntrega
        [HttpPost("{pedidoId}/ProgramarEntrega")]
        public async Task<IActionResult> ProgramarEntrega(int pedidoId)
        {
            try
            {
                await _pedidoService.ProgramarEntregaAsync(pedidoId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al programar entrega para el pedido {pedidoId}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor al programar la entrega.");
            }
        }
    }
}