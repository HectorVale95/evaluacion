USE BdEvaluacion;
GO

-- Verificar si el usuario existe
SELECT 
    Username,
    Nombre,
    ApellidoPaterno,
    ApellidoMaterno,
    Role,
    Activo,
    FechaCreacion
FROM Usuarios
WHERE Username = 'examen@nezter.com';

-- Probar el procedimiento almacenado directamente
EXEC SpdLoginUsuario 
    @Username = 'examen@nezter.com',
    @Password = 'Examen123'; 