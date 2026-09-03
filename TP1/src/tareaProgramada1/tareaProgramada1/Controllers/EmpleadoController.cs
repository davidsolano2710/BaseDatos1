using Microsoft.AspNetCore.Mvc;
using tareaProgramada1.Services;
using tareaProgramada1.Models;

namespace tareaProgramada1.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly EmpleadoService _empleadoService;

        public EmpleadoController(EmpleadoService empleadoService)
        {
            _empleadoService = empleadoService;
        }
        public IActionResult Index()
        {
            var empleados = _empleadoService.ListarEmpleados();
            return View(empleados);                                     //manda empleados a la vista
        }
    }
}
