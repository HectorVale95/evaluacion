ALTER PROCEDURE [dbo].[SpdLoginUsuario]
    @Username NVARCHAR(50),
    @Password NVARCHAR(100)  -- Recibe la contraseña en texto claro
AS
BEGIN
    SET NOCOUNT ON;

    -- Declarar variables para la respuesta
    DECLARE @IdUsuario INT, @Nombre NVARCHAR(100), @ApellidoPaterno NVARCHAR(100), @ApellidoMaterno NVARCHAR(100), @StoredPasswordHash VARBINARY(64);
    
    -- Obtener el hash almacenado de la contraseña
    SELECT 
        @IdUsuario = IdUsuario, 
        @Nombre = Nombre,
        @ApellidoPaterno = ApellidoPaterno,
        @ApellidoMaterno = ApellidoMaterno,
        @StoredPasswordHash = PasswordHash
    FROM Usuarios
    WHERE Username = @Username;

    -- Comparar el hash de la contraseña ingresada con el almacenado
    IF @IdUsuario IS NOT NULL AND @StoredPasswordHash = HASHBYTES('SHA2_256', @Password)
    BEGIN
        -- Devolver el usuario si la contraseña es correcta
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
        -- Si la autenticación falla, devolver NULL
        SELECT NULL AS IdUsuario, NULL AS Username, NULL AS Nombre, NULL AS ApellidoPaterno, NULL AS ApellidoMaterno;
    END
END;
