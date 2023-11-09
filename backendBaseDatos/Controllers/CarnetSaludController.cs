using backendBaseDatos.Models;
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
        public CarnetSaludController(ILogger<CarnetSaludController> logger)
        {
            _logger = logger;
        }



        [HttpPost("carnetsalud")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description ="Excepción del servidor.")]
        [Authorize]
        public IActionResult CargarCarnetSalud([FromBody] Carnet_Salud carnet)
        {
            // A partir del token, obtiene el usuario
            try
            {
                return Ok(carnet);

            }catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

    }
}
