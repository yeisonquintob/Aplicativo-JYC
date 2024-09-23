using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MercanciasJyCAPI.Models
{
    public class Pedido
    {
        public int PedidoID { get; set; }

        [Required(ErrorMessage = "El ID del cliente es obligatorio")]
        public int ClienteID { get; set; }

        [Required(ErrorMessage = "La fecha del pedido es obligatoria")]
        public DateTime FechaPedido { get; set; }

        [Required(ErrorMessage = "La fecha de entrega programada es obligatoria")]
        public DateTime FechaEntregaProgramada { get; set; }

        [Required(ErrorMessage = "El estado del pedido es obligatorio")]
        [StringLength(20, ErrorMessage = "El estado del pedido no puede exceder los 20 caracteres")]
        public string EstadoPedido { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<DetallePedido> DetallesPedido { get; set; }
    }
}