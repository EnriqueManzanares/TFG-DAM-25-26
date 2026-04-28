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
    public class EquipoViewModel : ViewModelBase
    {
        private MysqlJugadorRepository _jugadorRepository;
        private ObservableCollection<JugadorModel> _jugadores;
        private JugadorModel _selectedJugador;

        // --- Estado y Rol ---
        // El rol se obtiene dinámicamente de UserSession

        // --- Técnicas ---
        private ObservableCollection<SuperTecnicaModel> _tecnicasJugador;
        private List<SuperTecnicaModel> _catalogoCompletoCache;
        private List<SuperTecnicaModel> _catalogoFiltrado;
        private SuperTecnicaModel _tecnicaParaAprender;
        private string _textoBusqueda;

        // --- Control de Vistas ---
        private bool _isNewPlayerMode;
        private Visibility _isListVisible = Visibility.Visible;
        private Visibility _isDetailVisible = Visibility.Collapsed;

        // --- SISTEMA DE ALERTAS (La cajita bonita) ---
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

        // --- Propiedades Data Binding ---
        public List<string> ListaAfinidades { get; } = new List<string> { "Aire", "Bosque", "Fuego", "Montaña", "Neutro" };
        public List<string> ListaPosiciones { get; } = new List<string>
        {
            "PR", "LI", "DFCI", "DFC", "DFCD", "LD", "CI",
            "MCDI", "MCDC", "MCDD", "CD", "MI", "MCI", "MCC", "MCD", "MD", "II", "MCO", "ID",
            "EI", "DCI", "DC", "DCD", "ED"
        };

        public ObservableCollection<JugadorModel> Jugadores
        {
            get { return _jugadores; }
            set { _jugadores = value; OnPropertyChanged(nameof(Jugadores)); }
        }

        public ObservableCollection<SuperTecnicaModel> TecnicasJugador
        {
            get { return _tecnicasJugador; }
            set { _tecnicasJugador = value; OnPropertyChanged(nameof(TecnicasJugador)); }
        }

        public List<SuperTecnicaModel> CatalogoTecnicas
        {
            get { return _catalogoFiltrado; }
            set { _catalogoFiltrado = value; OnPropertyChanged(nameof(CatalogoTecnicas)); }
        }

        public SuperTecnicaModel TecnicaParaAprender
        {
            get { return _tecnicaParaAprender; }
            set { _tecnicaParaAprender = value; OnPropertyChanged(nameof(TecnicaParaAprender)); }
        }

        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set
            {
                _textoBusqueda = value;
                OnPropertyChanged(nameof(TextoBusqueda));
                FiltrarCatalogo();
            }
        }

        public JugadorModel SelectedJugador
        {
            get { return _selectedJugador; }
            set { _selectedJugador = value; OnPropertyChanged(nameof(SelectedJugador)); }
        }

        public bool IsNewPlayerMode
        {
            get { return _isNewPlayerMode; }
            set { _isNewPlayerMode = value; OnPropertyChanged(nameof(IsNewPlayerMode)); }
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

        // Comandos
        public ICommand ShowDetailsCommand { get; }
        public ICommand BackToListCommand { get; }
        public ICommand SaveChangesCommand { get; }
        public ICommand AprenderTecnicaCommand { get; }
        public ICommand RetirarTecnicaCommand { get; }
        public ICommand NuevoJugadorCommand { get; }
        public ICommand DeleteJugadorCommand { get; }
        public ICommand CloseAlertCommand { get; }

        public EquipoViewModel()
        {
            _jugadorRepository = new MysqlJugadorRepository();

            LoadJugadores();
            _catalogoCompletoCache = _jugadorRepository.GetCatalogoCompleto() ?? new List<SuperTecnicaModel>();
            CatalogoTecnicas = new List<SuperTecnicaModel>(_catalogoCompletoCache);

            ShowDetailsCommand = new ViewModelCommand(ExecuteShowDetails);
            BackToListCommand = new ViewModelCommand(ExecuteBackToList);
            SaveChangesCommand = new ViewModelCommand(ExecuteSaveChanges);
            AprenderTecnicaCommand = new ViewModelCommand(ExecuteAprenderTecnica);
            RetirarTecnicaCommand = new ViewModelCommand(ExecuteRetirarTecnica);
            NuevoJugadorCommand = new ViewModelCommand(ExecuteNuevoJugador);
            DeleteJugadorCommand = new ViewModelCommand(ExecuteDeleteJugador);
            CloseAlertCommand = new ViewModelCommand(ExecuteCloseAlert);
        }

        // --- MÉTODOS AUXILIARES ---

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

        // --- COMPROBACIONES DE PERMISOS ---

        private void ExecuteNuevoJugador(object obj)
        {
            string rol = GetCurrentRol();
            // REGLA: Jugador y Entrenador NO pueden añadir.
            if (rol == "Jugador" || rol == "Entrenador")
            {
                MostrarAlerta("ACCESO DENEGADO", "No tienes permisos para fichar nuevos jugadores.\nContacta con la dirección.");
                return;
            }

            IsNewPlayerMode = true;
            SelectedJugador = new JugadorModel
            {
                Url_imagen = "/Images/Players/default.png",
                Afinidad = "Neutro",
                Posicion = "MD",
                Debe_cambiar_pass = true
            };
            TecnicasJugador = new ObservableCollection<SuperTecnicaModel>();
            IsListVisible = Visibility.Collapsed;
            IsDetailVisible = Visibility.Visible;
        }

        private void ExecuteDeleteJugador(object obj)
        {
            string rol = GetCurrentRol();
            // REGLA: Jugador y Entrenador NO pueden borrar.
            if (rol == "Jugador" || rol == "Entrenador")
            {
                MostrarAlerta("ACCIÓN BLOQUEADA", "Solo el Manager o Director pueden expulsar jugadores del equipo.");
                return;
            }

            if (obj is JugadorModel jugador)
            {
                var result = MessageBox.Show($"¿Eliminar a {jugador.Apodo}?", "Confirmar", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _jugadorRepository.DeleteJugador(jugador.Id_jugador);
                        Jugadores.Remove(jugador);
                        MostrarAlerta("ÉXITO", "Jugador eliminado correctamente.", false);
                    }
                    catch (Exception ex) { MostrarAlerta("Error", ex.Message); }
                }
            }
        }

        private void ExecuteShowDetails(object obj)
        {
            string rol = GetCurrentRol();
            // REGLA: Jugador NO puede editar (ni siquiera ver detalles para editar desde la lista).
            if (rol == "Jugador")
            {
                MostrarAlerta("SOLO LECTURA", "Tu perfil es de jugador.\nNo tienes permisos para editar los datos de tus compañeros.");
                return;
            }

            // Entrenador, Manager y Director SÍ pueden entrar aquí
            if (obj is JugadorModel jugador)
            {
                AbrirModoPerfil(jugador); // Reutilizamos el método de carga
            }
        }

        // Este es el método que faltaba y que MainViewModel necesita
        public void AbrirModoPerfil(JugadorModel jugador)
        {
            if (jugador == null) return;

            IsNewPlayerMode = false;
            SelectedJugador = jugador;
            var tecnicas = _jugadorRepository.GetTecnicasDeJugador(jugador.Id_jugador);
            TecnicasJugador = new ObservableCollection<SuperTecnicaModel>(tecnicas);

            IsListVisible = Visibility.Collapsed;
            IsDetailVisible = Visibility.Visible;
        }

        private void ExecuteSaveChanges(object obj)
        {
            string rol = GetCurrentRol();
            // Por seguridad, si de alguna forma un jugador llegó aquí (ej: viendo su propio perfil), no dejamos guardar
            if (rol == "Jugador")
            {
                MostrarAlerta("SOLO LECTURA", "No tienes permisos para guardar cambios en la ficha.");
                return;
            }

            if (SelectedJugador != null)
            {
                try
                {
                    if (IsNewPlayerMode)
                    {
                        if (string.IsNullOrWhiteSpace(SelectedJugador.Username) || string.IsNullOrWhiteSpace(SelectedJugador.Password))
                        {
                            MostrarAlerta("Faltan Datos", "Usuario y contraseña requeridos.");
                            return;
                        }
                        _jugadorRepository.AddJugador(SelectedJugador);
                        MostrarAlerta("Éxito", $"¡{SelectedJugador.Apodo} fichado!", false);
                        IsNewPlayerMode = false;
                    }
                    else
                    {
                        _jugadorRepository.UpdateJugador(SelectedJugador);
                        MostrarAlerta("Éxito", "Datos actualizados correctamente.", false);
                    }
                    ExecuteBackToList(null);
                }
                catch (Exception ex)
                {
                    MostrarAlerta("Error BBDD", ex.Message);
                }
            }
        }

        private void ExecuteAprenderTecnica(object obj)
        {
            string rol = GetCurrentRol();
            if (rol == "Jugador")
            {
                MostrarAlerta("ACCESO DENEGADO", "Solo el cuerpo técnico puede asignar entrenamientos.");
                return;
            }

            if (SelectedJugador != null && TecnicaParaAprender != null)
            {
                if (IsNewPlayerMode) { MostrarAlerta("Aviso", "Guarda el jugador antes de asignar técnicas."); return; }
                if (TecnicasJugador.Any(t => t.Id_SuperTecnica == TecnicaParaAprender.Id_SuperTecnica))
                {
                    MostrarAlerta("Información", "El jugador ya conoce esta técnica.");
                    return;
                }

                try
                {
                    _jugadorRepository.AsignarTecnica(SelectedJugador.Id_jugador, TecnicaParaAprender.Id_SuperTecnica);
                    TecnicasJugador.Add(TecnicaParaAprender);
                    TextoBusqueda = string.Empty;
                    TecnicaParaAprender = null;
                }
                catch (Exception ex) { MostrarAlerta("Error", ex.Message); }
            }
        }

        private void ExecuteRetirarTecnica(object obj)
        {
            string rol = GetCurrentRol();
            if (rol == "Jugador") return;

            if (obj is SuperTecnicaModel tecnica && SelectedJugador != null)
            {
                try
                {
                    _jugadorRepository.RetirarTecnica(SelectedJugador.Id_jugador, tecnica.Id_SuperTecnica);
                    TecnicasJugador.Remove(tecnica);
                }
                catch (Exception ex) { MostrarAlerta("Error", ex.Message); }
            }
        }

        private void LoadJugadores()
        {
            var list = _jugadorRepository.GetAll();
            Jugadores = new ObservableCollection<JugadorModel>(list);
        }

        private void FiltrarCatalogo()
        {
            if (_catalogoCompletoCache == null || !_catalogoCompletoCache.Any())
                _catalogoCompletoCache = _jugadorRepository.GetCatalogoCompleto() ?? new List<SuperTecnicaModel>();

            if (string.IsNullOrWhiteSpace(TextoBusqueda))
                CatalogoTecnicas = new List<SuperTecnicaModel>(_catalogoCompletoCache);
            else
            {
                var busquedaLower = TextoBusqueda.ToLower();
                CatalogoTecnicas = _catalogoCompletoCache
                    .Where(t => t.Nombre_SuperTecnica != null && t.Nombre_SuperTecnica.ToLower().Contains(busquedaLower))
                    .ToList();
            }
        }

        private void ExecuteBackToList(object obj)
        {
            LoadJugadores();
            IsListVisible = Visibility.Visible;
            IsDetailVisible = Visibility.Collapsed;
            IsNewPlayerMode = false;
        }
    }
}