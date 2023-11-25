using backendBaseDatos.Models;
using backendBaseDatos.Servicios.MySQL;
using backendBaseDatos.Servicios.Validaciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using backendBaseDatos.Servicios;
using backendBaseDatos.Models.Requests;

namespace backendBaseDatos.Controllers
{
    [ApiController]
    [Route("api/funcionarios")]
    public class FuncionariosController : Controller
    {
        private readonly ILogger<FuncionariosController> _logger;
        private readonly MySQLInsert DDBBInsert;
        private readonly MySQLUpdate DDBBUpdate;
        public FuncionariosController(ILogger<FuncionariosController> logger,MySQLInsert mySQLInsert, MySQLUpdate BBDDUpdate)
        {
            _logger = logger;
            DDBBInsert = mySQLInsert;
            DDBBUpdate = BBDDUpdate;
        }

        [HttpPost("funcionario")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Funcionario creado con éxito")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Error en la validacion del funcionario")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Description = "Error en el token proporcionado")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "Error del servidor")]
        public ActionResult CrearFuncionarios([FromBody] Funcionarios funcionario)
        {
            try
            {
                string token = "";
                try
                {
                    token = this.Request.Headers.Authorization.ToString().Split(" ")[1];
                }
                catch (Exception) { }
                var validate = Validador.ValidarFuncionario(funcionario, token);
                if (!validate.IsOK)
                {
                    return StatusCode(400, new Error(400, validate.Message));
                }
                DDBBInsert.InsertarFuncionario(funcionario);
                return StatusCode(200, $"El funcionario {funcionario.Nombre}, se agrego con exito.");

            }catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear funcionario.");
                return StatusCode(500,new Error(500, ex.Message));
            }
        }


        [HttpPatch("funcionarioactual")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Funcionario actualizado con éxito")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Error en la validacion del funcionario")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Description = "Error en el token proporcionado")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "Error del servidor")]
        [Authorize]
        public ActionResult ActualizarFuncionario([FromBody] FuncionarioUpdate func)
        {
            try
            {
                var logId = JWTService.ClaimFromToken(Request.Headers.Authorization.ToString().Split(" ")[1], "id");
                if (string.IsNullOrEmpty(logId)) return StatusCode(500, new Error(500, "No se pudo recuperar el id del funcionario."));
                //El func no se valida porque lo hace la consulta
                DDBBUpdate.ActualizarFuncionario(func, logId);
                return StatusCode(200, "Funcionario actualizado con exito.");
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear funcionario.");
                return StatusCode(500, new Error(500, ex.Message));
            }
        }
    }
}
