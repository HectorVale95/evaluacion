CREATE TABLE Evaluaciones (
    IdEvaluacion INT IDENTITY(1,1) PRIMARY KEY,
    IdEmpleado INT NOT NULL,
    FechaEvaluacion DATE NOT NULL,
    Productividad INT NOT NULL,
    Puntualidad INT NOT NULL,
    CalidadTrabajo INT NOT NULL,
    Comunicacion INT NOT NULL,
    DisposicionAprender INT NOT NULL,
    Honestidad INT NOT NULL,
    Iniciativa INT NOT NULL,
    IntegracionEquipo INT NOT NULL,
    Comentarios VARCHAR(500),
    TotalPuntos INT NOT NULL,
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FechaModificacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (IdEmpleado) REFERENCES Empleados(IdEmpleado)
); 