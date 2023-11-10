namespace backendBaseDatos.Models
{
    public class TurnoClinica
    {
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get => HoraInicio.AddMinutes(30); }

        public int NumeroAgenda { get; set; }
        public bool EstaReservado { get; set; } = false;


        public override string ToString()
        {
            return $"Turno {NumeroAgenda}. \nReservado: {EstaReservado}.\nFecha: {HoraInicio.ToShortDateString()}.\n Horario: {HoraInicio.ToShortTimeString()}-{HoraFin.ToShortTimeString()} "; ;
        }
    }
}
