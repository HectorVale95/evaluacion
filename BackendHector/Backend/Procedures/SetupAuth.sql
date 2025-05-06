-- Crear la tabla Usuarios si no existe
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuarios')
BEGIN
    CREATE TABLE Usuarios (
        IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(50) NOT NULL UNIQUE,
        Nombre NVARCHAR(100) NOT NULL,
        ApellidoPaterno NVARCHAR(100) NOT NULL,
        ApellidoMaterno NVARCHAR(100) NOT NULL,
        PasswordHash VARBINARY(64) NOT NULL
    );
END

-- Crear o modificar el procedimiento almacenado
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SpdLoginUsuario')
    DROP PROCEDURE SpdLoginUsuario;
GO

CREATE PROCEDURE [dbo].[SpdLoginUsuario]
    @Username NVARCHAR(50),
    @Password NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdUsuario INT, @Nombre NVARCHAR(100), @ApellidoPaterno NVARCHAR(100), @ApellidoMaterno NVARCHAR(100), @StoredPasswordHash VARBINARY(64);
    
    SELECT 
        @IdUsuario = IdUsuario, 
        @Nombre = Nombre,
        @ApellidoPaterno = ApellidoPaterno,
        @ApellidoMaterno = ApellidoMaterno,
        @StoredPasswordHash = PasswordHash
    FROM Usuarios
    WHERE Username = @Username;

    IF @IdUsuario IS NOT NULL AND @StoredPasswordHash = HASHBYTES('SHA2_256', @Password)
    BEGIN
        SELECT 
            IdUsuario, 
            Username, 
            Nombre, 
            ApellidoPaterno, 
            ApellidoMaterno
        FROM Usuarios
        WHERE IdUsuario = @IdUsuario;
    END
    ELSE
    BEGIN
        SELECT NULL AS IdUsuario, NULL AS Username, NULL AS Nombre, NULL AS ApellidoPaterno, NULL AS ApellidoMaterno;
    END
END;
GO

-- Insertar un usuario de prueba si no existe
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Username = 'hector@nezzter.com')
BEGIN
    INSERT INTO Usuarios (Username, Nombre, ApellidoPaterno, ApellidoMaterno, PasswordHash)
    VALUES ('hector@nezzter.com', 'Hector', 'Valenzuela', 'Garcia', HASHBYTES('SHA2_256', '123456'));
END 