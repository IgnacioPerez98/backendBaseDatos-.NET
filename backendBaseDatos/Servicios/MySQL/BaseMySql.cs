using MySql.Data.MySqlClient;

namespace backendBaseDatos.Servicios.MySQL
{
    public class BaseMySql
    {
        public string Server { get; set; } = "mysql";
        public string Database { get; set; } = "proyectobbdd";
        public string Port { get; set; } = "3306";
        public string Username { get; set; } = "sa";
        public string Password { get; set; } = "proyecto";

        private MySqlConnection _connection;

        public MySqlConnection getConection()
        {
            try
            {
                string connectionString = $"Server={Server};Port={Port};Database={Database};Uid={Username};Pwd={Password};";
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
