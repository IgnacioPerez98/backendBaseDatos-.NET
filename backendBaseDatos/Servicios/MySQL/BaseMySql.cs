using MySql.Data.MySqlClient;

namespace backendBaseDatos.Servicios.MySQL
{
    public class BaseMySql
    {
        public string Server { get; set; } = "localhost";
        public string Database { get; set; } = "proyectoback";
        public string Username { get; set; } = "root";
        public string Password { get; set; } = "proyecto";

        private MySqlConnection _connection;

        public MySqlConnection getConection()
        {
            try
            {
                string connectionString = $"Server={Server};Database={Database};Uid={Username};Pwd={Password};";
                if(_connection == null)
                {
                    _connection = new MySqlConnection(connectionString);
                }
                return _connection;

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
