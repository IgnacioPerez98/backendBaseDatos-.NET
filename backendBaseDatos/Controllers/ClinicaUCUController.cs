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
        private readonly ClinicaMongo ClinicaMongoDB;

        public ClinicaUCUController(ILogger<ClinicaUCUController> logger, MySQLGet getmysql,ClinicaMongo monguito )
        {
            _logger = logger;
            DDBBGet = getmysql;
            ClinicaMongoDB = monguito;
        }

        [HttpGet("fechasdisponibles/{anio}/{semestre}")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Periodo obtenido exitosamente")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "La informacion proporcionada no es correcta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Description = "El token provisto no es valido.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "Excepción del servidor.")]
        [Authorize]
        public IActionResult ObtenerFechas([FromRoute] int anio , int semestre)
        {
            try
            {
                var res = Validador.ValidarAnioSemestre(anio, semestre);
                if (!res.IsOK)
                {
                    return StatusCode(500, res);
                }

                try
                {
                    var periodoActualizacion = DDBBGet.ObtenerPeriodoPorPK(anio, semestre);
                    var fechitas =  ClinicaMongoDB.ObtenerTurnos(periodoActualizacion);
                    if (fechitas != null)
                    {
                        return StatusCode(200, fechitas);
                    }
                    return StatusCode(500, new {Messagge = "No se pudieron generar/ recupera los turnos."});

                }
                catch (Exception e)
                {
                    return StatusCode(500, e);
                }
                
            }catch(Exception ex)
            {
                return StatusCode(500, ex);
            }

        }


        [HttpPost("reservarhora")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Periodo obtenido exitosamente")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "La informacion proporcionada no es correcta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Description = "El token provisto no es valido.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "Excepción del servidor.")]
        [Authorize]
        public IActionResult ReservarHora([FromBody]TurnoClinica turno)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
