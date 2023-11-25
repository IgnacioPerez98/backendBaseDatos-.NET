using backendBaseDatos.Models;
using backendBaseDatos.Models.Requests;
using MySql.Data.MySqlClient;

namespace backendBaseDatos.Servicios.MySQL
{
    public class MySQLUpdate:BaseMySql
    {
        public MySQLUpdate() { }


        public async void ActualizarFuncionario(FuncionarioUpdate f,string logID)
        {
            string query = @"UPDATE funcionarios SET  ";
            using(MySqlCommand cmd = new MySqlCommand(query,getConection()))
            {
                if (f != null)
                {
                    if (!string.IsNullOrEmpty(f.Direccion)) { query += " direccion = @ci,"; cmd.Parameters.AddWithValue("@ci", f.Direccion); }
                    if (!string.IsNullOrEmpty(f.Telefono)) { query += " telefono = @ci,"; cmd.Parameters.AddWithValue("@ci", f.Telefono); }
                    if (!string.IsNullOrEmpty(f.Email)) { query += " email = @ci,"; cmd.Parameters.AddWithValue("@ci", f.Email); }
                }
                if(query != "UPDATE funcionarios SET  ")
                {   
                    cmd.CommandText = query.Substring(0, query.Length - 1)+ " where logid = @logid";
                    cmd.Parameters.AddWithValue("@logid", logID);
                    await cmd.Connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    await cmd.Connection.CloseAsync();
                }
            }
            query = @"UPDATE logins SET  ";
            using (MySqlCommand cmd = new MySqlCommand(query, getConection()))
            {
                if (f != null)
                {
                    if (!string.IsNullOrEmpty(f.Password)) 
                    { 
                        query += $" password = @ci where logid = @logid "; 
                        cmd.Parameters.AddWithValue("@ci", f.Password); 
                    }
                }
                if (query != "UPDATE funcionarios SET  ")
                {
                    cmd.CommandText = query.Substring(0, query.Length - 1);
                    cmd.Parameters.AddWithValue("@logid", logID);
                    await cmd.Connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    await cmd.Connection.CloseAsync();
                }

            }

        }
    }
}
