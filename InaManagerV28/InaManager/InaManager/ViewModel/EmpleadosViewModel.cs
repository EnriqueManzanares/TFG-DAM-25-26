using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using InaManager.Models;
using InaManager.Repositories;

namespace InaManager.ViewModel
{
    public class EmpleadosViewModel : ViewModelBase
    {
        // --- Repositorios ---
        private MysqlEmpleadoRepository _empleadoRepository;
        private MysqlJugadorRepository _jugadorRepository;

        // --- Colecciones Principales ---
        private ObservableCollection<EmpleadoModel> _empleados;
        private EmpleadoModel _selectedEmpleado;

        // --- Gestión de Jugadores a Cargo ---
        private ObservableCollection<JugadorModel> _jugadoresAsignados;
        private List<JugadorModel> _catalogoJugadores;
        private JugadorModel _jugadorParaAsignar;

        // --- Control Visual ---
        private Visibility _isListVisible = Visibility.Visible;
        private Visibility _isDetailVisible = Visibility.Collapsed;
        private bool _isNewEmpleadoMode;

        // --- SISTEMA DE ALERTAS (Igual que en EquipoViewModel) ---
        private bool _isAlertVisible;
        private string _alertMessage;
        private string _alertTitle;
        private string _alertColor; // Rojo para error, Verde para éxito

        public bool IsAlertVisible
        {
            get { return _isAlertVisible; }
            set { _isAlertVisible = value; OnPropertyChanged(nameof(IsAlertVisible)); }
        }

        public string AlertMessage
        {
            get { return _alertMessage; }
            set { _alertMessage = value; OnPropertyChanged(nameof(AlertMessage)); }
        }

        public string AlertTitle
        {
            get { return _alertTitle; }
            set { _alertTitle = value; OnPropertyChanged(nameof(AlertTitle)); }
        }

        public string AlertColor
        {
            get { return _alertColor; }
            set { _alertColor = value; OnPropertyChanged(nameof(AlertColor)); }
        }

        // ==========================================================
        // PROPIEDADES PÚBLICAS
        // ==========================================================

        public ObservableCollection<EmpleadoModel> Empleados
        {
            get { return _empleados; }
            set { _empleados = value; OnPropertyChanged(nameof(Empleados)); }
        }

        public EmpleadoModel SelectedEmpleado
        {
            get { return _selectedEmpleado; }
            set { _selectedEmpleado = value; OnPropertyChanged(nameof(SelectedEmpleado)); }
        }

        public ObservableCollection<JugadorModel> JugadoresAsignados
        {
            get { return _jugadoresAsignados; }
            set { _jugadoresAsignados = value; OnPropertyChanged(nameof(JugadoresAsignados)); }
        }

        public List<JugadorModel> CatalogoJugadores
        {
            get { return _catalogoJugadores; }
            set { _catalogoJugadores = value; OnPropertyChanged(nameof(CatalogoJugadores)); }
        }

        public JugadorModel JugadorParaAsignar
        {
            get { return _jugadorParaAsignar; }
            set { _jugadorParaAsignar = value; OnPropertyChanged(nameof(JugadorParaAsignar)); }
        }

        public Visibility IsListVisible
        {
            get { return _isListVisible; }
            set { _isListVisible = value; OnPropertyChanged(nameof(IsListVisible)); }
        }

        public Visibility IsDetailVisible
        {
            get { return _isDetailVisible; }
            set { _isDetailVisible = value; OnPropertyChanged(nameof(IsDetailVisible)); }
        }

        public bool IsNewEmpleadoMode
        {
            get { return _isNewEmpleadoMode; }
            set { _isNewEmpleadoMode = value; OnPropertyChanged(nameof(IsNewEmpleadoMode)); }
        }

        public List<string> ListaCargos { get; } = new List<string>
        {
            "Director",
            "Manager",
            "Entrenador"
        };

        // ==========================================================
        // COMANDOS
        // ==========================================================
        public ICommand ShowDetailsCommand { get; }
        public ICommand BackToListCommand { get; }
        public ICommand SaveChangesCommand { get; }
        public ICommand NuevoEmpleadoCommand { get; }
        public ICommand AsignarJugadorCommand { get; }
        public ICommand DesasignarJugadorCommand { get; }
        public ICommand EliminarEmpleadoCommand { get; }
        public ICommand CloseAlertCommand { get; } // Comando para cerrar alerta

