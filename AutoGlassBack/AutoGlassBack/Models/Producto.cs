using System.ComponentModel.DataAnnotations;

namespace AutoGlassBack.Models
{
    public class Producto
    {
        [Key]
        public int Codigo_producto { get; set; }
        [Required]
        public string? Descripcion_producto { get; set; }
        [Required] 
        public string? Estado_producto { get; set; }
        [Required] 
        public DateTime Fecha_fabrica { get; set; }
        [Required] 
        public DateTime Fecha_valida { get; set; }
        [Required] 
        public int Codigo_proveedor { get; set; }
        [Required] 
        public string? Descripcion_proveedor { get; set; }
        [Required] 
        public string? Telefono_proveedor { get; set; }
        
    }
}
