using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MercanciasJyCAPI.Data;
using MercanciasJyCAPI.Models;

namespace MercanciasJyCAPI.Services
{
    public class PedidoService
    {
        private readonly AppDbContext _context;

        public PedidoService(AppDbContext context)
        {
            _context = context;
        }

        public bool ValidarFechaEntrega(DateTime fechaPedido, DateTime fechaEntregaProgramada)
        {
            var diaSemana = fechaEntregaProgramada.DayOfWeek;
            var horaPedido = fechaPedido.TimeOfDay;

            switch (diaSemana)
            {
                case DayOfWeek.Monday:
                    return fechaPedido >= fechaEntregaProgramada.AddDays(-3).Date.AddHours(12) &&
                           fechaPedido <= fechaEntregaProgramada.AddDays(-2).Date.AddHours(12);
                case DayOfWeek.Tuesday:
                    return fechaPedido >= fechaEntregaProgramada.AddDays(-3).Date.AddHours(12) &&
                           fechaPedido <= fechaEntregaProgramada.AddDays(-2).Date.AddHours(12);
                case DayOfWeek.Wednesday:
                    return fechaPedido >= fechaEntregaProgramada.AddDays(-3).Date.AddHours(12) &&
                           fechaPedido <= fechaEntregaProgramada.AddDays(-2).Date.AddHours(12);
                case DayOfWeek.Thursday:
                    return fechaPedido >= fechaEntregaProgramada.AddDays(-3).Date.AddHours(12) &&
                           fechaPedido <= fechaEntregaProgramada.AddDays(-2).Date.AddHours(12);
                case DayOfWeek.Friday:
                    return fechaPedido >= fechaEntregaProgramada.AddDays(-3).Date.AddHours(12) &&
                           fechaPedido <= fechaEntregaProgramada.AddDays(-2).Date.AddHours(12);
                case DayOfWeek.Saturday:
                    return fechaPedido >= fechaEntregaProgramada.AddDays(-3).Date.AddHours(12) &&
                           fechaPedido <= fechaEntregaProgramada.AddDays(-2).Date.AddHours(12);
                case DayOfWeek.Sunday:
                    return false; // No se realizan entregas los domingos
                default:
                    return false;
            }
        }

        public async Task<IEnumerable<Pedido>> GetPedidosAsync()
        {
            return await _context.Pedidos.ToListAsync();
        }

        public async Task<Pedido> GetPedidoAsync(int id)
        {
            return await _context.Pedidos.FindAsync(id);
        }

        public async Task<int> CreatePedidoAsync(Pedido pedido)
        {
            var clienteIdParam = new SqlParameter("@ClienteID", SqlDbType.Int) { Value = pedido.ClienteID };
            var fechaPedidoParam = new SqlParameter("@FechaPedido", SqlDbType.DateTime) { Value = pedido.FechaPedido };
            var fechaEntregaProgramadaParam = new SqlParameter("@FechaEntregaProgramada", SqlDbType.Date) { Value = pedido.FechaEntregaProgramada };
            var pedidoIdParam = new SqlParameter
            {
                ParameterName = "@PedidoID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC sp_CrearPedido @ClienteID, @FechaPedido, @FechaEntregaProgramada, @PedidoID OUTPUT",
                clienteIdParam, fechaPedidoParam, fechaEntregaProgramadaParam, pedidoIdParam);

            return (int)pedidoIdParam.Value;
        }

        public async Task AgregarDetallePedidoAsync(int pedidoId, int productoId, int cantidad)
        {
            var pedidoIdParam = new SqlParameter("@PedidoID", SqlDbType.Int) { Value = pedidoId };
            var productoIdParam = new SqlParameter("@ProductoID", SqlDbType.Int) { Value = productoId };
            var cantidadParam = new SqlParameter("@Cantidad", SqlDbType.Int) { Value = cantidad };

            await _context.Database.ExecuteSqlRawAsync("EXEC sp_AgregarDetallePedido @PedidoID, @ProductoID, @Cantidad",
                pedidoIdParam, productoIdParam, cantidadParam);
        }

        public async Task ProgramarEntregaAsync(int pedidoId)
        {
            var pedidoIdParam = new SqlParameter("@PedidoID", SqlDbType.Int) { Value = pedidoId };

            await _context.Database.ExecuteSqlRawAsync("EXEC sp_ProgramarEntrega @PedidoID", pedidoIdParam);
        }
    }
}