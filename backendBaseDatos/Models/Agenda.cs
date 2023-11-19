namespace backendBaseDatos.Models
{
    public class Agenda
    {
        public int Numero {  get; set; }
        public string? Ci { get; set; } = null;
        public DateTime? Fecha_Agenda { get; set; }
        public bool EstaReservado { get; set; }
        
    }
}
