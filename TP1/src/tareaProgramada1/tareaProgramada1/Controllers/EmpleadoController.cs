using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using tareaProgramada1.Services;
using tareaProgramada1.Models;

namespace tareaProgramada1.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly EmpleadoService _empleadoService;
        private readonly ILogger<EmpleadoController> _logger;

        public EmpleadoController(
            EmpleadoService empleadoService,
            ILogger<EmpleadoController> logger)
        {
            _empleadoService = empleadoService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_empleadoService.ListarEmpleados());
        }

        [HttpGet]
        public IActionResult Insertar()
        {
            return View(new InsertarEmpleadoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Insertar(InsertarEmpleadoViewModel model)
        {
            // Validación de presentación: campos obligatorios y formato.
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                int codigo = _empleadoService.InsertarEmpleado(model.Nombre, model.Salario);

                switch (codigo)
                {
                    case 0:
                        // Redirigir evita repetir el POST al actualizar el navegador.
                        TempData["Exito"] = "Inserción exitosa.";
                        return RedirectToAction(nameof(Index));
                    case 1:
                        ModelState.AddModelError(nameof(model.Nombre),
                            "Nombre de Empleado ya existe.");
                        break;
                    case 2:
                        ModelState.AddModelError(string.Empty,
                            "La base de datos rechazó los datos. Revise el nombre (máximo 128 caracteres) y que el salario esté dentro del rango MONEY.");
                        break;
                    default:
                        _logger.LogError("El SP de inserción devolvió el código {Codigo}.", codigo);
                        ModelState.AddModelError(string.Empty,
                            "No se pudo completar la inserción. Revise la base de datos.");
                        break;
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error de SQL al insertar empleado.");
                ModelState.AddModelError(string.Empty,
                    "No se pudo confirmar la inserción. Revise el listado antes de volver a intentarlo.");
            }

            return View(model);
        }
    }
}

