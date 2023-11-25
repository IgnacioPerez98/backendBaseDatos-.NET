namespace backendBaseDatos.Models
{
    public class Funcionarios
    {
        public Funcionarios() { }
        public string? Ci {  get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public DateTime? Fch_Nac {  get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set;}
        public string? Password { get; set;}
        public bool? EsAdmin { get; set;}
    }
}
