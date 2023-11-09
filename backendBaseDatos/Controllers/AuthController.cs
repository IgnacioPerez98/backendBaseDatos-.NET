using backendBaseDatos.Models.Requests;
using backendBaseDatos.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backendBaseDatos.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        public AuthController()
        {
        }

        [HttpPost("gettoken")]
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
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Email = token.Payload.First().Value,
                        ValidTo = token.Payload.ValidTo,
                        iat = token.Payload.Iat

                    });
                }
                else
                {
                    return StatusCode(403, new { Message = "Fallo la autenticación, la clave y/o el mail proporcionados no son válidos" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Se produjo un error.", Details = ex.ToString() });
            }
        }

    }
}
