using MySql.Data.MySqlClient;

namespace backendBaseDatos.Servicios.MySQL
{
    public class BaseMySql
    {
        public string Server { get; set; } = "localhost";
        public string Port { get; set; } = "3306";
        public string Database { get; set; } = "proyectoback";
        public string Username { get; set; } = "root";
        public string Password { get; set; } = "contrasenia";

        public MySqlConnection getConection()
        {
            try
            {
                string connectionString = $"Server={Server}:{Port};Database={Database};Uid={Username};Pwd={Password};";

            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
       

    }
}
