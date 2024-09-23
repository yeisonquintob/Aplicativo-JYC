using MercanciasJyCAPI.Data;
using MercanciasJyCAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MercanciasJyCAPI.Services
{
    public class EntregaService
    {
        private readonly AppDbContext _context;

        public EntregaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Entrega>> GetEntregasAsync()
        {
            return await _context.Entregas
                .Include(e => e.Pedido)
                .ThenInclude(p => p.Cliente)
                .ToListAsync();
        }

        public async Task<Entrega> GetEntregaAsync(int id)
        {
            return await _context.Entregas
                .Include(e => e.Pedido)
                .ThenInclude(p => p.Cliente)
                .FirstOrDefaultAsync(e => e.EntregaID == id);
        }

        public async Task<Entrega> CreateEntregaAsync(Entrega entrega)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(p => p.PedidoID == entrega.PedidoID);

            if (pedido == null)
            {
                throw new ArgumentException("El pedido especificado no existe.");
            }

            entrega.Pedido = pedido;
            _context.Entregas.Add(entrega);
            await _context.SaveChangesAsync();
            return entrega;
        }

        public async Task UpdateEntregaAsync(Entrega entrega)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(p => p.PedidoID == entrega.PedidoID);

            if (pedido == null)
            {
                throw new ArgumentException("El pedido especificado no existe.");
            }

            entrega.Pedido = pedido;
            _context.Entry(entrega).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEntregaAsync(int id)
        {
            var entrega = await _context.Entregas.FindAsync(id);
            if (entrega != null)
            {
                _context.Entregas.Remove(entrega);
                await _context.SaveChangesAsync();
            }
        }
    }
}