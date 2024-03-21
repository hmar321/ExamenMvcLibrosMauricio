using ExamenMvcLibrosMauricio.Filters;
using ExamenMvcLibrosMauricio.Models;
using ExamenMvcLibrosMauricio.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ExamenMvcLibrosMauricio.Extensions;
using System.Security.Claims;

namespace ExamenMvcLibrosMauricio.Controllers
{
    public class LibroController : Controller
    {
        private RepositoryLibros repo;
        private IMemoryCache memoryCache;

        public LibroController(RepositoryLibros repo, IMemoryCache memoryCache)
        {
            this.repo = repo;
            this.memoryCache = memoryCache;
        }

        public async Task<IActionResult> Home()
        {
            List<Libro> libros = await this.repo.GetAllLibrosAsync();
            return View(libros);
        }

        public async Task<IActionResult> Genero(int idgenero)
        {
            List<Libro> libros = await this.repo.GetAllLibrosByGeneroAsync(idgenero);
            return View(libros);
        }

        public async Task<IActionResult> Detalles(int idlibro)
        {
            Libro libro = await this.repo.GetLibroByIdLibroAsync(idlibro);
            return View(libro);
        }

        public async Task<IActionResult> Comprar(int idlibro)
        {
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
            if (carrito == null)
            {
                carrito = new List<int>();
            }
            if (carrito.Any(id => id == idlibro) == false)
            {
                carrito.Add(idlibro);
                HttpContext.Session.SetObject("CARRITO", carrito);
            }

            return RedirectToAction("Carrito");
        }

        public async Task<IActionResult> Quitar(int idlibro)
        {
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
            if (carrito != null)
            {
                if (carrito.Any(id => id == idlibro) == true)
                {
                    int id = carrito.Find(id => id == idlibro);
                    carrito.Remove(id);
                    if (carrito.Count == 0)
                    {
                        HttpContext.Session.Remove("CARRITO");
                    }
                    else
                    {
                        HttpContext.Session.SetObject("CARRITO", carrito);
                    }
                }
            }
            return RedirectToAction("Carrito");
        }

        public async Task<IActionResult> Carrito()
        {
            List<int> idslibro = HttpContext.Session.GetObject<List<int>>("CARRITO");
            List<Libro> libros = null;
            if (idslibro != null)
            {
                libros = await this.repo.GetAllLibrosByListIdsAsync(idslibro);
            }
            return View(libros);
        }
        [AuthorizeUsuarios]
        public async Task<IActionResult> UsuarioPerfil()
        {
            return View();
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> FinalizarCompra()
        {
            List<int> idslibro = HttpContext.Session.GetObject<List<int>>("CARRITO");
            if (idslibro != null)
            {
                int idusuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await this.repo.InsertPedidoAsync(idslibro, idusuario);
                HttpContext.Session.Remove("CARRITO");
            }
            return RedirectToAction("Pedidos");
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> Pedidos()
        {
            int idusuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            List<PedidoView> pedidoViews = await this.repo.GetAllPedidoViewByIdUsuarioAsync(idusuario);
            return View(pedidoViews);
        }
    }
}
