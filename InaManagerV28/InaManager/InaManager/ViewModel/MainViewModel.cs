using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using InaManager.Models;
using InaManager.Repositories;

namespace InaManager.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        // Campos existentes
        private EmpleadoAccountModel _userAccount;
        private ViewModelBase _currentChildView;
        private IEmpleadoRepository _empleadoRepository;
        private IJugadorRepository _jugadorRepository;

        // NUEVO CAMPO: Controla la visibilidad
        private bool _isAdminMenuVisible;

        // Propiedades
        public EmpleadoAccountModel UserAccount
        {
            get { return _userAccount; }
            set
            {
                _userAccount = value;
                OnPropertyChanged(nameof(UserAccount));
            }
        }

        public ViewModelBase CurrentChildView
        {
            get { return _currentChildView; }
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        // NUEVA PROPIEDAD: Se vinculará a la vista para ocultar botones
        public bool IsAdminMenuVisible
        {
            get { return _isAdminMenuVisible; }
            set
            {
                _isAdminMenuVisible = value;
                OnPropertyChanged(nameof(IsAdminMenuVisible));
            }
        }

        // Comandos de Navegación
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowEquipoViewCommand { get; }
        public ICommand ShowPlantillaViewCommand { get; }
        public ICommand ShowPartidosViewCommand { get; }
        public ICommand ShowSupertecnicasViewCommand { get; }
        public ICommand ShowSponsorsViewCommand { get; }
        public ICommand ShowEmpleadosViewCommand { get; }
        public ICommand ShowFormacionesViewCommand { get; }
        public ICommand ShowEjerciciosViewCommand { get; }
        public ICommand ShowPerfilViewCommand { get; }

        public MainViewModel()
        {
            _empleadoRepository = new MysqlEmpleadoRepository();
            _jugadorRepository = new MysqlJugadorRepository();

            // Inicializar Comandos
            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeView);
            ShowEquipoViewCommand = new ViewModelCommand(ExecuteShowEquipoView);
            ShowPlantillaViewCommand = new ViewModelCommand(ExecuteShowPlantillaView);
            ShowPartidosViewCommand = new ViewModelCommand(ExecuteShowPartidosView);
            ShowSupertecnicasViewCommand = new ViewModelCommand(ExecuteShowSupertecnicasView);
            ShowSponsorsViewCommand = new ViewModelCommand(ExecuteShowSponsorsView);
            ShowEmpleadosViewCommand = new ViewModelCommand(ExecuteShowEmpleadosView);
            ShowFormacionesViewCommand = new ViewModelCommand(ExecuteShowFormacionesView);
            ShowEjerciciosViewCommand = new ViewModelCommand(ExecuteShowEjerciciosView);
            ShowPerfilViewCommand = new ViewModelCommand(ExecuteShowPerfilView);

            // Carga inicial
            ExecuteShowHomeView(null);
            LoadCurrentUserData();
        }

        // --- MÉTODOS DE NAVEGACIÓN ---
        private void ExecuteShowHomeView(object obj) => CurrentChildView = new HomeViewModel();
        private void ExecuteShowEquipoView(object obj) => CurrentChildView = new EquipoViewModel();
        private void ExecuteShowPlantillaView(object obj) => CurrentChildView = new PlantillaViewModel();
        private void ExecuteShowPartidosView(object obj) => CurrentChildView = new PartidosViewModel();
        private void ExecuteShowSupertecnicasView(object obj) => CurrentChildView = new SupertecnicasViewModel();
        private void ExecuteShowSponsorsView(object obj) => CurrentChildView = new SponsorsViewModel();
        private void ExecuteShowEmpleadosView(object obj) => CurrentChildView = new EmpleadosViewModel();
        private void ExecuteShowFormacionesView(object obj) => CurrentChildView = new FormacionesViewModel();
        private void ExecuteShowEjerciciosView(object obj) => CurrentChildView = new EjerciciosViewModel();

        // --- LÓGICA DE PERFIL ---
        private void ExecuteShowPerfilView(object obj)
        {
            string username = Thread.CurrentPrincipal?.Identity?.Name;

            var empleado = _empleadoRepository.GetEmployeeByUsername(username);
            if (empleado != null)
            {
                var empleadosVM = new EmpleadosViewModel();
                empleadosVM.AbrirModoPerfil(empleado);
                CurrentChildView = empleadosVM;
                return;
            }

            var jugador = _jugadorRepository.GetJugadorByUsername(username);
            if (jugador != null)
            {
                var equipoVM = new EquipoViewModel();
                equipoVM.AbrirModoPerfil(jugador);
                CurrentChildView = equipoVM;
            }
        }

        // --- CARGA DE DATOS ---
        private void LoadCurrentUserData()
        {
            string username = Thread.CurrentPrincipal?.Identity?.Name;

            if (string.IsNullOrEmpty(username))
            {
                UserAccount = new EmpleadoAccountModel { DisplayName = "Invitado", Rol = "Ninguno" };
                IsAdminMenuVisible = false; // Por seguridad
                return;
            }

            // 1. Intentar cargar como Empleado
            var empleado = _empleadoRepository.GetEmployeeByUsername(username);
            if (empleado != null)
            {
                UserAccount = new EmpleadoAccountModel
                {
                    IdUsuario = empleado.Id_empleado,
                    Username = empleado.Username,
                    DisplayName = $"{empleado.Nombre} {empleado.Apellido}",
                    Rol = empleado.Puesto,
                    ProfilePicture = empleado.Url_imagen
                };

                // ES EMPLEADO: PUEDE VER TODO EL MENÚ
                IsAdminMenuVisible = true;
                return;
            }

            // 2. Intentar cargar como Jugador
            var jugador = _jugadorRepository.GetJugadorByUsername(username);
            if (jugador != null)
            {
                UserAccount = new EmpleadoAccountModel
                {
                    IdUsuario = jugador.Id_jugador,
                    Username = jugador.Username,
                    DisplayName = $"{jugador.Nombre} {jugador.Apellido}",
                    Rol = "Jugador",
                    ProfilePicture = jugador.Url_imagen
                };

                // ES JUGADOR: OCULTAMOS LAS OPCIONES ADMINISTRATIVAS
                IsAdminMenuVisible = false;
            }
        }
    }
}