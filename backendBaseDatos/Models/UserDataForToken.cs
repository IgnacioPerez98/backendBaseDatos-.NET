namespace backendBaseDatos.Models
{
    public class UserDataForToken
    {
        public string Email { get; set; }
        public string Id { get; set; }
        public string Ci { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public bool EsAdmin { get; set; }
    }
}
