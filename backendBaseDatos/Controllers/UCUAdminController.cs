using backendBaseDatos.Models;
using backendBaseDatos.Servicios.MySQL;
using backendBaseDatos.Servicios.Validaciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backendBaseDatos.Controllers
{
    [ApiController]
    [Route("api/ucuadmin")]
    public class UCUAdminController : ControllerBase
    {
        private readonly ILogger<UCUAdminController> _logger;
        private readonly MySQLInsert DDBBInsert;
        private readonly MySQLGet DDBBGet;
        public UCUAdminController(ILogger<UCUAdminController> logger, MySQLInsert mySQL, MySQLGet mySQLGet) 
        {
            _logger = logger;
            DDBBInsert = mySQL;
            DDBBGet = mySQLGet;
        }

        [HttpPost("periodoespecial")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Periodo generado con éxito")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Error en la validacion del periodo")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Description = "Error en el token proporcionado")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "Error del servidor")]
        [Authorize]
        public IActionResult AbrirPeriodoEspecial([FromBody]PeriodoActualizacion period)
        {
            try
            {
                var validate = Validador.ValidarPeriodo(period);
                if (validate.IsOK == false )
                {
                    return StatusCode(400, validate);
                }
                DDBBInsert.CrearPeriodo(period);
                return StatusCode(200, new { Message = "El periodo se creo de forma exitosa." });
            }catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("funcionariosdesactualizado")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Funcionarios recuperados con éxito")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Error en la validación del periodo")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Description = "Error en el token proporcionado")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "Error del servidor")]
        [Authorize]
        public IActionResult ObtenerFuncionariosdesactualizados()
        {
            try
            {
                var listado = DDBBGet.ObtenerFuncionariosSinActualizar();
                return StatusCode(200, listado);    
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }


    }
}
