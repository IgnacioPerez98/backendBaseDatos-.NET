using backendBaseDatos.Models;
using backendBaseDatos.Servicios.MySQL;
using backendBaseDatos.Servicios.Validaciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backendBaseDatos.Controllers
{
    [Route("api/carnetsalud")]
    [ApiController]
    public class CarnetSaludController : ControllerBase
    {
        private readonly ILogger<CarnetSaludController> _logger;
        private readonly MySQLInsert DDBBInsert;
        public CarnetSaludController(ILogger<CarnetSaludController> logger, MySQLInsert mySQL)
        {
            _logger = logger;
            DDBBInsert = mySQL;
        }

        [HttpPost("carnetsalud")]
        [SwaggerResponse(StatusCodes.Status200OK, Description ="Carnet de Salud subido/actualizado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description ="La informacion proporcionada no es correcta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Description ="El token provisto no es valido.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description ="Excepción del servidor.")]
        [Authorize]
        public IActionResult CargarCarnetSalud([FromBody] Carnet_Salud carnet)
        {
            // A partir del token, obtiene el usuario
            try
            {
                var valRes = Validador.ValidarCanetdeSalud(carnet);
                if(!valRes.IsOK)
                {
                    return StatusCode(400, valRes);
                }
                try
                {
                    DDBBInsert.InsertarActualizarCarnetDeSalud(carnet);

                }catch(Exception ex)
                {
                    return StatusCode(500, ex);
                }
                return Ok("El carnet se creo con exito");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

    }
}
