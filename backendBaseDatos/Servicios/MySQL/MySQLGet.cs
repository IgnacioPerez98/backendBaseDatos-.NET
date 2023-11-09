using backendBaseDatos.Models;
using backendBaseDatos.Models.Requests;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Reflection.PortableExecutable;

namespace backendBaseDatos.Servicios.MySQL
{
    public class MySQLGet : BaseMySql
    {
        public LoginRequest ObtenerPorEmail(string email)
        {
            string query = @"SELECT F.email, L.password
                        FROM funcionarios F join logins L on F.logid = L.logid
                        WHERE F.email = @email_param
                        ";
            try
            {
                using(MySqlCommand cmd = new MySqlCommand(query,getConection()))
                {
                    cmd.Connection.Open();
                    cmd.Parameters.AddWithValue("@email_param", email);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        return new LoginRequest
                        {
                            Email = reader.GetString(0),
                            Password = reader.GetString(1),
                        };
                    }
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex )
            {
                Console.WriteLine(ex);
            }
            return null;
        } 
    }
}
