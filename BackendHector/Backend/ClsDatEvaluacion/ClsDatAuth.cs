using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ClbModEvaluacion;
using System;

namespace ClbDatEvaluacion
{
    public class ClsDatAuth
    {
        private readonly string _connectionString;

        public ClsDatAuth(string connectionString)
        {
            _connectionString = connectionString;
            Console.WriteLine($"ClsDatAuth inicializado con conexión: {connectionString}");
        }

        public async Task<ClsModUsuario> LoginAsync(string username, string password)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();

                    var query = "SpdLoginUsuario";
                    var parameters = new { NombreUsuario = username, Contrasena = password };

                    return await dbConnection.QueryFirstOrDefaultAsync<ClsModUsuario>(query, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en LoginAsync: {ex.Message}");
                throw new Exception("Error al intentar iniciar sesión", ex);
            }
        }

        public ClsModUsuario ValidateUser(string username, string password)
        {
            try
            {
                Console.WriteLine("========== INICIO VALIDACIÓN DE USUARIO ==========");
                Console.WriteLine($"Username recibido: '{username}'");
                Console.WriteLine($"Password recibido: '{password}'");
                
                using (var connection = new SqlConnection(_connectionString))
                {
                    Console.WriteLine("Abriendo conexión...");
                    connection.Open();
                    Console.WriteLine("Conexión abierta correctamente");

                    // Primero verificar si el usuario existe
                    var checkUserQuery = "SELECT COUNT(1) FROM Usuarios WHERE NombreUsuario = @NombreUsuario";
                    var userExists = connection.ExecuteScalar<int>(checkUserQuery, new { NombreUsuario = username });
                    Console.WriteLine($"¿Usuario existe? {userExists > 0}");

                    // Verificar los datos exactos del usuario
                    var userData = connection.QueryFirstOrDefault<dynamic>(
                        "SELECT NombreUsuario, Contrasena FROM Usuarios WHERE NombreUsuario = @NombreUsuario",
                        new { NombreUsuario = username }
                    );
                    
                    if (userData != null)
                    {
                        Console.WriteLine($"Usuario encontrado en BD:");
                        Console.WriteLine($"NombreUsuario en BD: '{userData.NombreUsuario}'");
                        Console.WriteLine($"Contrasena en BD: '{userData.Contrasena}'");
                    }
                    else
                    {
                        Console.WriteLine("No se encontró el usuario en la base de datos");
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@NombreUsuario", username);
                    parameters.Add("@Contrasena", password);

                    Console.WriteLine("Ejecutando SpdLoginUsuario con parámetros:");
                    Console.WriteLine($"@NombreUsuario: '{username}'");
                    Console.WriteLine($"@Contrasena: '{password}'");

                    var result = connection.QueryFirstOrDefault<ClsModUsuario>(
                        "SpdLoginUsuario",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null)
                    {
                        Console.WriteLine("No se encontró el usuario o las credenciales son incorrectas");
                        throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");
                    }

                    Console.WriteLine("Resultado de la autenticación:");
                    Console.WriteLine($"IdUsuario: {result.IdUsuario}");
                    Console.WriteLine($"Username: '{result.Username}'");
                    Console.WriteLine($"Role: '{result.Role}'");
                    Console.WriteLine("========== FIN VALIDACIÓN DE USUARIO ==========");

                    return result;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error de SQL: {ex.Message}");
                Console.WriteLine($"Número de error: {ex.Number}");
                Console.WriteLine($"Procedimiento: {ex.Procedure}");
                Console.WriteLine($"Estado: {ex.State}");
                Console.WriteLine($"LineNumber: {ex.LineNumber}");
                throw new Exception($"Error de base de datos: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
                throw new Exception("Error al validar el usuario", ex);
            }
        }

        public async Task<string> CrearUsuarioAsync(string username, string password, string nombre, string apellidoPaterno, string apellidoMaterno)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();

                    var query = "SpdCrearUsuario";
                    var parameters = new
                    {
                        NombreUsuario = username,
                        Contrasena = password,
                        Nombre = nombre,
                        ApellidoPaterno = apellidoPaterno,
                        ApellidoMaterno = apellidoMaterno
                    };

                    string result = await dbConnection.QueryFirstOrDefaultAsync<string>(query, parameters, commandType: CommandType.StoredProcedure);

                    if (!string.IsNullOrEmpty(result) && result.Contains("ya está en uso"))
                    {
                        throw new Exception(result);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CrearUsuarioAsync: {ex.Message}");
                throw new Exception("Error al intentar crear el usuario", ex);
            }
        }

        private byte[] ConvertToVarBinary(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
