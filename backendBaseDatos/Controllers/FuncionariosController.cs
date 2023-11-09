using backendBaseDatos.Models;
using backendBaseDatos.Servicios.MySQL;
using backendBaseDatos.Servicios.Validaciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize]
        public ActionResult CrearFuncionarios(Funcionarios funcionario)
        {
            try
            {
                
                var validate = Validador.ValidarFuncionario(funcionario);
                if (!validate.IsOK)
                {
                    return StatusCode(400, validate);
                }

                DDBBInsert.InsertarFuncionario(funcionario);
                return StatusCode(200, $"El funcionario {funcionario.Nombre}, se agrego con exito.");

            }catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear funcionario.");
                return StatusCode(500,ex);
            }
        }

        //public Funcionarios ObtenerFuncionarioporEmail(string email)
        //{

        //}
    }
}
