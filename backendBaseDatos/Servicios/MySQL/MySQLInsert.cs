using backendBaseDatos.Models;
using MySql.Data.MySqlClient;

namespace backendBaseDatos.Servicios.MySQL
{
    public class MySQLInsert:BaseMySql
    {
        public void InsertarFuncionario(Funcionarios funcionario)
        {
            string query = @"INSERT INTO funcionarios(ci,nombre,apellido,fch_nac,direccion, telefono, email,logid) values (@_v1,@_v2,@_v3,@_v4,@_v5,@_v6,@_v7, (SELECT COALESCE(MAX(logid),0)+1 FROM funcionarios))";
            using(MySqlCommand  cmd = new MySqlCommand(query,getConection()))
            {
                cmd.Connection.Open();
                cmd.Parameters.AddWithValue("@_v1",funcionario.Ci);
                cmd.Parameters.AddWithValue("@_v2",funcionario.Nombre);
                cmd.Parameters.AddWithValue("@_v3",funcionario.Apellido);
                cmd.Parameters.AddWithValue("@_v4",funcionario.Fch_Nac);
                cmd.Parameters.AddWithValue("@_v5",funcionario.Direccion);
                cmd.Parameters.AddWithValue("@_v6",funcionario.Telefono);
                cmd.Parameters.AddWithValue("@_v7",funcionario.Email);

                var affROws = cmd.ExecuteNonQuery();
                if (affROws == 0) throw new Exception("EL comando no afecto ninguna fila.");
                cmd.Connection.Close();
            }
        }
    }
}
