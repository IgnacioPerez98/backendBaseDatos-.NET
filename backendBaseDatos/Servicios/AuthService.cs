using backendBaseDatos.Models;
using backendBaseDatos.Models.Requests;
using backendBaseDatos.Servicios.MySQL;

namespace backendBaseDatos.Servicios
{
    public static class AuthService
    {
        public static UserDataForToken ValidarUsuario(string email, string password)
        {
            MySQLGet consultaBBDD = new MySQLGet();
            return consultaBBDD.ObtenerPorEmail(email);
        }
    }
}
