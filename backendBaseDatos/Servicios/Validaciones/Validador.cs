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


        public static ValidateStatus ValidarCarnetdeSalud(Carnet_Salud carnet)
        {
            ValidateStatus estado = new();
            if (carnet == null) return new(false, "Faltan los parametros.");
            if (string.IsNullOrEmpty(carnet.Ci)) return new(false, "La CI es requerida.");
            if (!CI_Validate.IsCIValid(carnet.Ci)) return new(false, "La CI es proporcionada no es valida.");
            return estado;
        }

        public static ValidateStatus ValidarPeriodo(PeriodoActualizacion periodod)
        {
            ValidateStatus estado = new();
            if (periodod == null) return new(false, "La informacion proporcionada esta vacia.");
            if (periodod.Anio == null) return new(false,"El año no puede estar vacio");
            if (periodod.Anio < DateTime.Now.Year) return new(false,"No se puede setear unn periodo anterior a la fecha actual");
            if (periodod.Semestre == null) return new(false,"El semestre no puede estar vacio.");
            if (!(1<=periodod.Semestre && periodod.Semestre <= 2)) return new(false,"El semestre debe ser un valor entre 1 y 2");
            if (periodod.Fch_Inicio == null) return new(false, "La fecha de inicio no puede estar vacia.");
            if (periodod.Fch_Fin == null) return new(false, "La fecha de fin no puede estar vacia.");
            if (periodod.Fch_Fin < periodod.Fch_Inicio) return new(false, "La fecha de inicio no coincide con la fecha de fin.");

            return estado;
        }

        public static ValidateStatus ValidarAnioSemestre(int anio, int semestre)
        {
            ValidateStatus v = new();
            if (!(1900 <= anio && anio <= 3000)) return new(false, "EL año debe estar entre el  1900 y el 3000.");
            if (!(1 <= semestre && semestre <= 2)) return new(false, "El semstre debe ser 1 o 2.");
            return v;
        }
        public static ValidateStatus ValidarTurno(Agenda turno)
        {
            ValidateStatus estado = new();
            if (turno == null) return new(false, "El turno provisto no es válido.");
            if (turno.Fecha_Agenda == null) return new(false, "La hora de inicio es requerida");
            return estado;
        }

        public static ValidateStatus ValidarRangoFechas(DateOnly inicio, DateOnly final)
        {
            ValidateStatus v = new();
            if(inicio == null ) return new (false, "Es necesario proporcionar una fecha de inicio.");
            if(final == null ) return new (false, "Es necesario proporcionar una fecha de finalizacion.");
            var init = new DateOnly(1900,1,1); 
            var end = new DateOnly(3000,12,31);
            if(!(init <= inicio && inicio <= end)) return new (false,"La fecha de inicio no esta en un rango válido");
            if(!(init <= final && final <= end)) return new (false,"La fecha de finalización no esta en un rango válido");
            if(inicio > final) return new(false, "La fecha de finalización no puede ser menor que la de apertura");
            return v;
        }

        /// <summary>
        /// Determina si una fecha esta dentro del rango valido. Retorna true si esta correcta, false si es incorrecta.
        /// </summary>
        /// <returns>bool</returns>
        private static bool FechaInPeriod(DateTime Fecha)
        {
            if (Fecha == null) return false;
            var inicio = new DateTime(1900, 1, 1);
            var final = new DateTime(3000, 12, 31);
            if (Fecha < inicio) return false;
            if (Fecha > final) return false;
            return true;
        }
    }
}
