using backendBaseDatos.Models;
using backendBaseDatos.Servicios.MongoDB;
using backendBaseDatos.Servicios.MySQL;
using backendBaseDatos.Servicios.Validaciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backendBaseDatos.Controllers
{
    [Route("api/carnetsalud")]
    [ApiController]
    public class ClinicaUCUController : ControllerBase
    {
        private readonly ILogger<ClinicaUCUController> _logger = null;
        private readonly MySQLGet DDBBGet;        
        private readonly MySQLInsert DDBBInsert;

        private readonly ClinicaMongo ClinicaMongoDB;

        public ClinicaUCUController(ILogger<ClinicaUCUController> logger, MySQLGet getmysql,MySQLInsert insertsql,ClinicaMongo monguito )
        {
            _logger = logger;
            DDBBGet = getmysql;
            ClinicaMongoDB = monguito;
            DDBBInsert = insertsql;
        }

        [HttpGet("fechasdisponibles/{inicio}/{fin}")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Periodo obtenido exitosamente")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "La informacion proporcionada no es correcta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Description = "El token provisto no es valido.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "Excepción del servidor.")]
        [Authorize]
        public IActionResult ObtenerFechas([FromRoute] DateOnly inicio , DateOnly fin)
        {
            try
            {
                var res = Validador.ValidarRangoFechas(inicio, fin);
                if (!res.IsOK)
                {
                    return StatusCode(500, res);
                }                    
                var periodoActualizacion = DDBBGet.ObtenerPeriodoPorPK(inicio, fin);
                var fechitas =  ClinicaMongoDB.ObtenerTurnos(periodoActualizacion);
                if (fechitas != null)
                {
                    return StatusCode(200, fechitas);
                }
                return StatusCode(500, new {Messagge = "No se pudieron generar y/o recuperar los turnos."});

            }catch(Exception ex)
            {
                return StatusCode(500, ex);
            }
        }


        [HttpPost("reservarhora/{inicioperiodo}/{finperiodo}/{ci}")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Turno reservado exitosamente")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "La informacion proporcionada no es correcta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Description = "El token provisto no es valido.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "Excepción del servidor.")]
        [Authorize]
        public IActionResult ReservarHora([FromBody]TurnoClinica turno,DateOnly inicioperiodo , DateOnly finperiodo,string ci)
        {
            try
            {
                var turnoState = Validador.ValidarTurno(turno);
                if (!turnoState.IsOK)
                {
                    return StatusCode(400, turnoState);
                }

                var valPeriodo = Validador.ValidarRangoFechas(inicioperiodo, finperiodo);
                if (valPeriodo.IsOK == false)
                {
                    return StatusCode(400, valPeriodo);
                }

                var ciValidate = CI_Validate.IsCIValid(ci);
                if (!ciValidate)
                {
                    return StatusCode(400, new { Message = "La CI proporcionada no es válida, no verifica el dígito de validación." });
                }
                var periodoActualizacion = DDBBGet.ObtenerPeriodoPorPK(inicioperiodo, finperiodo);

                if(!(periodoActualizacion.Fch_Inicio <= turno.HoraInicio && turno.HoraInicio <= periodoActualizacion.Fch_Fin))
                {
                    return StatusCode(400, new { Message = $"El periodo va desde {periodoActualizacion.Fch_Inicio.ToShortDateString()}, hasta el {periodoActualizacion.Fch_Fin.ToShortDateString()}." +
                        $"El turno seleccionado no esta comprendido en ese periodo ({turno.ToString()})." });
                }

                //Reserva en la Base SQL
                Agenda nueva = new Agenda();
                nueva.Ci = ci;
                nueva.Numero = turno.HoraInicio.ToShortDateString() + turno.NumeroAgenda;
                nueva.Fecha_Agenda = turno.HoraInicio;
                DDBBInsert.CargarNumeroAgenda(nueva);
                //No valido que la cedula exista, xq si no existe va lanzar una excepcion cuando intente cargar el numero de agenda
                //para que se puedan agendar mas de uno en un dia, y con mas informacion asociada a los turnos en especifico, como cache.
                ClinicaMongoDB.ReservarTurno(turno, periodoActualizacion.ToString());
                return StatusCode(200, new { Message = "El turno se reservo de forma exitosa." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
