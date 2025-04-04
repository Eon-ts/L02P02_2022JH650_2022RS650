using Microsoft.EntityFrameworkCore;
namespace L02P02_2022JH650_2022RS650.Models
{
    public class libreriaDbContext : DbContext
    {

        public libreriaDbContext(DbContextOptions<libreriaDbContext> options) : base(options)
        {
        }
        public DbSet<clientes> clientes { get; set; }
        public DbSet<autores> autores { get; set; }
        public DbSet<categorias> categorias { get; set; }
        public DbSet<libros> libros { get; set; }
        public DbSet<pedido_encabezado> pedido_encabezado { get; set; }
        public DbSet<pedido_detalle> pedido_detalle { get; set; }
        public DbSet<comentarios_libros> comentarios_libros { get; set; }
    }
}
