using System;
using System.ComponentModel.DataAnnotations;

namespace MercanciasJyCAPI.Models
{
    public class Entrega
    {
        public int EntregaID { get; set; }

        [Required(ErrorMessage = "El ID del pedido es obligatorio")]
        public int PedidoID { get; set; }

        public DateTime? FechaEntregaReal { get; set; }

        [Required(ErrorMessage = "El estado de la entrega es obligatorio")]
        [StringLength(20, ErrorMessage = "El estado de la entrega no puede exceder los 20 caracteres")]
        public string EstadoEntrega { get; set; }

        public virtual Pedido Pedido { get; set; }
    }
}