using backendBaseDatos.Models;

namespace backendBaseDatos.Servicios
{
    public class ServicioHorariosClinica
    {
        //HORARIO DE 8:00 a 17:00.
        public DateTime InicioPeriodo {  get; set; }
        public DateTime FinPeriodo { get; set; }

        public List<TurnoClinica> TurnosDelPeriodo { get; set; }= new List<TurnoClinica>();

        /// <summary>
        /// Crea el servicio y le carga los horarios.
        /// </summary>
        /// <param name="periodo"></param>
        public ServicioHorariosClinica(PeriodoActualizacion periodo)
        {
            InicioPeriodo = periodo.Fch_Inicio;
            FinPeriodo = periodo.Fch_Fin;
            TurnosDelPeriodo = CargarHorarios();
        }

        private List<TurnoClinica> CargarHorarios()
        {
            List<TurnoClinica> TurnosDelPeriodo = new List<TurnoClinica>();

            DateTime actual = InicioPeriodo;

            // Generate days
            while (actual.Date < FinPeriodo.Date || (actual.Date == FinPeriodo.Date && actual.TimeOfDay <= FinPeriodo.TimeOfDay))
            {
                DateTime turnoInicio = new DateTime(actual.Year, actual.Month, actual.Day, 8, 0, 0);
                for (int i = 0; i < 18; i++)
                {
                    TurnoClinica turno = new TurnoClinica
                    {
                        NumeroAgenda = i,
                        EstaReservado = false,
                        HoraInicio = turnoInicio,
                        HoraFin = turnoInicio.AddMinutes(30)
                    };
                    TurnosDelPeriodo.Add(turno);
                    turnoInicio = turnoInicio.AddMinutes(30);
                }
                actual = actual.AddDays(1);
            }
            return TurnosDelPeriodo;
        }

    }
}
