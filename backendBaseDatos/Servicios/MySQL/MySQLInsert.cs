using backendBaseDatos.Models;
using MySql.Data.MySqlClient;
using System.Collections;

namespace backendBaseDatos.Servicios.MySQL
{
    public class MySQLInsert:BaseMySql
    {
        public void InsertarFuncionario(Funcionarios funcionario)
        {
            using (MySqlConnection connection = getConection())
            {
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insertar en la tabla logins
                        string queryLogin = @"INSERT INTO logins (logid, password)
                                            SELECT COALESCE(MAX(logid) + 1, 0),@password
                                            FROM logins;
                                            ";

                        using (MySqlCommand cmdLogin = new MySqlCommand(queryLogin, connection, transaction))
                        {
                            cmdLogin.Parameters.AddWithValue("@password", SHA512Service.Encrypt(funcionario.Password));
                            var affRowsLogin = cmdLogin.ExecuteNonQuery();

                            if (affRowsLogin == 0)
                            {
                                throw new Exception("El comando de insertar logins no afectó ninguna fila.");
                            }
                        }

                        // Insertar en la tabla funcionarios
                        string query = @"
                    INSERT INTO funcionarios(ci,nombre,apellido,fch_nac,direccion, telefono, email,esadmin,logid) 
                    values (@_v1,@_v2,@_v3,@_v4,@_v5,@_v6,@_v7,@_v8, (SELECT COALESCE(MAX(logid),0) FROM logins));";

                        using (MySqlCommand cmdFuncionario = new MySqlCommand(query, connection, transaction))
                        {
                            cmdFuncionario.Parameters.AddWithValue("@_v1", funcionario.Ci);
                            cmdFuncionario.Parameters.AddWithValue("@_v2", funcionario.Nombre);
                            cmdFuncionario.Parameters.AddWithValue("@_v3", funcionario.Apellido);
                            cmdFuncionario.Parameters.AddWithValue("@_v4", funcionario.Fch_Nac);
                            cmdFuncionario.Parameters.AddWithValue("@_v5", funcionario.Direccion);
                            cmdFuncionario.Parameters.AddWithValue("@_v6", funcionario.Telefono);
                            cmdFuncionario.Parameters.AddWithValue("@_v7", funcionario.Email);
                            cmdFuncionario.Parameters.AddWithValue("@_v8", funcionario.EsAdmin);

                            var affRowsFuncionario = cmdFuncionario.ExecuteNonQuery();

                            if (affRowsFuncionario == 0)
                            {
                                throw new Exception("El comando de insertar funcionarios no afectó ninguna fila.");
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
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
                cmd.Parameters.AddWithValue("@ci",carnet.Ci);
                cmd.Parameters.AddWithValue("@fch_emi",carnet.Fecha_Emision);
                cmd.Parameters.AddWithValue("@fch_venc",carnet.Fecha_Vencimiento);
                cmd.Parameters.Add("@comprobante", MySqlDbType.Blob).Value = carnet.Image;
                int rowAffected = cmd.ExecuteNonQuery();
                if (rowAffected == 0) throw new Exception("No se efectuaron cambios en la base de datos.");
            }
        }
        public void CrearPeriodo(PeriodoActualizacion periodo)
        {
            string query = @"INSERT INTO periodos_actualizacion(
                    anio, semestre, fch_inicio, fch_fin) VALUES ( @anio, @semestre, @fch_inicio, @fch_fin)";
            using(MySqlCommand cmd = new MySqlCommand(query, getConection()))
            {
                cmd.Parameters.AddWithValue("@anio",periodo.Anio);
                cmd.Parameters.AddWithValue("@semestre", periodo.Semestre);
                cmd.Parameters.AddWithValue("@fch_inicio", periodo.Fch_Inicio);
                cmd.Parameters.AddWithValue("@fch_fin", periodo.Fch_Fin);
                var affRows = cmd.ExecuteNonQuery();
                if(affRows == 0)
                {
                    throw new Exception("No se afectaron filas.");
                }
                getConection().Close();
            }
        }
        public void CargarNumeroAgenda(Agenda agenda)
        {
            string query = @"INSERT INTO agenda(nro, ci, fch_agenda,estareservado) VALUES (@nro, @ci, @fch_agenda,@estareservado)
                            ON DUPLICATE KEY UPDATE estareservado = 1 , ci = @ci;
                ";
            using (MySqlCommand cmd = new MySqlCommand(query,getConection()))
            {
                cmd.Parameters.AddWithValue("@nro", agenda.Numero);
                cmd.Parameters.AddWithValue("@ci", agenda.Ci);
                cmd.Parameters.AddWithValue("@fch_agenda", agenda.Fecha_Agenda);
                cmd.Parameters.AddWithValue("@estareservado", agenda.EstaReservado);
                var response = cmd.ExecuteNonQuery();
                if (response == 0) throw new Exception("No se pudo modificar el registro en la base de datos");
                getConection().Close();
            }
        }
    }
}
