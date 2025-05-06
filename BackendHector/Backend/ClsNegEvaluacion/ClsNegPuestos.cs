using System;
using System.Collections.Generic;
using ClbModEvaluacion;
using ClbDatEvaluacion;

namespace ClbNegEvaluacion
{
    public class ClsNegPuestos
    {
        private readonly ClsDatPuestos _datPuestos;

        public ClsNegPuestos(string connectionString)
        {
            _datPuestos = new ClsDatPuestos(connectionString);
        }

        public List<ClsModPuesto> GetAll()
        {
            try
            {
                return _datPuestos.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los puestos: {ex.Message}");
            }
        }

        public ClsModPuesto GetById(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("El ID del puesto debe ser mayor que 0");

                return _datPuestos.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el puesto: {ex.Message}");
            }
        }

        public int Create(ClsModPuesto puesto)
        {
            try
            {
                ValidatePuesto(puesto);
                return _datPuestos.Create(puesto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el puesto: {ex.Message}");
            }
        }

        public void Update(ClsModPuesto puesto)
        {
            try
            {
                ValidatePuesto(puesto);
                _datPuestos.Update(puesto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el puesto: {ex.Message}");
            }
        }

        public void Delete(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("El ID del puesto debe ser mayor que 0");

                _datPuestos.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el puesto: {ex.Message}");
            }
        }

        private void ValidatePuesto(ClsModPuesto puesto)
        {
            if (puesto == null)
                throw new ArgumentNullException(nameof(puesto), "El puesto no puede ser nulo");

            if (string.IsNullOrWhiteSpace(puesto.NombrePuesto))
                throw new ArgumentException("El nombre del puesto es requerido");
        }
    }
} 