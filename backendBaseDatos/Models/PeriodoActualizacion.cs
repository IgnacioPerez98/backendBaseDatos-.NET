namespace backendBaseDatos.Models
{
    public class PeriodoActualizacion
    {
        public int Anio { get; set; }
        public int Semestre { get; set;}
        public DateTime Fch_Inicio { get; set; } 
        public DateTime Fch_Fin {  get; set; }

        public override string ToString()
        {
            return $"Semestre {Semestre}, Año {Anio}, desde el {Fch_Inicio.ToShortDateString()} al {Fch_Fin.ToShortDateString()}";
        }
    }
}
