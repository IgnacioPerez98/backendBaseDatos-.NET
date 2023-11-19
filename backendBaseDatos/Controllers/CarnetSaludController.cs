﻿using backendBaseDatos.Models;
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
        private readonly MySQLGet DDBBGet;
        public CarnetSaludController(ILogger<CarnetSaludController> logger, MySQLInsert mySQL, MySQLGet ddbbget)
        {
            _logger = logger;
            DDBBInsert = mySQL;
            DDBBGet = ddbbget;
        }

        [HttpPost("carnetsalud")]
        [SwaggerResponse(StatusCodes.Status200OK, Description ="Carnet de Salud subido/actualizado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description ="La informacion proporcionada no es correcta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Description ="El token provisto no es valido.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description ="Excepción del servidor.")]
        [Authorize]
        public IActionResult CargarCarnetSalud([FromBody] Carnet_Salud carnet)
        {
            try
            {
                var valRes = Validador.ValidarCarnetdeSalud(carnet);
                if(!valRes.IsOK)
                {
                    return StatusCode(400, new Error(400, valRes.Message));
                }
                try
                {
                    DDBBInsert.InsertarActualizarCarnetDeSalud(carnet);

                }catch(Exception ex)
                {
                    return StatusCode(500, new Error(500, ex.Message));
                }
                return Ok("El carnet se creo con exito");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Error(500, ex.Message));
            }
        }

        [HttpGet("obtenercarnet/{ci}")]        
        [SwaggerResponse(StatusCodes.Status200OK, Description ="Carnet de Salud")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description ="La informacion proporcionada no es correcta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Description ="El token provisto no es valido.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description ="Excepción del servidor.")]
        [Authorize]
        public IActionResult ObtenerCarnetporCI([FromRoute]string ci)
        {
            try
            {
                var valCI = CI_Validate.IsCIValid(ci);
                if (!valCI)
                {
                    return StatusCode(400, new Error(400,"El digito verificador de la ci no es valido "));
                }
                var cs = DDBBGet.GetCarnetSaludByCI(ci);
                if (cs != null)
                {
                    return StatusCode(200, cs);
                }
                else
                {
                    return StatusCode(500, new Error(500, "No se pudo obtener el carnet de salud."));
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new Error(500, e.Message));
            }
        }

    }
}
