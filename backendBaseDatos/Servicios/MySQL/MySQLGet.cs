using backendBaseDatos.Models;
using backendBaseDatos.Models.Requests;
using backendBaseDatos.Models.Responses;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Reflection.PortableExecutable;

namespace backendBaseDatos.Servicios.MySQL
{
    public class MySQLGet : BaseMySql
    {
        public UserDataForToken ObtenerPorEmail(string email)
        {
            string query = @"SELECT F.email, L.password,F.esadmin, F.nombre
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
                        return new UserDataForToken
                        {
                            Email = reader.GetString(0),
                            Password = reader.GetString(1),
                            EsAdmin = reader.GetBoolean(2),
                            Nombre = reader.GetString(3),
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
                            'Nombre', F.nombre, 
                            'Apellido',F.apellido,
                            'Email',F.email
                        )
                    ) FROM funcionarios F left join carnet_salud C on F.ci = C.ci
                    where F.esadmin = 0 AND ( C.fch_vencimiento is null or C.fch_vencimiento < curdate())
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

        public PeriodoActualizacion ObtenerPeriodoPorPK(DateOnly inicio, DateOnly fin)
        {
            var start = new DateTime(inicio.Year, inicio.Month, inicio.Day);
            var finilize = new DateTime(fin.Year, fin.Month, fin.Day);

            PeriodoActualizacion period = null;
            string query = @"SELECT JSON_OBJECT(
                    'Anio', anio,
                    'Semestre', semestre,
                    'Fch_Inicio',fch_inicio ,
                    'Fch_Fin', fch_fin
                ) FROM periodos_actualizacion P WHERE P.fch_inicio = @fchinicio and P.fch_fin = @fchfin";
            using (MySqlCommand cmd = new MySqlCommand(query, getConection()))
            {
                cmd.Connection.Open();
                cmd.Parameters.AddWithValue("@fchinicio", start);
                cmd.Parameters.AddWithValue("@fchfin", finilize);
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

        public List<Agenda> ObtenerHorariosporPeriodo(PeriodoActualizacion p)
        {
            string query = @"SELECT JSON_ARRAYAGG(JSON_OBJECT(
	            'Numero',nro,
                'Ci',ci,
                'Fecha_Agenda',fch_agenda,
                'EstaReservado',estareservado
            )) FROM agenda where fch_agenda BETWEEN @inicio and @final;";
            using (MySqlCommand cmd = new MySqlCommand(query, getConection()))
            {
                List<Agenda> agendas = new List<Agenda>();
                cmd.Connection.Open();
                cmd.Parameters.AddWithValue("@inicio", p.Fch_Inicio);
                cmd.Parameters.AddWithValue("@final", p.Fch_Fin);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        agendas = JsonConvert.DeserializeObject<List<Agenda>>(reader.GetString(0));
                    }
                }

                cmd.Connection.Close();
                return agendas;
            }
        }
        public List<Periodos> GetPeriodos()
        {
            string query = @"SELECT JSON_ARRAY
                            (
	                            JSON_OBJECT(
		                            'Anio' ,anio,
                                    'Semesttre', semestre,
                                    'Fch_Inicio',fch_inicio,
                                    'Fch_Fin', fch_fin
                                )
                            ) FROM periodos_actualizacion;";
            using(MySqlCommand cmd = new MySqlCommand( query, getConection()))
            {
                cmd.Connection.Open();
                var reader =cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        return JsonConvert.DeserializeObject<List<Periodos>>(reader.GetString(0));
                    }
                }   
                cmd.Connection.Close();
            }
            return new List<Periodos>();

        }
        public Carnet_Salud GetCarnetSaludByCI(string ci)
        {
            string query = @"SELECT json_object(
	            'ci',ci,
                'fch_emision', fch_emision,
                'fch_vencimiento', fch_vencimiento,
                'comprobante', comprobante
            ) FROM carnet_salud where ci = @ci;";
            using (MySqlCommand cmd = new MySqlCommand(query, getConection()))
            {
                cmd.Connection.Open();
                cmd.Parameters.AddWithValue("@ci", ci);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        return JsonConvert.DeserializeObject<Carnet_Salud>(reader.GetString(0));
                    }
                }
                
                cmd.Connection.Close();
            }
            throw new Exception("No se puedo encontrar el Carnet de Salud");
        }
    }
}
