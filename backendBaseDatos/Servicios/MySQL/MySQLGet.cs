using backendBaseDatos.Models;
using backendBaseDatos.Models.Requests;
using backendBaseDatos.Models.Responses;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Net;
using System.Reflection.PortableExecutable;

namespace backendBaseDatos.Servicios.MySQL
{
    public class MySQLGet : BaseMySql
    {

        public Funcionarios GetFuncionarios(string ci)
        {
            string query = @"SELECT json_object(
	                    'Ci', F.ci,
                        'Nombre', F.nombre,
                        'Apellido', F.apellido ,
                        'Fch_Nac', F.fch_nac ,
                        'Direccion', F.direccion ,
                        'Telefono', F.telefono ,
                        'Email', F.email  ,
                        'Password', L.password ,
                        'EsAdmin', F.esadmin 
                    )
                    FROM funcionarios F join logins L on F.logid = L.logid
                    WHERE F.ci = @ci ;";
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, getConection()))
                {
                    cmd.Parameters.AddWithValue("@ci", ci);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        return JsonConvert.DeserializeObject<Funcionarios>(reader.GetString(0));

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                getConection().Close();
            }
            return null;
        }

        public UserDataForToken ObtenerPorEmail(string email)
        {
            string query = @"SELECT F.email, L.password,F.esadmin, F.nombre, F.ci,F.logid
                        FROM funcionarios F join logins L on F.logid = L.logid
                        WHERE F.email = @email_param ;";
            try
            {
                using(MySqlCommand cmd = new MySqlCommand(query,getConection()))
                {
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
                            Ci = reader.GetString(4),
                            Id = reader.GetInt32(5).ToString(),
                        };
                    }
                }
            }
            catch (Exception ex )
            {
                Console.WriteLine(ex);
            }
            finally
            {
                getConection().Close();
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
                var reader = cmd.ExecuteReader();
                bool flag = true;
                while (reader.Read())
                {
                    flag = false;
                    lista = JsonConvert.DeserializeObject<List<FuncionarioPendiente>>(reader.GetString(0));
                }
                getConection().Close();
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
                getConection().Close();
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
                getConection().Close();
                return agendas;
            }
        }
        public List<Periodos> GetPeriodos()
        {
            List<Periodos> variable = new List<Periodos>();
            string query = @"SELECT JSON_ARRAY
                            (
	                            JSON_OBJECT(
		                            'Anio' ,anio,
                                    'Semestre', semestre,
                                    'Fch_Inicio',fch_inicio,
                                    'Fch_Fin', fch_fin
                                )
                            ) FROM periodos_actualizacion;";
            using(MySqlCommand cmd = new MySqlCommand( query, getConection()))
            {
                var reader =cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        variable = JsonConvert.DeserializeObject<List<Periodos>>(reader.GetString(0));
                    }
                }   
            }

            getConection().Close();
            return variable;

        }
        public Carnet_Salud GetCarnetSaludByCI(string ci)
        {
            string query = @"SELECT json_object(
	            'Ci',ci,
                'Fecha_Emision', fch_emision,
                'Fecha_Vencimiento', fch_vencimiento,
                'Image', null
            ) FROM carnet_salud where ci = @ci;";
            using (MySqlCommand cmd = new MySqlCommand(query, getConection()))
            {
                cmd.Parameters.AddWithValue("@ci", ci);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        var variable = JsonConvert.DeserializeObject<Carnet_Salud>(reader.GetString(0));
                        getConection().Close();
                        return variable;
                    }
                }
            }
            getConection().Close();
            throw new Exception("No se puedo encontrar el Carnet de Salud");
        }

        public string GetFoto(string ci)
        {
            string query = "SELECT comprobante FROM carnet_salud where ci = @cedula";
            using (MySqlCommand cmd = new MySqlCommand(query, getConection()))
            {
                cmd.Parameters.AddWithValue("@cedula", ci);
                var reader = cmd.ExecuteReader(System.Data.CommandBehavior.SequentialAccess);
                if (reader.Read())
                {
                    var memoryStream = new MemoryStream();
                    var stream = reader.GetStream(0);
                    stream.CopyTo(memoryStream);

                    // Convert the image to a Base64 string
                    var foto = Convert.ToBase64String(memoryStream.ToArray());
                    getConection().Close();
                    return foto;

                }
            }
            getConection().Close();
            throw new Exception("No se puedo recuperar la imagen solicitada");
        }
    }
}
