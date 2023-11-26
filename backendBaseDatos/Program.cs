using backendBaseDatos.Servicios;
using backendBaseDatos.Servicios.MySQL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace backendBaseDatos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //JWT Auth
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTService.SecretKey))
                };
            });
            //Swagger
            builder.Services.AddSwaggerGen(option =>
            {
                option.EnableAnnotations();
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend Base de Datos", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Ingrese un token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            // Add services to the container.
            builder.Services.AddTransient(typeof(MySQLInsert));
            builder.Services.AddTransient(typeof(MySQLGet));
            builder.Services.AddTransient(typeof(MySQLUpdate));

            //Web Socket
            builder.Services.AddSignalR();

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors(
                options => options.WithOrigins(
                    "http://localhost:4200",
                    "https://localhost:4200",
                    "http://localhost:8080",
                    "http://localhost:8000",
                    "http://localhost:8800",
                    "https://localhost:8080", 
                    "https://localhost:8000",
                    "https://localhost:8800"
                ).AllowAnyHeader().AllowAnyMethod());

            app.MapControllers();


            app.Run();
        }
    }
}