using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MercanciasJyCAPI.Models
{
    public class DetallePedido
    {
        [Key]
        public int DetalleID { get; set; }

        [Required]
        public int PedidoID { get; set; }

        [Required]
        public int ProductoID { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        [ForeignKey("PedidoID")]
        public virtual Pedido Pedido { get; set; }

        [ForeignKey("ProductoID")]
        public virtual Producto Producto { get; set; }
    }
}