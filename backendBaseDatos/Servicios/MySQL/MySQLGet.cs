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

        public List<FuncionarioPendiente> ObtenerFuncionariosSinActualizar()
        {
            var lista = new List<FuncionarioPendiente>();
            string query = @"SELECT json_arrayagg(
                        json_object(
                            'Nombre' F.nombre, 
                            'Apellido',F.apellido,
                            'Email',F.email ,
                        )
                    )  
FROM funcionarios F join carnet_salud C on F.ci = C.ci
WHERE C.fch_vencimiento < current_date() or C.fch_emision is null
";
            using (MySqlCommand cmd = new MySqlCommand(query, getConection()))
            {
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                bool flag = true;
                while (reader.Read())
                {
                    flag = false;
                    lista = JsonConvert.DeserializeObject<List<FuncionarioPendiente>>(reader.GetString(0));
                }
                cmd.Connection.Close();
                if (flag)
                {
                    throw new Exception("No se puedo leer los registros.");
                }
            }
            return lista;
        }

        public PeriodoActualizacion ObtenerPeriodoPorPK(int anio, int semestre)
        {
            PeriodoActualizacion period = null;
            string query = @"SELECT JSON_OBJECT(
                    'Anio', anio,
                    'Semestre', semestre,
                    'Fch_Inicio',fch_inicio ,
                    'Fch_Fin', fch_fin,
                ) FROM periodos_actualizacion P WHERE P.anio = @panio and P.semestre = @psemestre";
            using (MySqlCommand cmd = new MySqlCommand(query, getConection()))
            {
                cmd.Connection.Open();
                cmd.Parameters.AddWithValue("@panio", anio);
                cmd.Parameters.AddWithValue("@psemestre", semestre);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) == false)
                    {
                        period = JsonConvert.DeserializeObject<PeriodoActualizacion>(reader.GetString(0));
                    }
                }
                cmd.Connection.Close();
            }
            return period;
        }
    }
}
