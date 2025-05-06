USE BdEvaluacion;
GO

-- Insertar puestos de prueba
INSERT INTO Puestos (NombrePuesto, Activo)
VALUES 
    ('Desarrollador Senior', 1),
    ('Desarrollador Junior', 1),
    ('Analista de Sistemas', 1),
    ('Gerente de Proyecto', 1),
    ('Diseñador UI/UX', 1),
    ('Administrador de Base de Datos', 1),
    ('Tester QA', 1),
    ('Scrum Master', 1);
GO

-- Insertar empleados de prueba
INSERT INTO Empleados (CodigoEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno, FechaNacimiento, FechaInicioContrato, IdPuesto, Activo)
VALUES 
    ('EMP001', 'Juan', 'Pérez', 'García', '1990-05-15', '2020-01-10', 1, 1),
    ('EMP002', 'María', 'López', 'Martínez', '1988-08-22', '2019-03-15', 2, 1),
    ('EMP003', 'Carlos', 'González', 'Sánchez', '1992-11-30', '2021-06-20', 3, 1),
    ('EMP004', 'Ana', 'Rodríguez', 'Fernández', '1995-04-12', '2022-02-01', 4, 1),
    ('EMP005', 'Pedro', 'Martínez', 'Díaz', '1991-07-25', '2020-09-05', 5, 1),
    ('EMP006', 'Laura', 'Sánchez', 'Ruiz', '1993-02-18', '2021-04-10', 6, 1),
    ('EMP007', 'Miguel', 'Fernández', 'Gómez', '1989-10-05', '2019-11-15', 7, 1),
    ('EMP008', 'Sofía', 'Díaz', 'Hernández', '1994-03-28', '2022-01-20', 8, 1),
    ('EMP009', 'David', 'Ruiz', 'Moreno', '1990-12-10', '2020-07-30', 1, 1),
    ('EMP010', 'Elena', 'Gómez', 'Álvarez', '1992-06-15', '2021-03-25', 2, 1);
GO

-- Verificar los datos insertados
SELECT p.IdPuesto, p.NombrePuesto, COUNT(e.IdEmpleado) as TotalEmpleados
FROM Puestos p
LEFT JOIN Empleados e ON p.IdPuesto = e.IdPuesto
GROUP BY p.IdPuesto, p.NombrePuesto
ORDER BY p.IdPuesto;
GO

SELECT e.IdEmpleado, e.CodigoEmpleado, e.Nombre, e.ApellidoPaterno, e.ApellidoMaterno,
       e.FechaNacimiento, e.FechaInicioContrato, p.NombrePuesto
FROM Empleados e
INNER JOIN Puestos p ON e.IdPuesto = p.IdPuesto
WHERE e.Activo = 1
ORDER BY e.IdEmpleado;
GO 