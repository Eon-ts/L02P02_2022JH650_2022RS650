using Microsoft.AspNetCore.Mvc;
using L02P02_2022JH650_2022RS650.Models;
using System.Collections.Immutable;

namespace L02P02_2022JH650_2022RS650.Controllers
{
    public class VentaController : Controller
    {
        private readonly libreriaDbContext _libreriaDBcontext;
        public VentaController(libreriaDbContext libreriaDBcontext)
        {
            _libreriaDBcontext = libreriaDBcontext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CrearCliente(clientes cliente)
        {
            var ultimoId = _libreriaDBcontext.clientes.Max(c => (int?)c.id) ?? 0;
            cliente.id = ultimoId + 1;
            cliente.created_at = DateTime.Now;
            _libreriaDBcontext.Add(cliente);
            _libreriaDBcontext.SaveChanges();

            // Crear el encabezado del pedido
            pedido_encabezado encabezado = new pedido_encabezado();
            encabezado.id_cliente = cliente.id;
            var ultimoIdencabezado = _libreriaDBcontext.pedido_encabezado.Max(p => (int?)p.id) ?? 0;
            encabezado.id = ultimoIdencabezado + 1;
            encabezado.cantidad_libros = 0;
            encabezado.total = 0;
            encabezado.estado = "P";
            _libreriaDBcontext.Add(encabezado);
            _libreriaDBcontext.SaveChanges();

            return RedirectToAction("ListadoLibros");
        }

        public IActionResult ListadoLibros()
        {
            var libros = _libreriaDBcontext.libros.ToList();
            ViewBag.Libros = libros;

            var pedidoEncabezado = _libreriaDBcontext.pedido_encabezado
                .Where(p => p.estado == "P") 
                .OrderByDescending(p => p.id) // Obtener el último pedido abierto con estado "P"
                .FirstOrDefault();

            if (pedidoEncabezado != null)
            {
                var carrito = (from pd in _libreriaDBcontext.pedido_detalle
                               join libro in _libreriaDBcontext.libros on pd.id_libro equals libro.id
                               where pd.id_pedido == pedidoEncabezado.id
                               select new
                               {
                                   libro_nombre = libro.nombre,
                                   precio = libro.precio
                               }).ToList();

                ViewBag.Carrito = carrito;
                ViewBag.Total = pedidoEncabezado.total;
                ViewBag.id2 = pedidoEncabezado.id;



            }

            return View();
        }


        [HttpPost]
        public IActionResult AgregarAlCarrito(int libroId)
        {
            var pedidoEncabezado = _libreriaDBcontext.pedido_encabezado
                .Where(p => p.estado == "P")
                .OrderByDescending(p => p.id) // Último pedido creado (El que acabamos de crear, que tiene estado "P")
                .FirstOrDefault();

            if (pedidoEncabezado == null)
            {
                return RedirectToAction("Index");
            }
            
            // Obtener el libro
            var libro = _libreriaDBcontext.libros.FirstOrDefault(l => l.id == libroId);
            if (libro == null || libro.estado == 'O') // Comprobación sobre la existencia de libro y estado (Que no esté ocupado)
            {
                return RedirectToAction("ListadoLibros");
            }

            // Cambiar el estado del libro a "O" (ocupado)
            libro.estado = 'O';
            _libreriaDBcontext.libros.Update(libro);

            // Crear el detalle del pedido
            var pedidoDetalle = new pedido_detalle
            {
                id = _libreriaDBcontext.pedido_detalle.Max(pd => (int?)pd.id) + 1 ?? 1,
                id_pedido = pedidoEncabezado.id,
                id_libro = libro.id,
                created_at = DateTime.Now
            };

            // Actualizar cantidad y total en el encabezado del pedido
            pedidoEncabezado.cantidad_libros += 1;
            pedidoEncabezado.total += libro.precio;

            _libreriaDBcontext.pedido_detalle.Add(pedidoDetalle);
            _libreriaDBcontext.pedido_encabezado.Update(pedidoEncabezado);
            _libreriaDBcontext.SaveChanges();

            return RedirectToAction("ListadoLibros");
        }
        // prototipo 03
        public IActionResult cerrar(int id)
        {

            //Se pasa un id de pedido_encabezado para encontrar el cliente
            var pedidoEncabezado = (from p in _libreriaDBcontext.pedido_encabezado
                                    join c in _libreriaDBcontext.clientes on p.id_cliente equals c.id
                                    where p.id == id
                                    select c).FirstOrDefault();


            ViewData["pedidoEncabezado"] = pedidoEncabezado;

            var pedidoDetalle = (from p in _libreriaDBcontext.pedido_detalle
                                 join l in _libreriaDBcontext.libros on p.id_libro equals l.id
                                 join a in _libreriaDBcontext.autores on l.id_autor equals a.id
                                 where p.id_pedido == id
                                 select new
                                 {
                                     Idpedido = p.id_pedido,
                                     Id = p.id,
                                     titulo = l.nombre,
                                     precio = l.precio,
                                     autor = a.autor
                                 }).ToList();
            ViewData["pedidoDetalle"] = pedidoDetalle;


            var total = (from p in _libreriaDBcontext.pedido_detalle
                         join pe in _libreriaDBcontext.pedido_encabezado on p.id_libro equals pe.id
                         where p.id_pedido == id
                         select pe.total).FirstOrDefault();
            ViewBag.total = total;


            var ultimoId = _libreriaDBcontext.pedido_encabezado.Max(c => (int?)c.id) ?? 0;
            return View();
        }
        public IActionResult actualizarencabeezado(int id)
        {
            var pedidoEncabezado = (from p in _libreriaDBcontext.pedido_encabezado
                                    join c in _libreriaDBcontext.clientes on p.id equals c.id
                                    where p.id == id
                                    select c).FirstOrDefault();
            ViewData["pedidoEncabezado"] = pedidoEncabezado;

            return View();
        }

    }
}