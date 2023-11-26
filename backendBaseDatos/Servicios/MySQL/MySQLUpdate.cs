using backendBaseDatos.Models;
using backendBaseDatos.Models.Requests;
using MySql.Data.MySqlClient;

namespace backendBaseDatos.Servicios.MySQL
{
    public class MySQLUpdate:BaseMySql
    {
        public MySQLUpdate() { }

        public async Task ActualizarFuncionario(FuncionarioUpdate f, string logID)
        {
            using (MySqlConnection connection = getConection())
            {
                await connection.OpenAsync();

                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await ActualizarFuncionarioTable(f, logID, connection, transaction);
                        await ActualizarLoginsTable(f, logID, connection, transaction);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error updating funcionario: {ex.Message}");
                        transaction.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private async Task ActualizarFuncionarioTable(FuncionarioUpdate f, string logID, MySqlConnection connection, MySqlTransaction transaction)
        {
            string query = "UPDATE funcionarios SET ";

            using (MySqlCommand cmd = new MySqlCommand(query, connection, transaction))
            {
                if (f != null)
                {
                    if (!string.IsNullOrEmpty(f.Direccion)) { query += " direccion = @direccion,"; cmd.Parameters.AddWithValue("@direccion", f.Direccion); }
                    if (!string.IsNullOrEmpty(f.Telefono)) { query += " telefono = @tel,"; cmd.Parameters.AddWithValue("@tel", f.Telefono); }
                    if (!string.IsNullOrEmpty(f.Email)) { query += " email = @mail,"; cmd.Parameters.AddWithValue("@mail", f.Email); }
                }

                if (query != "UPDATE funcionarios SET ")
                {
                    cmd.CommandText = query.Substring(0, query.Length - 1) + " WHERE logid = @logid";
                    cmd.Parameters.AddWithValue("@logid", logID);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task ActualizarLoginsTable(FuncionarioUpdate f, string logID, MySqlConnection connection, MySqlTransaction transaction)
        {
            string query = "UPDATE logins SET ";

            using (MySqlCommand cmd = new MySqlCommand(query, connection, transaction))
            {
                if (f != null && !string.IsNullOrEmpty(f.Password))
                {
                    query += " password = @ci WHERE logid = @logid ";
                    cmd.Parameters.AddWithValue("@ci", SHA512Service.Encrypt(f.Password));
                }

                if (query != "UPDATE logins SET ")
                {
                    cmd.CommandText = query.Substring(0, query.Length - 1);
                    cmd.Parameters.AddWithValue("@logid", logID);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        //public async void ActualizarFuncionario(FuncionarioUpdate f,string logID)
        //{
        //    string query = @"UPDATE funcionarios SET  ";
        //    using(MySqlCommand cmd = new MySqlCommand(query,getConection()))
        //    {
        //        if (f != null)
        //        {
        //            if (!string.IsNullOrEmpty(f.Direccion)) { query += " direccion = @direccion,"; cmd.Parameters.AddWithValue("@direccion", f.Direccion); }
        //            if (!string.IsNullOrEmpty(f.Telefono)) { query += " telefono = @tel,"; cmd.Parameters.AddWithValue("@tel", f.Telefono); }
        //            if (!string.IsNullOrEmpty(f.Email)) { query += " email = @mail,"; cmd.Parameters.AddWithValue("@mail", f.Email); }
        //        }
        //        if(query != "UPDATE funcionarios SET  ")
        //        {   
        //            cmd.CommandText = query.Substring(0, query.Length - 1)+ " where logid = @logid";
        //            cmd.Parameters.AddWithValue("@logid", logID);
        //            await cmd.ExecuteNonQueryAsync();
        //        }
        //    }
        //    query = @"UPDATE logins SET  ";
        //    using (MySqlCommand cmd = new MySqlCommand(query, getConection()))
        //    {
        //        if (f != null)
        //        {
        //            if (!string.IsNullOrEmpty(f.Password)) 
        //            { 
        //                query += $" password = @ci where logid = @logid "; 
        //                cmd.Parameters.AddWithValue("@ci", SHA512Service.Encrypt(f.Password)); 
        //            }
        //        }
        //        if (query != "UPDATE funcionarios SET  ")
        //        {
        //            cmd.CommandText = query.Substring(0, query.Length - 1);
        //            cmd.Parameters.AddWithValue("@logid", logID);
        //            await cmd.ExecuteNonQueryAsync();
        //        }

        //    }

        //}

    }
}
