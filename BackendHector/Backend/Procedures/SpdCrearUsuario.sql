ALTER PROCEDURE [dbo].[SpdCrearUsuario]
    @Username NVARCHAR(50),
    @Password NVARCHAR(100),  -- Recibe la contraseña en texto claro
    @Nombre NVARCHAR(100),
    @ApellidoPaterno NVARCHAR(100),
    @ApellidoMaterno NVARCHAR(100),
    @Mensaje NVARCHAR(100) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar si el usuario ya existe
    IF EXISTS (SELECT 1 FROM Usuarios WHERE Username = @Username)
    BEGIN
        SET @Mensaje = 'El nombre de usuario ya está en uso.';
        RETURN;
    END

    -- Hashear la contraseña dentro del SP
    DECLARE @PasswordHash VARBINARY(64);
    SET @PasswordHash = HASHBYTES('SHA2_256', @Password);

    -- Insertar el nuevo usuario
    INSERT INTO Usuarios (Username, PasswordHash, Nombre, ApellidoPaterno, ApellidoMaterno)
    VALUES (@Username, @PasswordHash, @Nombre, @ApellidoPaterno, @ApellidoMaterno);

    SET @Mensaje = 'Usuario creado exitosamente.';
END;
