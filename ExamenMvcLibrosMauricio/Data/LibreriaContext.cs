using ExamenMvcLibrosMauricio.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamenMvcLibrosMauricio.Data
{
    public class LibreriaContext : DbContext
    {
        public LibreriaContext(DbContextOptions<LibreriaContext> options) : base(options)
        {
        }

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<PedidoView> PedidosView { get; set; }
    }
}
