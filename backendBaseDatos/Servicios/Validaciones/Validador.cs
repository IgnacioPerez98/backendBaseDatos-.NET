using backendBaseDatos.Models;

namespace backendBaseDatos.Servicios.Validaciones
{
    public static class Validador
    {
        /// <summary>
        /// Cheque que los campos del objeto funcionario que es requerido en la base de datos no sea nulo
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
            //el Log id no lo valido xq es autogenerado


            return estado;
        }
    }
}
