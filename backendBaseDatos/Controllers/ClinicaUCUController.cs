using backendBaseDatos.Models;
using backendBaseDatos.Servicios;
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

        public ClinicaUCUController(ILogger<ClinicaUCUController> logger, MySQLGet getmysql,MySQLInsert insertsql)
        {
            _logger = logger;
            DDBBGet = getmysql;
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
                    return StatusCode(400, new Error(400, res.Message));
                }                    
                var periodoActualizacion = DDBBGet.ObtenerPeriodoPorPK(inicio, fin);
                var fechitas = DDBBGet.ObtenerHorariosporPeriodo(periodoActualizacion);
                if (fechitas.Count() == 0)
                {
                    ServicioHorariosClinica s = new ServicioHorariosClinica(periodoActualizacion);
                    foreach (var t in fechitas)
                    {
                        DDBBInsert.CargarNumeroAgenda(t);
                    }
                }
                if (fechitas != null)
                {
                    return StatusCode(200, fechitas);
                }
                return StatusCode(500, new Error(500,"No se pudieron generar y/o recuperar los turnos."));

            }catch(Exception ex)
            {
                return StatusCode(500, new Error(500, ex.Message));
            }
        }


        [HttpPost("reservarhora/{inicioperiodo}/{finperiodo}/{ci}")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Turno reservado exitosamente")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "La informacion proporcionada no es correcta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Description = "El token provisto no es valido.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "Excepción del servidor.")]
        [Authorize]
        public IActionResult ReservarHora([FromBody]Agenda turno,DateOnly inicioperiodo , DateOnly finperiodo,string ci)
        {
            try
            {
                var turnoState = Validador.ValidarTurno(turno);
                if (!turnoState.IsOK)
                {
                    return StatusCode(400, new Error(400, turnoState.Message));
                }

                var valPeriodo = Validador.ValidarRangoFechas(inicioperiodo, finperiodo);
                if (valPeriodo.IsOK == false)
                {
                    return StatusCode(400, new Error(400, valPeriodo.Message));
                }

                var ciValidate = CI_Validate.IsCIValid(ci);
                if (!ciValidate)
                {
                    return StatusCode(400, new Error(400,"La CI proporcionada no es válida, no verifica el dígito de validación." ));
                }
                var periodoActualizacion = DDBBGet.ObtenerPeriodoPorPK(inicioperiodo, finperiodo);
                if(!(periodoActualizacion.Fch_Inicio <= turno.Fecha_Agenda && turno.Fecha_Agenda <= periodoActualizacion.Fch_Fin))
                {
                    return StatusCode(400, new Error(400,$"El periodo va desde {periodoActualizacion.Fch_Inicio.ToShortDateString()}, hasta el {periodoActualizacion.Fch_Fin.ToShortDateString()}." +
                                                         $"El turno seleccionado no esta comprendido en ese periodo ({turno.ToString()})."));
                }
                //Reserva en la Base SQL
                Agenda nueva = new Agenda();
                nueva.Ci = ci;
                nueva.Numero = turno.Numero;
                nueva.Fecha_Agenda = turno.Fecha_Agenda;
                DDBBInsert.CargarNumeroAgenda(nueva);
                //No valido que la cedula exista, xq si no existe va lanzar una excepcion cuando intente cargar el numero de agenda
                return StatusCode(200,  "El turno se reservo de forma exitosa.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Error(500,ex.Message));
            }
        }
    }
}
