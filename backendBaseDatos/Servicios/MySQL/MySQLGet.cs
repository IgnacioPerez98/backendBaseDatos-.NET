using backendBaseDatos.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace backendBaseDatos.Servicios.MySQL
{
    public class MySQLGet : BaseMySql
    {
        public Funcionarios? ObtenerPorID(string email)
        {
            Funcionarios? f = null;
            string query = @"SELECT JSON_OBJECT(
                ) FROM funcionarios F join logins L on F.logid = L.logid
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
                        f = JsonConvert.DeserializeObject<Funcionarios>(reader.GetString(0));
                    }

                    cmd.Connection.Close();
                }

            }
            catch (Exception ex )
            {
                Console.WriteLine(ex);
            }


            return f;
        } 
    }
}
