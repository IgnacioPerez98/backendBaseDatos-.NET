using MySql.Data.MySqlClient;
using System.Data;

namespace backendBaseDatos.Servicios.MySQL
{
    public class BaseMySql
    {
        public string Server { get; set; } = "mysql";
        public string Database { get; set; } = "proyectobbdd";
        public uint Port { get; set; } = 3306;
        public string Username { get; set; } = "sa";
        public string Password { get; set; } = "proyecto";

        private MySqlConnection _connection;

        public MySqlConnection getConection()
        {
            try
            {
                string connectionString = new MySqlConnectionStringBuilder
                {
                    Server = Server,
                    Port = Port,
                    Database = Database,
                    UserID = Username,
                    Password = Password,
                    Pooling = true,
                    MinimumPoolSize = 3,
                    MaximumPoolSize = 100
                    
                }.ToString();
                if (_connection == null )
                {
                    _connection = new MySqlConnection(connectionString);
                }

                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                return _connection;
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"MySQL Exception: {e.Message}");
                Console.WriteLine($"StackTrace: {e.StackTrace}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }

            return null;
        }



    }
}
