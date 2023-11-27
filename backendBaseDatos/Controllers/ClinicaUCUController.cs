﻿ using backendBaseDatos.Models;
using backendBaseDatos.Servicios;
using backendBaseDatos.Servicios.MySQL;
using backendBaseDatos.Servicios.Validaciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backendBaseDatos.Controllers
{
    [Route("api/clinica")]
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
                    foreach (var t in s.TurnosDelPeriodo)
                    {
                        DDBBInsert.CargarNumeroAgenda(t);
                    }
                    fechitas = new List<Agenda>(s.TurnosDelPeriodo);
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


        [HttpPost("reservarhora/{inicioperiodo}/{finperiodo}")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Turno reservado exitosamente")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "La informacion proporcionada no es correcta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Description = "El token provisto no es valido.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "Excepción del servidor.")]
        [Authorize]
        public IActionResult ReservarHora([FromBody]Agenda turno,DateOnly inicioperiodo , DateOnly finperiodo)
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

                var ciValidate = CI_Validate.IsCIValid(turno.Ci);
                if (!ciValidate)
                {
                    return StatusCode(400, new Error(400,"La CI proporcionada no es válida, no verifica el dígito de validación." ));
                }
                if(!(toDatetime(inicioperiodo) <= turno.Fecha_Agenda && turno.Fecha_Agenda <= toDatetime(finperiodo)))
                {

                    return StatusCode(400, new Error(400, "La fecha no es valida, porque no esta en el periodo"));
                }

                var periodoActualizacion = DDBBGet.ObtenerPeriodoPorPK(inicioperiodo, finperiodo);
                if(!(periodoActualizacion.Fch_Inicio <= turno.Fecha_Agenda && turno.Fecha_Agenda <= periodoActualizacion.Fch_Fin))
                {
                    return StatusCode(400, new Error(400,$"El periodo va desde {periodoActualizacion.Fch_Inicio.ToShortDateString()}, hasta el {periodoActualizacion.Fch_Fin.ToShortDateString()}." +
                                                         $"El turno seleccionado no esta comprendido en ese periodo ({turno.ToString()})."));
                }
                //Reserva en la Base SQL
                Agenda nueva = new Agenda();
                nueva.Ci = turno.Ci;
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

        [HttpGet("periodos")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Turno reservado exitosamente")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "La informacion proporcionada no es correcta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Description = "El token provisto no es valido.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "Excepción del servidor.")]
        [Authorize] 
        public ActionResult ObtenerPeriodos()
        {
            try
            {
                var periodos = DDBBGet.GetPeriodos();
                return StatusCode(200, periodos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Error(500, ex.Message));
            }
        }



        private DateTime toDatetime(DateOnly d)
        {
            return new DateTime(d.Year, d.Month, d.Day);
        }
    }
}
