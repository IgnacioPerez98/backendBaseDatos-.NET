using backendBaseDatos.Models;
using backendBaseDatos.Servicios.MySQL;
using backendBaseDatos.Servicios.Validaciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using backendBaseDatos.Servicios;

namespace backendBaseDatos.Controllers
{
    [ApiController]
    [Route("api/funcionarios")]
    public class FuncionariosController : Controller
    {
        private readonly ILogger<FuncionariosController> _logger;
        private readonly MySQLInsert DDBBInsert;
        public FuncionariosController(ILogger<FuncionariosController> logger,MySQLInsert mySQLInsert)
        {
            _logger = logger;
            DDBBInsert = mySQLInsert;
        }

        [HttpPost("funcionario")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Funcionario creado con éxito")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Error en la validacion del funcionario")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Description = "Error en el token proporcionado")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "Error del servidor")]
        [Authorize]
        public ActionResult CrearFuncionarios([FromBody] Funcionarios funcionario)
        {
            try
            {
                var validate = Validador.ValidarFuncionario(funcionario);
                if (!validate.IsOK)
                {
                    return StatusCode(400, new Error(400, validate.Message));
                }
                var token = this.Request.Headers.Authorization.ToString().Split("")[1];
                DDBBInsert.InsertarFuncionario(funcionario);
                return StatusCode(200, $"El funcionario {funcionario.Nombre}, se agrego con exito.");

            }catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear funcionario.");
                return StatusCode(500,new Error(500, ex.Message));
            }
        }


        //public ActionResult ActualizarFuncionario()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error al crear funcionario.");
        //        return StatusCode(500, new Error(500, ex.Message));
        //    }
        //}
    }
}
