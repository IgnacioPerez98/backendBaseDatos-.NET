using backendBaseDatos.Servicios.MySQL;

namespace backendBaseDatos.Servicios
{
    public static class AuthService
    {
        public static bool ValidarUsuario(string email, string password)
        {
            MySQLGet consultaBBDD = new MySQLGet();
            var usr = consultaBBDD.ObtenerPorEmail(email);
            if(usr != null )
            {
                return usr.Email == email && usr.Password == SHA512Service.Encrypt(password);
            }
            return false;
        }
    }
}
