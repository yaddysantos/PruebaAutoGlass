namespace AutoGlassBack.DTO
{
    public class ProductoDTO
    {
        public int Codigo_producto { get; set; }
        public string? Descripcion_producto { get; set; }
        public string? Estado_producto { get; set; }
        public DateTime Fecha_fabrica { get; set; }
        public DateTime Fecha_valida { get; set; }
        public int Codigo_proveedor { get; set; }
        public string? Descripcion_proveedor { get; set; }
        public string? Telefono_proveedor { get; set; }
    }
}
