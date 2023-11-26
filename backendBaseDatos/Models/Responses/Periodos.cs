using MongoDB.Driver;

namespace backendBaseDatos.Models.Responses
{
    public class Periodos
    {
        public int Anio { get; set; }
        public int Semestre { get; set; }
        public DateTime? Fch_Inicio { get; set; }
        public DateTime? Fch_Fin { get; set; }
        public bool IsOpen => Fch_Inicio <= DateTime.Now && DateTime.Now <= Fch_Fin;
    }
}
