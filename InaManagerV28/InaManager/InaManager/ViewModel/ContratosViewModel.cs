using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using InaManager.Models;
using InaManager.Repositories;

namespace InaManager.ViewModel
{
    public class ContratosViewModel : ViewModelBase
    {
        private IJugadorRepository _jugadorRepository;
        private IEmpleadoRepository _empleadoRepository;

        // --- COLECCIONES ---
        private ObservableCollection<JugadorModel> _jugadores;
        public ObservableCollection<JugadorModel> Jugadores
        {
            get { return _jugadores; }
            set { _jugadores = value; OnPropertyChanged(nameof(Jugadores)); }
        }

        private ObservableCollection<JugadorModel> _jugadoresFiltrados;
        public ObservableCollection<JugadorModel> JugadoresFiltrados
        {
            get { return _jugadoresFiltrados; }
            set { _jugadoresFiltrados = value; OnPropertyChanged(nameof(JugadoresFiltrados)); }
        }

        private ObservableCollection<EmpleadoModel> _empleados;
        public ObservableCollection<EmpleadoModel> Empleados
        {
            get { return _empleados; }
            set { _empleados = value; OnPropertyChanged(nameof(Empleados)); }
        }

        private ObservableCollection<EmpleadoModel> _empleadosFiltrados;
        public ObservableCollection<EmpleadoModel> EmpleadosFiltrados
        {
            get { return _empleadosFiltrados; }
            set { _empleadosFiltrados = value; OnPropertyChanged(nameof(EmpleadosFiltrados)); }
        }

        // --- PESTAÑAS (TABS) ---
        private bool _isJugadoresTabActive = true;
        public bool IsJugadoresTabActive
        {
            get { return _isJugadoresTabActive; }
            set
            {
                _isJugadoresTabActive = value;
                OnPropertyChanged(nameof(IsJugadoresTabActive));
                OnPropertyChanged(nameof(IsEmpleadosTabActive));
            }
        }
        public bool IsEmpleadosTabActive => !IsJugadoresTabActive;

        // --- FILTROS JUGADORES ---
        private string _searchTextJugador;
        public string SearchTextJugador
        {
            get { return _searchTextJugador; }
            set { _searchTextJugador = value; OnPropertyChanged(nameof(SearchTextJugador)); FiltrarJugadores(); }
        }

        private string _filtroPosicion = "Todas";
        public string FiltroPosicion
        {
            get { return _filtroPosicion; }
            set { _filtroPosicion = value; OnPropertyChanged(nameof(FiltroPosicion)); FiltrarJugadores(); }
        }

        private string _filtroDisponibilidad = "Todos";
        public string FiltroDisponibilidad
        {
            get { return _filtroDisponibilidad; }
            set { _filtroDisponibilidad = value; OnPropertyChanged(nameof(FiltroDisponibilidad)); FiltrarJugadores(); }
        }

        // --- FILTROS EMPLEADOS ---
        private string _searchTextEmpleado;
        public string SearchTextEmpleado
        {
            get { return _searchTextEmpleado; }
            set { _searchTextEmpleado = value; OnPropertyChanged(nameof(SearchTextEmpleado)); FiltrarEmpleados(); }
        }

        private string _filtroPuesto = "Todos";
        public string FiltroPuesto
        {
            get { return _filtroPuesto; }
            set { _filtroPuesto = value; OnPropertyChanged(nameof(FiltroPuesto)); FiltrarEmpleados(); }
        }

        // --- COMANDOS ---
        public ICommand SwitchToJugadoresCommand { get; }
        public ICommand SwitchToEmpleadosCommand { get; }
        public ICommand GuardarJugadoresCommand { get; }
        public ICommand GuardarEmpleadosCommand { get; }
        public ICommand CloseAlertCommand { get; }

        // --- ALERTA ---
        private string _alertTitle;
        public string AlertTitle { get => _alertTitle; set { _alertTitle = value; OnPropertyChanged(nameof(AlertTitle)); } }

        private string _alertMessage;
        public string AlertMessage { get => _alertMessage; set { _alertMessage = value; OnPropertyChanged(nameof(AlertMessage)); } }

        private string _alertColor;
        public string AlertColor { get => _alertColor; set { _alertColor = value; OnPropertyChanged(nameof(AlertColor)); } }

        private bool _isAlertVisible;
        public bool IsAlertVisible { get => _isAlertVisible; set { _isAlertVisible = value; OnPropertyChanged(nameof(IsAlertVisible)); } }

        public ContratosViewModel()
        {
            _jugadorRepository = new MysqlJugadorRepository();
            _empleadoRepository = new MysqlEmpleadoRepository();

            SwitchToJugadoresCommand = new ViewModelCommand(o => IsJugadoresTabActive = true);
            SwitchToEmpleadosCommand = new ViewModelCommand(o => IsJugadoresTabActive = false);
            
            GuardarJugadoresCommand = new ViewModelCommand(ExecuteGuardarJugadores);
            GuardarEmpleadosCommand = new ViewModelCommand(ExecuteGuardarEmpleados);
            CloseAlertCommand = new ViewModelCommand(o => IsAlertVisible = false);

            LoadData();
        }

