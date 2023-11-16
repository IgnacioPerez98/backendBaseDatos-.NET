using backendBaseDatos.Models;
using MySql.Data.MySqlClient;
using System.Collections;

namespace backendBaseDatos.Servicios.MySQL
{
    public class MySQLInsert:BaseMySql
    {
        public void InsertarFuncionario(Funcionarios funcionario)
        {
            string queryLogin = @"
                INSERT INTO logins (logid, password) VALUES ((SELECT COALESCE(MAX(logid),0)+1), @password);";
            using(MySqlCommand cmd = new MySqlCommand(queryLogin, getConection()))
            {
                cmd.Connection.Open();
                cmd.Parameters.AddWithValue("@password", SHA512Service.Encrypt(funcionario.Password));
                var affRows = cmd.ExecuteNonQuery();
                if (affRows == 0) throw new Exception("El comando de insertar funcionarios no afecto ninguna fila.");
                cmd.Connection.Close(); 
            }
            string query = @"
                INSERT INTO funcionarios(ci,nombre,apellido,fch_nac,direccion, telefono, email,logid) 
                values (@_v1,@_v2,@_v3,@_v4,@_v5,@_v6,@_v7, (SELECT COALESCE(MAX(logid),0) FROM logins))";
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

                var affRows = cmd.ExecuteNonQuery();
                if (affRows == 0) throw new Exception("El comando de insertar funcionarios no afecto ninguna fila.");
                cmd.Connection.Close();
            }
        }

        public void InsertarActualizarCarnetDeSalud(Carnet_Salud carnet)
        {
            string query = @"INSERT INTO carnet_salud(ci, fch_emision,fch_vencimiento, comprobante)
                    VALUES (@ci, @fch_emi, @fch_venc, @comprobante)
                    ON DUPLICATE KEY UPDATE 
                        fch_emision = @fch_emi,
                        fch_vencimiento = @fch_venc,
                        comprobante = @comprobante;
                    ";
            using( MySqlCommand cmd = new MySqlCommand( query,getConection()))
            {
                cmd.Connection.Open();
                cmd.Parameters.AddWithValue("@ci",carnet.Ci);
                cmd.Parameters.AddWithValue("@fch_emi",carnet.Fecha_Emision);
                cmd.Parameters.AddWithValue("@fch_venc",carnet.Fecha_Vencimiento);
                cmd.Parameters.Add("@comprobante", MySqlDbType.Blob).Value = carnet.Image;
                int rowAffected = cmd.ExecuteNonQuery();
                if (rowAffected == 0) throw new Exception("No se efectuaron cambios en la base de datos.");
                cmd.Connection.Close();
            }
        }
        public void CrearPeriodo(PeriodoActualizacion periodo)
        {
            string query = @"INSERT INTO periodos_actualizacion(
                    anio, semestre, fch_inicio, fch_fin) VALUES ( @anio, @semestre, @fch_inicio, @fch_fin)";
            using(MySqlCommand cmd = new MySqlCommand(query, getConection()))
            {
                cmd.Connection.Open();
                cmd.Parameters.AddWithValue("@anio",periodo.Anio);
                cmd.Parameters.AddWithValue("@semestre", periodo.Semestre);
                cmd.Parameters.AddWithValue("@fch_inicio", periodo.Fch_Inicio);
                cmd.Parameters.AddWithValue("@fch_fin", periodo.Fch_Fin);
                var affRows = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                if(affRows == 0)
                {
                    throw new Exception("No se afectaron filas.");
                }
            }
        }
        public void CargarNumeroAgenda(Agenda agenda)
        {
            string query = @"INSERT INTO agenda(nro, ci, fch_agenda) VALUES (@nro, @ci, @fch_agenda)";
            using (MySqlCommand cmd = new MySqlCommand(query,getConection()))
            {
                cmd.Connection.Open();
                cmd.Parameters.AddWithValue("@nro", agenda.Numero);
                cmd.Parameters.AddWithValue("@ci", agenda.Ci);
                cmd.Parameters.AddWithValue("@fch_agenda", agenda.Fecha_Agenda);
                var response = cmd.ExecuteNonQuery();
                if (response == 0) throw new Exception("No se pudo modificar el registro en la base de datos");
                
                cmd.Connection.Close();
                
            }
        }
    }
}
