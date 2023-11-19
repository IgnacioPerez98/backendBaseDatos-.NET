using backendBaseDatos.Models.Requests;
using backendBaseDatos.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backendBaseDatos.Models;

namespace backendBaseDatos.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;   
        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpPost("gettoken")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Retorna el token")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, Description = "Error en el token proporcionado")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "Error del servidor")]
        public IActionResult GetToken([FromBody] LoginRequest model)
        {
            try
            {
                var user = AuthService.ValidarUsuario(model.Email, model.Password);
                if (user)
                {
                    var token = JWTService.GenerateToken(model.Email);
                    return Ok(new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token)
                    });
                }
                else
                {
                    return StatusCode(403, new Error( 403,"Fallo la autenticación, la clave y/o el mail proporcionados no son válidos"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Error( 403,"Se produjo un error."));
            }
        }

    }
}
