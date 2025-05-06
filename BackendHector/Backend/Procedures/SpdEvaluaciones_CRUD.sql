-- Procedimiento para obtener todas las evaluaciones
CREATE PROCEDURE SpdEvaluaciones_GetAll
AS
BEGIN
    SELECT e.IdEvaluacion, e.IdEmpleado, emp.Nombre + ' ' + emp.ApellidoPaterno AS NombreEmpleado,
           e.FechaEvaluacion, e.Productividad, e.Puntualidad, e.CalidadTrabajo,
           e.Comunicacion, e.DisposicionAprender, e.Honestidad, e.Iniciativa,
           e.IntegracionEquipo, e.Comentarios, e.TotalPuntos
    FROM Evaluaciones e
    INNER JOIN Empleados emp ON e.IdEmpleado = emp.IdEmpleado
    WHERE e.Activo = 1
    ORDER BY e.FechaEvaluacion DESC;
END
GO

-- Procedimiento para obtener una evaluación por ID
CREATE PROCEDURE SpdEvaluaciones_GetById
    @IdEvaluacion INT
AS
BEGIN
    SELECT e.IdEvaluacion, e.IdEmpleado, emp.Nombre + ' ' + emp.ApellidoPaterno AS NombreEmpleado,
           e.FechaEvaluacion, e.Productividad, e.Puntualidad, e.CalidadTrabajo,
           e.Comunicacion, e.DisposicionAprender, e.Honestidad, e.Iniciativa,
           e.IntegracionEquipo, e.Comentarios, e.TotalPuntos
    FROM Evaluaciones e
    INNER JOIN Empleados emp ON e.IdEmpleado = emp.IdEmpleado
    WHERE e.IdEvaluacion = @IdEvaluacion AND e.Activo = 1;
END
GO

-- Procedimiento para obtener evaluaciones por empleado
CREATE PROCEDURE SpdEvaluaciones_GetByEmpleado
    @IdEmpleado INT
AS
BEGIN
    SELECT e.IdEvaluacion, e.IdEmpleado, emp.Nombre + ' ' + emp.ApellidoPaterno AS NombreEmpleado,
           e.FechaEvaluacion, e.Productividad, e.Puntualidad, e.CalidadTrabajo,
           e.Comunicacion, e.DisposicionAprender, e.Honestidad, e.Iniciativa,
           e.IntegracionEquipo, e.Comentarios, e.TotalPuntos
    FROM Evaluaciones e
    INNER JOIN Empleados emp ON e.IdEmpleado = emp.IdEmpleado
    WHERE e.IdEmpleado = @IdEmpleado AND e.Activo = 1
    ORDER BY e.FechaEvaluacion DESC;
END
GO

-- Procedimiento para crear una nueva evaluación
CREATE PROCEDURE SpdEvaluaciones_Create
    @IdEmpleado INT,
    @FechaEvaluacion DATE,
    @Productividad INT,
    @Puntualidad INT,
    @CalidadTrabajo INT,
    @Comunicacion INT,
    @DisposicionAprender INT,
    @Honestidad INT,
    @Iniciativa INT,
    @IntegracionEquipo INT,
    @Comentarios VARCHAR(500)
AS
BEGIN
    DECLARE @TotalPuntos INT;
    SET @TotalPuntos = @Productividad + @Puntualidad + @CalidadTrabajo + @Comunicacion +
                      @DisposicionAprender + @Honestidad + @Iniciativa + @IntegracionEquipo;

    INSERT INTO Evaluaciones (IdEmpleado, FechaEvaluacion, Productividad, Puntualidad,
                            CalidadTrabajo, Comunicacion, DisposicionAprender, Honestidad,
                            Iniciativa, IntegracionEquipo, Comentarios, TotalPuntos)
    VALUES (@IdEmpleado, @FechaEvaluacion, @Productividad, @Puntualidad,
            @CalidadTrabajo, @Comunicacion, @DisposicionAprender, @Honestidad,
            @Iniciativa, @IntegracionEquipo, @Comentarios, @TotalPuntos);
    
    SELECT SCOPE_IDENTITY() AS IdEvaluacion;
END
GO

-- Procedimiento para actualizar una evaluación
CREATE PROCEDURE SpdEvaluaciones_Update
    @IdEvaluacion INT,
    @IdEmpleado INT,
    @FechaEvaluacion DATE,
    @Productividad INT,
    @Puntualidad INT,
    @CalidadTrabajo INT,
    @Comunicacion INT,
    @DisposicionAprender INT,
    @Honestidad INT,
    @Iniciativa INT,
    @IntegracionEquipo INT,
    @Comentarios VARCHAR(500)
AS
BEGIN
    DECLARE @TotalPuntos INT;
    SET @TotalPuntos = @Productividad + @Puntualidad + @CalidadTrabajo + @Comunicacion +
                      @DisposicionAprender + @Honestidad + @Iniciativa + @IntegracionEquipo;

    UPDATE Evaluaciones
    SET IdEmpleado = @IdEmpleado,
        FechaEvaluacion = @FechaEvaluacion,
        Productividad = @Productividad,
        Puntualidad = @Puntualidad,
        CalidadTrabajo = @CalidadTrabajo,
        Comunicacion = @Comunicacion,
        DisposicionAprender = @DisposicionAprender,
        Honestidad = @Honestidad,
        Iniciativa = @Iniciativa,
        IntegracionEquipo = @IntegracionEquipo,
        Comentarios = @Comentarios,
        TotalPuntos = @TotalPuntos,
        FechaModificacion = GETDATE()
    WHERE IdEvaluacion = @IdEvaluacion;
END
GO

-- Procedimiento para eliminar (desactivar) una evaluación
CREATE PROCEDURE SpdEvaluaciones_Delete
    @IdEvaluacion INT
AS
BEGIN
    UPDATE Evaluaciones
    SET Activo = 0,
        FechaModificacion = GETDATE()
 