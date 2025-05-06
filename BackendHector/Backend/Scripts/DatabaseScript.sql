-- Parte 1: Crear la base de datos
USE master;
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'BdEvaluacion')
BEGIN
    CREATE DATABASE BdEvaluacion;
END;
GO

USE BdEvaluacion;
GO

-- Parte 2: Configurar permisos de usuario
-- Crear login si no existe (para Windows Authentication)
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'HectorVlz\hecto')
BEGIN
    CREATE LOGIN [HectorVlz\hecto] FROM WINDOWS;
END
GO

-- Crear usuario en la base de datos y asignar permisos
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'HectorVlz\hecto')
BEGIN
    CREATE USER [HectorVlz\hecto] FOR LOGIN [HectorVlz\hecto];
    ALTER ROLE db_owner ADD MEMBER [HectorVlz\hecto];
END
GO

-- Parte 3: Creación de tablas
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usuarios]') AND type in (N'U'))
BEGIN
    DROP TABLE Usuarios;
END
GO

CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario NVARCHAR(50) NOT NULL UNIQUE,
    Contrasena NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Nombre NVARCHAR(50) NOT NULL,
    ApellidoPaterno NVARCHAR(50) NOT NULL,
    ApellidoMaterno NVARCHAR(50) NOT NULL,
    Role NVARCHAR(50) NOT NULL DEFAULT 'User',
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Parte 4: Stored Procedures
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SpdLoginUsuario')
    DROP PROCEDURE SpdLoginUsuario
GO

CREATE PROCEDURE SpdLoginUsuario
    @NombreUsuario NVARCHAR(50),
    @Contrasena NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Imprimir los parámetros recibidos para debugging
    PRINT 'Parámetros recibidos:';
    PRINT 'NombreUsuario: ' + ISNULL(@NombreUsuario, 'NULL');
    PRINT 'Contrasena: ' + ISNULL(@Contrasena, 'NULL');

    -- Verificar si existe el usuario con esas credenciales
    IF NOT EXISTS (
        SELECT 1 
        FROM Usuarios 
        WHERE NombreUsuario = @NombreUsuario 
        AND Contrasena = @Contrasena 
        AND Activo = 1
    )
    BEGIN
        PRINT 'Usuario o contraseña incorrectos';
        RAISERROR ('Usuario o contraseña incorrectos', 16, 1);
        RETURN;
    END

    -- Si llegamos aquí, el usuario existe y las credenciales son correctas
    PRINT 'Usuario encontrado, devolviendo datos...';
    
    -- Asegurarnos de que los nombres de las columnas coincidan exactamente con las propiedades de C#
    SELECT 
        IdUsuario,
        NombreUsuario as Username,  -- Mapeo explícito a la propiedad C#
        Nombre,
        ApellidoPaterno,
        ApellidoMaterno,
        Role,
        Activo,
        FechaCreacion
    FROM Usuarios WITH(NOLOCK)
    WHERE NombreUsuario = @NombreUsuario
    AND Contrasena = @Contrasena
    AND Activo = 1;

    -- Verificar que se devolvieron datos
    IF @@ROWCOUNT > 0
        PRINT 'Datos devueltos correctamente'
    ELSE
        PRINT 'No se devolvieron datos'
END;
GO

-- Limpiar datos anteriores
PRINT 'Limpiando datos anteriores...';
DELETE FROM Usuarios WHERE NombreUsuario = 'examen@nezter.com';
GO

-- Insertar usuario de prueba con todos los campos
PRINT 'Insertando usuario de prueba...';
INSERT INTO Usuarios (
    NombreUsuario, 
    Contrasena, 
    Email, 
    Nombre, 
    ApellidoPaterno, 
    ApellidoMaterno, 
    Role,
    Activo,
    FechaCreacion
)
VALUES (
    'examen@nezter.com',  -- NombreUsuario
    'Examen123',          -- Contrasena
    'examen@nezter.com',  -- Email
    'Usuario',            -- Nombre
    'De',                -- ApellidoPaterno
    'Prueba',            -- ApellidoMaterno
    'Admin',             -- Role
    1,                   -- Activo
    GETDATE()            -- FechaCreacion
);
GO

-- Verificar la inserción
PRINT 'Verificando datos insertados...';
SELECT 
    'Datos del usuario:' as Mensaje,
    IdUsuario,
    NombreUsuario,
    Contrasena,
    Email,
    Nombre,
    ApellidoPaterno,
    ApellidoMaterno,
    Role,
    Activo,
    FechaCreacion
FROM Usuarios 
WHERE NombreUsuario = 'examen@nezter.com';
GO

-- Probar el stored procedure
PRINT 'Probando el stored procedure con credenciales correctas...';
EXEC SpdLoginUsuario @NombreUsuario = 'examen@nezter.com', @Contrasena = 'Examen123';
GO

PRINT 'Probando el stored procedure con credenciales incorrectas...';
EXEC SpdLoginUsuario @NombreUsuario = 'examen@nezter.com', @Contrasena = 'ContraseñaIncorrecta';
GO 