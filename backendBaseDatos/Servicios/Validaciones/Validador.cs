using backendBaseDatos.Models;
using System.Text.RegularExpressions;

namespace backendBaseDatos.Servicios.Validaciones
{
    public static class Validador
    {
        public static bool IsValidEmail(string email)
        {
            // Define a regular expression for a simple email validation
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Create a Regex object
            Regex regex = new Regex(pattern);

            // Use the IsMatch method to validate the email
            return regex.IsMatch(email);
        }
        /// <summary>
        /// Chequea que los campos del objeto funcionario que es requerido en la base de datos no sea nulo.
        /// </summary>
        /// <param name="funcionarios"></param>
        /// <returns></returns>
        public static ValidateStatus ValidarFuncionario(Funcionarios funcionarios)
        {
            ValidateStatus estado = new();
            if(funcionarios == null) { return new(false,"El funcionario esta vacio."); }
            if(string.IsNullOrEmpty(funcionarios.Ci)) { return new(false,"La cedula del funcionario es requerida"); }
            if(!CI_Validate.IsCIValid(funcionarios.Ci)) { return new(false,"La cedula del funcionario no es valida, no coincide el digito verificador."); }
            if(string.IsNullOrEmpty(funcionarios.Email)) { return new(false,"El e-mail es requerido."); }
            if(IsValidEmail(funcionarios.Email)== false) { return new(false, "El e-mail no es valido."); }
            if(string.IsNullOrEmpty (funcionarios.Password)) { return new(false, "La contraseña esta vacia."); }
            //el Log id no lo valido xq es autogenerado
            return estado;
        }
    }
}
