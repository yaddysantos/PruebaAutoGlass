using AutoGlassBack.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoGlassBack
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {
        }

        ///Configuracion del DbSet para mapear nuestros modelos de la base de datos
        public DbSet<Producto> Producto { get; set; }
    }
}