        // ==========================================================
        // CONSTRUCTOR
        // ==========================================================
        public EmpleadosViewModel()
        {
            _empleadoRepository = new MysqlEmpleadoRepository();
            _jugadorRepository = new MysqlJugadorRepository();

            LoadEmpleados();

            // Carga segura del catálogo
            CatalogoJugadores = _jugadorRepository.GetAll()?.ToList() ?? new List<JugadorModel>();

            ShowDetailsCommand = new ViewModelCommand(ExecuteShowDetails);
            BackToListCommand = new ViewModelCommand(ExecuteBackToList);
            SaveChangesCommand = new ViewModelCommand(ExecuteSaveChanges);
            NuevoEmpleadoCommand = new ViewModelCommand(ExecuteNuevoEmpleado);
            AsignarJugadorCommand = new ViewModelCommand(ExecuteAsignarJugador);
            DesasignarJugadorCommand = new ViewModelCommand(ExecuteDesasignarJugador);
            EliminarEmpleadoCommand = new ViewModelCommand(ExecuteEliminarEmpleado);
            CloseAlertCommand = new ViewModelCommand(ExecuteCloseAlert);
        }

        // ==========================================================
        // MÉTODOS AUXILIARES Y SEGURIDAD
        // ==========================================================

        private string GetCurrentRol()
        {
            if (UserSession.UsuarioActual != null)
            {
                return UserSession.UsuarioActual.Rol;
            }
            return "Invitado";
        }

        private void MostrarAlerta(string titulo, string mensaje, bool esError = true)
        {
            AlertTitle = titulo;
            AlertMessage = mensaje;
            AlertColor = esError ? "#FF5555" : "#4CAF50"; // Rojo o Verde
            IsAlertVisible = true;
        }

        private void ExecuteCloseAlert(object obj)
        {
            IsAlertVisible = false;
        }

        private void LoadEmpleados()
        {
            var list = _empleadoRepository.GetAllEmployees();
            Empleados = new ObservableCollection<EmpleadoModel>(list);
        }

        // ==========================================================
        // LÓGICA DE NAVEGACIÓN Y CRUD
        // ==========================================================

        private void ExecuteNuevoEmpleado(object obj)
        {
            string rol = GetCurrentRol();

            // RESTRICCIÓN: Entrenador y Jugador NO pueden contratar
            if (rol == "Entrenador" || rol == "Jugador")
            {
                MostrarAlerta("ACCESO DENEGADO", "No tienes permisos para contratar personal.\nEsta acción es exclusiva de Dirección y Managers.");
                return;
            }

            IsNewEmpleadoMode = true;

            SelectedEmpleado = new EmpleadoModel
            {
                Url_imagen = "/Images/Empleados/default.png",
                Nombre = "",
                Apellido = "",
                Email = "",
                Username = "temp_user_" + DateTime.Now.Ticks,
                Password = "temp_password",
                Puesto = "Staff",
                Especialidad = "",
                Años_experiencia = 0,
                Telefono = 0,
                Salario = 1000
            };

            JugadoresAsignados = new ObservableCollection<JugadorModel>();

            IsListVisible = Visibility.Collapsed;
            IsDetailVisible = Visibility.Visible;
        }

        private void ExecuteShowDetails(object obj)
        {
            // TODOS pueden ver los detalles, incluso el Entrenador.
            // La restricción está al intentar guardar.
            if (obj is EmpleadoModel empleado)
            {
                IsNewEmpleadoMode = false;
                SelectedEmpleado = empleado;

                var listaJugadores = _empleadoRepository.GetJugadoresPorEmpleado(empleado.Id_empleado);
                JugadoresAsignados = new ObservableCollection<JugadorModel>(listaJugadores);

                IsListVisible = Visibility.Collapsed;
                IsDetailVisible = Visibility.Visible;
            }
        }

        private void ExecuteBackToList(object obj)
        {
            LoadEmpleados();
            IsListVisible = Visibility.Visible;
            IsDetailVisible = Visibility.Collapsed;
            SelectedEmpleado = null;
            JugadorParaAsignar = null;
        }

        private void ExecuteSaveChanges(object obj)
        {
            string rol = GetCurrentRol();

            // RESTRICCIÓN: Entrenador (y Jugador) NO pueden editar datos de nadie
            if (rol == "Entrenador" || rol == "Jugador")
            {
                MostrarAlerta("SOLO LECTURA", "Tu rol no permite modificar los datos del personal.");
                return;
            }

            if (SelectedEmpleado != null)
            {
                // RESTRICCIÓN: Manager no puede cambiar el rol a Director (opcional, pero buena práctica)
                // Aquí solo validamos el guardado general.

                try
                {
                    if (IsNewEmpleadoMode)
                    {
                        _empleadoRepository.AddEmployee(SelectedEmpleado);
                        IsNewEmpleadoMode = false;
                        MostrarAlerta("ÉXITO", "Empleado contratado correctamente.", false);
                    }
                    else
                    {
                        _empleadoRepository.UpdateEmployee(SelectedEmpleado);
                        MostrarAlerta("ÉXITO", "Datos actualizados correctamente.", false);
                    }
                    // No volvemos a la lista automáticamente para que vea la alerta
                    // ExecuteBackToList(null); 
                }
                catch (Exception ex)
                {
                    MostrarAlerta("ERROR BBDD", ex.Message);
                }
            }
        }

