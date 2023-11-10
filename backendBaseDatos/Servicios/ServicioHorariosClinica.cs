using backendBaseDatos.Models;

namespace backendBaseDatos.Servicios
{
    public class ServicioHorariosClinica
    {
        //HORARIO DE 8:00 a 17:00.
        public DateTime InicioPeriodo {  get; set; }
        public DateTime FinPeriodo { get; set; }

        public List<TurnoClinica> TurnosDelPeriodo { get; set; }= new List<TurnoClinica>();


        private static ServicioHorariosClinica _instance = null;

        private ServicioHorariosClinica()
        {
        }

        public static ServicioHorariosClinica GetInstance()
        {
            if(_instance == null)
            {
                //pegarle a la base de datos Mongo DB

            }
            return _instance;
        }
        public void CargarHorarios()
        {
            _instance.TurnosDelPeriodo = new List<TurnoClinica>();

            //Genero dias
            



        }
        
    }
}
