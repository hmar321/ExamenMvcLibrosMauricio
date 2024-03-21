using ExamenMvcLibrosMauricio.Data;
using ExamenMvcLibrosMauricio.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamenMvcLibrosMauricio.Repositories
{
    public class RepositoryLibros
    {
        private LibreriaContext context;

        public RepositoryLibros(LibreriaContext context)
        {
            this.context = context;
        }

        public async Task<List<Libro>> GetAllLibrosAsync()
        {
            return await this.context.Libros.ToListAsync();
        }
        public async Task<List<Libro>> GetAllLibrosByGeneroAsync(int idgenero)
        {
            return await this.context.Libros.Where(l => l.IdGenero == idgenero).ToListAsync();
        }
        public async Task<List<Libro>> GetAllLibrosByListIdsAsync(List<int> idslibro)
        {
            return await this.context.Libros.Where(l => idslibro.Contains(l.IdLibro)).ToListAsync();
        }
        public async Task<Libro> GetLibroByIdLibroAsync(int idlibro)
        {
            return await this.context.Libros.FirstOrDefaultAsync(l => l.IdLibro == idlibro);
        }

        public async Task<List<Genero>> GetAllGenerosAsync()
        {
            return await this.context.Generos.ToListAsync();
        }

        public async Task<Usuario> LogInUserAsync(string email, string password)
        {
            return await this.context.Usuarios.FirstOrDefaultAsync(u => u.Email == email && u.Pass == password);
        }

        public int GetNextFacturaId()
        {
            return this.context.Pedidos.Max(p => p.IdFactura) + 1;
        }
        public int GetNextPedidoId()
        {
            return this.context.Pedidos.Max(p => p.IdPedido) + 1;
        }

        public async Task<int> InsertPedidoAsync(List<int> idslibro, int idusuario)
        {
            int idfactura = this.GetNextFacturaId();
            DateTime fecha = DateTime.Now;
            foreach (int idlibro in idslibro)
            {
                int idpedido = this.GetNextPedidoId();
                this.context.Add(new Pedido
                {
                    IdPedido = idpedido,
                    IdFactura = idfactura,
                    Fecha = fecha,
                    IdLibro = idlibro,
                    IdUsuario = idusuario,
                    Cantidad = 1
                });
            }
            return await this.context.SaveChangesAsync();
        }

        public async Task<List<PedidoView>> GetAllPedidoViewByIdUsuarioAsync(int idusuario)
        {
            return await this.context.PedidosView.Where(pv => pv.IdUsuario == idusuario).ToListAsync();
        }
    }
}
