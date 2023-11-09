namespace backendBaseDatos.Models
{
    public class Carnet_Salud
    {
        public string? Ci { get; set; }
        public DateTime? Fecha_Emision { get; set; }
        public DateTime? Fecha_Vencimiento { get; set; }
        public byte []? Image {  get; set; }

    }
}
