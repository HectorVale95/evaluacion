using System.Data.SqlClient;
using System.Text;
using ClbDatEvaluacion;
using ClbNegEvaluacion;
using ClbModEvaluacion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.SetIsOriginAllowed(origin => true) // Permite cualquier origen
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// Configuración de la base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("ERROR: La cadena de conexión no se está cargando desde appsettings.");
}

// Verificar conexión a la base de datos
try
{
    using (var connection = new SqlConnection(connectionString))
    {
        connection.Open();
        Console.WriteLine("Conexión a la base de datos establecida correctamente.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error al conectar con la base de datos: {ex.Message}");
    throw;
}

// Configuración JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? throw new Exception("JWT Secret no configurado"));

Console.WriteLine($"[JWT] Secret: {jwtSettings["Secret"]}");
Console.WriteLine($"[JWT] Issuer: {jwtSettings["Issuer"]}");
Console.WriteLine($"[JWT] Audience: {jwtSettings["Audience"]}");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
    x.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"[JWT] Error de autenticación: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("[JWT] Token validado correctamente");
            return Task.CompletedTask;
        }
    };
});

// Servicios de la aplicación
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sistema de Evaluación", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Registro de servicios
builder.Services.AddSingleton<ClsDatImportacion>(new ClsDatImportacion(connectionString));
builder.Services.AddTransient<ClsNegImportacion>();
builder.Services.AddScoped<ClsDatAuth>(provider => new ClsDatAuth(connectionString));
builder.Services.AddScoped<ClsNegAuth>(provider => new ClsNegAuth(provider.GetRequiredService<IConfiguration>()));

builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .Select(e => new { Name = e.Key, Message = e.Value.Errors.First().ErrorMessage })
            .ToArray();
        return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(errors);
    };
});

var app = builder.Build();

// Pipeline de la aplicación
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Importante: El orden de estos middleware es crucial
app.UseRouting();
app.UseDefaultFiles(); // Agregar soporte para archivos por defecto
app.UseStaticFiles();  // Agregar soporte para archivos estáticos
app.UseCors();        // CORS debe estar entre UseRouting y UseAuthorization

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Configurar respuesta por defecto para favicon.ico
app.MapGet("/favicon.ico", () => Results.NoContent());

app.Run();
