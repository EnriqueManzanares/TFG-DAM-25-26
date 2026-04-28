using System.Collections.Generic;
using System.Net;
using InaManager.Models;

namespace InaManager.Repositories // He corregido el namespace a Repositories, que es lo habitual
{
    public interface IEmpleadoRepository
    {
        // --- MÉTODOS ANTIGUOS (LOGIN Y CRUD) ---
        bool ValidateEmployee(NetworkCredential credential);
        EmpleadoModel GetEmployeeById(int id);
        EmpleadoModel GetEmployeeByUsername(string username);
        IEnumerable<EmpleadoModel> GetAllEmployees();
        void AddEmployee(EmpleadoModel empleado);
        void UpdateEmployee(EmpleadoModel empleado);
        void DeleteEmployee(int id);

        
        IEnumerable<JugadorModel> GetJugadoresPorEmpleado(int idEmpleado);
        void SetActiveFormation(string username, int idFormacion);
        void AsignarTactica(string username, string rol, int idFormacion);

        EmpleadoModel GetEntrenadorTitular();
    }
}