        private void LoadData()
        {
            var jList = _jugadorRepository.GetAll().ToList();
            Jugadores = new ObservableCollection<JugadorModel>(jList);
            JugadoresFiltrados = new ObservableCollection<JugadorModel>(jList);

            var eList = _empleadoRepository.GetAllEmployees().ToList();
            Empleados = new ObservableCollection<EmpleadoModel>(eList);
            EmpleadosFiltrados = new ObservableCollection<EmpleadoModel>(eList);
            
            OnPropertyChanged(nameof(TotalMasaSalarialJugadores));
            OnPropertyChanged(nameof(TotalMasaSalarialEmpleados));
        }

        private void FiltrarJugadores()
        {
            if (Jugadores == null) return;

            var filtrados = Jugadores.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchTextJugador))
            {
                filtrados = filtrados.Where(j => 
                    j.Nombre.IndexOf(SearchTextJugador, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    j.Apellido.IndexOf(SearchTextJugador, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    j.Apodo.IndexOf(SearchTextJugador, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (FiltroPosicion != "Todas")
            {
                // Simplificación: PR, DF, MD, DL
                if (FiltroPosicion == "Porteros") filtrados = filtrados.Where(j => j.Posicion.Contains("PR"));
                else if (FiltroPosicion == "Defensas") filtrados = filtrados.Where(j => j.Posicion.Contains("DF") || j.Posicion.Contains("LI") || j.Posicion.Contains("LD") || j.Posicion == "CI" || j.Posicion == "CD");
                else if (FiltroPosicion == "Medios") filtrados = filtrados.Where(j => j.Posicion.Contains("MC") || j.Posicion.Contains("MI") || j.Posicion.Contains("MD"));
                else if (FiltroPosicion == "Delanteros") filtrados = filtrados.Where(j => j.Posicion.Contains("DC") || j.Posicion.Contains("EI") || j.Posicion.Contains("ED"));
            }

            if (FiltroDisponibilidad == "Transferibles")
                filtrados = filtrados.Where(j => j.Esta_disponible);
            else if (FiltroDisponibilidad == "Intransferibles")
                filtrados = filtrados.Where(j => !j.Esta_disponible);

            JugadoresFiltrados = new ObservableCollection<JugadorModel>(filtrados);
        }

        private void FiltrarEmpleados()
        {
            if (Empleados == null) return;

            var filtrados = Empleados.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchTextEmpleado))
            {
                filtrados = filtrados.Where(e => 
                    e.Nombre.IndexOf(SearchTextEmpleado, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    e.Apellido.IndexOf(SearchTextEmpleado, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    e.Username.IndexOf(SearchTextEmpleado, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (FiltroPuesto != "Todos")
            {
                filtrados = filtrados.Where(e => e.Puesto == FiltroPuesto);
            }

            EmpleadosFiltrados = new ObservableCollection<EmpleadoModel>(filtrados);
        }

        private void ExecuteGuardarJugadores(object obj)
        {
            // Guardamos todos los jugadores filtrados (asumiendo que son los que se ven y se pueden editar)
            foreach (var j in Jugadores)
            {
                _jugadorRepository.UpdateJugador(j);
            }
            OnPropertyChanged(nameof(TotalMasaSalarialJugadores));
            MostrarAlerta("Éxito", "Los contratos de los jugadores se han guardado correctamente.", false);
        }

        private void ExecuteGuardarEmpleados(object obj)
        {
            string username = Thread.CurrentPrincipal?.Identity?.Name;
            var currentEmpleado = _empleadoRepository.GetEmployeeByUsername(username);
            bool isManager = currentEmpleado != null && currentEmpleado.Puesto == "Manager";
            bool hasUnauthorizedEdits = false;

            foreach (var e in Empleados)
            {
                if (isManager && (e.Puesto == "Manager" || e.Puesto == "Director"))
                {
                    var dbEmp = _empleadoRepository.GetEmployeeById(e.Id_empleado);
                    if (e.Salario != dbEmp.Salario)
                    {
                        hasUnauthorizedEdits = true;
                        e.Salario = dbEmp.Salario; 
                    }
                }
                _empleadoRepository.UpdateEmployee(e);
            }
            OnPropertyChanged(nameof(TotalMasaSalarialEmpleados));

            if (hasUnauthorizedEdits)
            {
                MostrarAlerta("Advertencia", "No tienes permisos para modificar el salario de otros Managers ni de Directores. El resto de datos se ha guardado correctamente.", true);
            }
            else
            {
                MostrarAlerta("Éxito", "Los salarios de los empleados se han guardado correctamente.", false);
            }
        }

        private void MostrarAlerta(string titulo, string mensaje, bool esError = true)
        {
            AlertTitle = titulo;
            AlertMessage = mensaje;
            AlertColor = esError ? "#FF4B4B" : "#2E7D32"; // Rojo o Verde
            IsAlertVisible = true;
        }

        // Totales para la vista
        public decimal TotalMasaSalarialJugadores => Jugadores?.Sum(j => j.Sueldo) ?? 0;
        public decimal TotalMasaSalarialEmpleados => Empleados?.Sum(e => e.Salario) ?? 0;
    }
}
