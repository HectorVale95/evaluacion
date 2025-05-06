using System;
using System.Collections.Generic;
using ClbModEvaluacion;
using ClbDatEvaluacion;

namespace ClbNegEvaluacion
{
    public class ClsNegEmpleados
    {
        private readonly ClsDatEmpleados _datEmpleados;

        public ClsNegEmpleados(string connectionString)
        {
            _datEmpleados = new ClsDatEmpleados(connectionString);
        }

        public List<ClsModEmpleado> GetAll()
        {
            try
            {
                return _datEmpleados.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los empleados: {ex.Message}");
            }
        }

        public ClsModEmpleado GetById(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("El ID del empleado debe ser mayor que 0");

                return _datEmpleados.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el empleado: {ex.Message}");
            }
        }

        public int Create(ClsModEmpleado empleado)
        {
            try
            {
                ValidateEmpleado(empleado);
                return _datEmpleados.Create(empleado);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el empleado: {ex.Message}");
            }
        }

        public void Update(ClsModEmpleado empleado)
        {
            try
            {
                ValidateEmpleado(empleado);
                _datEmpleados.Update(empleado);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el empleado: {ex.Message}");
            }
        }

        public void Delete(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("El ID del empleado debe ser mayor que 0");

                _datEmpleados.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el empleado: {ex.Message}");
            }
        }

        private void ValidateEmpleado(ClsModEmpleado empleado)
        {
            if (empleado == null)
                throw new ArgumentNullException(nameof(empleado), "El empleado no puede ser nulo");

            if (string.IsNullOrWhiteSpace(empleado.CodigoEmpleado))
                throw new ArgumentException("El código de empleado es requerido");

            if (string.IsNullOrWhiteSpace(empleado.Nombre))
                throw new ArgumentException("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(empleado.ApellidoPaterno))
                throw new ArgumentException("El apellido paterno es requerido");

            if (empleado.FechaNacimiento == DateTime.MinValue)
                throw new ArgumentException("La fecha de nacimiento es requerida");

            if (empleado.FechaInicioContrato == DateTime.MinValue)
                throw new ArgumentException("La fecha de inicio de contrato es requerida");

            if (empleado.IdPuesto <= 0)
                throw new ArgumentException("El ID del puesto debe ser mayor que 0");
        }
    }
} 