using System.ComponentModel.DataAnnotations;

namespace PlatUni.Utils
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var email = value as string;
            if (email == null)
            {
                return new ValidationResult("El correo electrónico es requerido.");
            }
            
            // Obtener el DbContext desde el ValidationContext
            var dbContext = (PlatUniContext)validationContext.GetService(typeof(PlatUniContext));
            
            // Verificar si el correo electrónico ya está en uso
            var existingStudent = dbContext.Students.FirstOrDefault(s => s.Email == email);
            if (existingStudent != null)
            {
                return new ValidationResult("Este correo electrónico ya está en uso.");
            }
            
            return ValidationResult.Success;
        }
    }
}
