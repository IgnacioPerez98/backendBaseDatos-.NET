using backendBaseDatos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backendBaseDatos.Controllers
{
    [Route("api/carnetsalud")]
    [ApiController]
    public class ClinicaUCUController : ControllerBase
    {
        [HttpGet("fechasdisponibles")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Periodo obtenido exitosamente")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "La informacion proporcionada no es correcta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Description = "El token provisto no es valido.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "Excepción del servidor.")]
        [Authorize]
        public IActionResult ObtenerFechas()
        {
            try
            {
                return null;
            }catch(Exception ex)
            {
                return StatusCode(500, ex);
            }

        }


        [HttpPost("reservarhora")]
        [Authorize]
        public void ReservarHora([FromBody]TurnoClinica turno)
        {

        }
    }
}
