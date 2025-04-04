using Microsoft.AspNetCore.Mvc;
using L02P02_2022JH650_2022RS650.Models;

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

            return RedirectToAction("Index");
        }

    }
}
