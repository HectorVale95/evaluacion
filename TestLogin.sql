USE BdEvaluacion;
GO

-- Verificar si la tabla existe y tiene datos
SELECT * FROM sys.tables WHERE name = 'Usuarios';
GO

-- Verificar los datos en la tabla
SELECT * FROM Usuarios;
GO

-- Probar el stored procedure
EXEC SpdLoginUsuario @NombreUsuario = 'examen@nezter.com', @Contrasena = 'Examen123';
GO 