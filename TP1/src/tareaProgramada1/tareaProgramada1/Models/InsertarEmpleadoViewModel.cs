using System.ComponentModel.DataAnnotations;

namespace tareaProgramada1.Models
{
    // Modelo de presentación: solo campos obligatorios y formatos.
    public class InsertarEmpleadoViewModel
    {
        [Required(ErrorMessage = "Escriba el nombre del empleado.")]
        [RegularExpression(@"(?=.*[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ])[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ \-]+",
            ErrorMessage = "Use solamente letras, espacios y guiones; incluya al menos una letra.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Escriba el salario.")]
        [RegularExpression(@"[0-9]+(\.[0-9]{1,4})?",
            ErrorMessage = "Use dígitos y un punto opcional, con hasta cuatro decimales.")]
        public string Salario { get; set; } = string.Empty;
    }
}

