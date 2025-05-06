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
-- Primero eliminamos la tabla si existe
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usuarios]') AND type in (N'U'))
BEGIN
    DROP TABLE Usuarios;
END
GO

-- Ahora creamos la tabla desde cero
CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    PasswordHash VARBINARY(64) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    ApellidoPaterno NVARCHAR(100) NOT NULL,
    ApellidoMaterno NVARCHAR(100),
    Role NVARCHAR(50) DEFAULT 'User',
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

-- Parte 4: Stored Procedures
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SpdLoginUsuario')
    DROP PROCEDURE SpdLoginUsuario
GO

CREATE PROCEDURE SpdLoginUsuario
    @Username NVARCHAR(50),
    @Password NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    -- Declarar variables para la respuesta
    DECLARE @IdUsuario INT, @Nombre NVARCHAR(100), @ApellidoPaterno NVARCHAR(100), 
            @ApellidoMaterno NVARCHAR(100), @Role NVARCHAR(50), @StoredPasswordHash VARBINARY(64);
    
    -- Obtener el hash almacenado de la contraseña
    SELECT 
        @IdUsuario = IdUsuario, 
        @Nombre = Nombre,
        @ApellidoPaterno = ApellidoPaterno,
        @ApellidoMaterno = ApellidoMaterno,
        @Role = Role,
        @StoredPasswordHash = PasswordHash
    FROM Usuarios
    WHERE Username = @Username AND Activo = 1;

    -- Comparar el hash de la contraseña ingresada con el almacenado
    IF @IdUsuario IS NOT NULL AND @StoredPasswordHash = HASHBYTES('SHA2_256', @Password)
    BEGIN
        -- Devolver el usuario si la contraseña es correcta
        SELECT 
            IdUsuario, 
            Username,
            Nombre, 
            ApellidoPaterno, 
            ApellidoMaterno,
            Role
        FROM Usuarios
        WHERE IdUsuario = @IdUsuario;
        RETURN 0;
    END
    ELSE
    BEGIN
        -- Si la autenticación falla, devolver NULL
        RAISERROR ('Usuario o contraseña incorrectos', 16, 1);
        RETURN -1;
    END
END;
GO

-- Parte 5: Insertar datos iniciales
INSERT INTO Usuarios (Username, PasswordHash, Nombre, ApellidoPaterno, ApellidoMaterno, Role, Activo)
VALUES (
    'examen@nezter.com', 
    HASHBYTES('SHA2_256', 'Examen123'),
    'Usuario',
    'De',
    'Prueba',
    'Admin',
    1
);
GO 