        private void ExecuteAsignarJugador(object obj)
        {
            string rol = GetCurrentRol();

            if (rol == "Entrenador" || rol == "Jugador")
            {
                MostrarAlerta("ACCESO DENEGADO", "No puedes modificar las asignaciones de jugadores.");
                return;
            }

            if (SelectedEmpleado == null || JugadorParaAsignar == null) return;

            if (IsNewEmpleadoMode)
            {
                MostrarAlerta("AVISO", "Guarda el empleado antes de asignarle jugadores.");
                return;
            }

            if (JugadoresAsignados.Any(j => j.Id_jugador == JugadorParaAsignar.Id_jugador))
            {
                MostrarAlerta("INFO", "Este jugador ya está asignado.");
                return;
            }

            try
            {
                _empleadoRepository.AsignarJugador(SelectedEmpleado.Id_empleado, JugadorParaAsignar.Id_jugador);
                JugadoresAsignados.Add(JugadorParaAsignar);
                JugadorParaAsignar = null;
                // Opcional: MostrarAlerta("ASIGNADO", "Jugador vinculado.", false);
            }
            catch (Exception ex)
            {
                MostrarAlerta("ERROR", ex.Message);
            }
        }

        private void ExecuteDesasignarJugador(object obj)
        {
            string rol = GetCurrentRol();

            if (rol == "Entrenador" || rol == "Jugador")
            {
                MostrarAlerta("ACCESO DENEGADO", "No puedes modificar las asignaciones de jugadores.");
                return;
            }

            if (obj is JugadorModel jugador && SelectedEmpleado != null)
            {
                if (MessageBox.Show($"¿Desvincular a {jugador.Nombre}?", "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        _empleadoRepository.DesasignarJugador(SelectedEmpleado.Id_empleado, jugador.Id_jugador);
                        JugadoresAsignados.Remove(jugador);
                    }
                    catch (Exception ex)
                    {
                        MostrarAlerta("ERROR", ex.Message);
                    }
                }
            }
        }

        private void ExecuteEliminarEmpleado(object obj)
        {
            string rol = GetCurrentRol();

            // RESTRICCIÓN 1: Entrenador y Jugador NO pueden borrar
            if (rol == "Entrenador" || rol == "Jugador")
            {
                MostrarAlerta("ACCIÓN BLOQUEADA", "No tienes permisos para despedir empleados.");
                return;
            }

            if (obj is EmpleadoModel empleado)
            {
                // RESTRICCIÓN 2: Manager tiene límites
                if (rol == "Manager")
                {
                    if (empleado.Puesto == "Director" || empleado.Puesto == "Manager")
                    {
                        MostrarAlerta("JERARQUÍA", "No puedes despedir a un Director ni a otro Manager.");
                        return;
                    }
                    // Si intentas borrar algo que no sea Entrenador (ej: Fisio), el prompt decía "solo puedes despedir a entrenadores"
                    // Si el puesto es distinto a "Entrenador", bloqueamos.
                    if (empleado.Puesto != "Entrenador")
                    {
                        MostrarAlerta("LIMITACIÓN", "Como Manager, solo tienes potestad para despedir Entrenadores.");
                        return;
                    }
                }

                // Confirmación
                var result = MessageBox.Show(
                    $"¿Estás seguro de que deseas eliminar a {empleado.Nombre} {empleado.Apellido}?",
                    "Confirmar borrado",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _empleadoRepository.DeleteEmployee(empleado.Id_empleado);
                        Empleados.Remove(empleado);
                        MostrarAlerta("BAJA TRAMITADA", "Empleado eliminado del sistema.", false);
                    }
                    catch (Exception ex)
                    {
                        MostrarAlerta("ERROR", "No se pudo eliminar: " + ex.Message);
                    }
                }
            }
        }

        public void AbrirModoPerfil(EmpleadoModel empleadoActual)
        {
            // Este método se llama desde fuera (MainViewModel probablemente), permitimos ver.
            IsNewEmpleadoMode = false;
            SelectedEmpleado = empleadoActual;

            var listaJugadores = _empleadoRepository.GetJugadoresPorEmpleado(empleadoActual.Id_empleado);
            JugadoresAsignados = new ObservableCollection<JugadorModel>(listaJugadores);

            IsListVisible = Visibility.Collapsed;
            IsDetailVisible = Visibility.Visible;
        }
    }
}