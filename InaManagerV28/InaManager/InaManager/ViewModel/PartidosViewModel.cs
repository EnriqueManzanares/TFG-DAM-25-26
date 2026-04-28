using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using InaManager.Models;
using InaManager.Repositories;

namespace InaManager.ViewModel
{
    public class PartidosViewModel : ViewModelBase
    {
        private MysqlPartidoRepository _partidoRepo;
        private MysqlEntrenoRepository _entrenoRepo;

        // --- LISTAS FIJAS (HARDCODED) ---
        public List<string> TiposEvento { get; } = new List<string> { "Entrenamiento", "Partido" };

        // Aquí están las opciones tal cual las tienes en el ENUM de SQL
        public List<string> ListaCompeticiones { get; } = new List<string> { "Amistoso", "Fútbol Frontier", "Mundial" };

        public ObservableCollection<SelectorSimple> ComboJugadores { get; set; }
        public ObservableCollection<SelectorSimple> ComboEjercicios { get; set; }
        public ObservableCollection<SelectorSimple> ComboEmpleados { get; set; }

        // --- CAMPOS FORMULARIO ---
        private string _formTipo;
        public string FormTipo { get => _formTipo; set { _formTipo = value; OnPropertyChanged(nameof(FormTipo)); } }

        private DateTime _formFecha;
        public DateTime FormFecha { get => _formFecha; set { _formFecha = value; OnPropertyChanged(nameof(FormFecha)); } }

        private string _formRival;
        public string FormRival { get => _formRival; set { _formRival = value; OnPropertyChanged(nameof(FormRival)); } }

        private string _formCompeticion;
        public string FormCompeticion { get => _formCompeticion; set { _formCompeticion = value; OnPropertyChanged(nameof(FormCompeticion)); } }

        private SelectorSimple _selectedJugador;
        public SelectorSimple SelectedJugador { get => _selectedJugador; set { _selectedJugador = value; OnPropertyChanged(nameof(SelectedJugador)); } }

        private SelectorSimple _selectedEjercicio;
        public SelectorSimple SelectedEjercicio { get => _selectedEjercicio; set { _selectedEjercicio = value; OnPropertyChanged(nameof(SelectedEjercicio)); } }

        private SelectorSimple _selectedEmpleado;
        public SelectorSimple SelectedEmpleado { get => _selectedEmpleado; set { _selectedEmpleado = value; OnPropertyChanged(nameof(SelectedEmpleado)); } }

        private string _formComentarios;
        public string FormComentarios { get => _formComentarios; set { _formComentarios = value; OnPropertyChanged(nameof(FormComentarios)); } }

        // --- VISTA ---
        private ObservableCollection<AgendaItemModel> _todosLosEventos;
        public ObservableCollection<AgendaItemModel> TodosLosEventos
        {
            get => _todosLosEventos;
            set { _todosLosEventos = value; OnPropertyChanged(nameof(TodosLosEventos)); }
        }
        public ObservableCollection<AgendaItemModel> EventosVisibles { get; set; }

        private string _textoFecha;
        public string TextoFecha { get => _textoFecha; set { _textoFecha = value; OnPropertyChanged(nameof(TextoFecha)); } }

        private bool _esDiaLibre;
        public bool EsDiaLibre { get => _esDiaLibre; set { _esDiaLibre = value; OnPropertyChanged(nameof(EsDiaLibre)); } }

        private DateTime _fechaSeleccionada;
        public DateTime FechaSeleccionada
        {
            get => _fechaSeleccionada;
            set { _fechaSeleccionada = value; OnPropertyChanged(nameof(FechaSeleccionada)); FiltrarPorFecha(value); }
        }

        private bool _isCreatorVisible;
        public bool IsCreatorVisible { get => _isCreatorVisible; set { _isCreatorVisible = value; OnPropertyChanged(nameof(IsCreatorVisible)); } }

        // --- ALERTAS (Propiedades para el Popup) ---
        private bool _isAlertVisible;
        public bool IsAlertVisible { get => _isAlertVisible; set { _isAlertVisible = value; OnPropertyChanged(nameof(IsAlertVisible)); } }

        private string _alertMessage;
        public string AlertMessage { get => _alertMessage; set { _alertMessage = value; OnPropertyChanged(nameof(AlertMessage)); } }

        private string _alertTitle;
        public string AlertTitle { get => _alertTitle; set { _alertTitle = value; OnPropertyChanged(nameof(AlertTitle)); } }

        private string _alertColor;
        public string AlertColor { get => _alertColor; set { _alertColor = value; OnPropertyChanged(nameof(AlertColor)); } }

        // --- COMANDOS ---
        public ICommand AbrirCreadorCommand { get; }
        public ICommand CerrarCreadorCommand { get; }
        public ICommand GuardarEventoCommand { get; }
        public ICommand EliminarEventoCommand { get; }
        public ICommand UpdateItemCommand { get; }
        public ICommand CloseAlertCommand { get; }
        public ICommand ToggleExpandCommand { get; }

        public PartidosViewModel()
        {
            _partidoRepo = new MysqlPartidoRepository();
            _entrenoRepo = new MysqlEntrenoRepository();

            EventosVisibles = new ObservableCollection<AgendaItemModel>();
            ComboJugadores = new ObservableCollection<SelectorSimple>();
            ComboEjercicios = new ObservableCollection<SelectorSimple>();
            ComboEmpleados = new ObservableCollection<SelectorSimple>();

            FechaSeleccionada = DateTime.Today;

            AbrirCreadorCommand = new ViewModelCommand(ExecuteAbrirCreador);
            CerrarCreadorCommand = new ViewModelCommand(ExecuteCerrarCreador);
            GuardarEventoCommand = new ViewModelCommand(ExecuteGuardarEvento);
            EliminarEventoCommand = new ViewModelCommand(ExecuteEliminarEvento);
            UpdateItemCommand = new ViewModelCommand(ExecuteUpdateItem);
            CloseAlertCommand = new ViewModelCommand(ExecuteCloseAlert);
            ToggleExpandCommand = new ViewModelCommand(ExecuteToggleExpand);

            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                var listaTemp = new List<AgendaItemModel>();

                // 1. Partidos
                listaTemp.AddRange(_partidoRepo.ObtenerAgendaPartidos());

                // 2. Entrenos (Filtro por Rol)
                string rol = UserSession.UsuarioActual?.Rol;
                int idUsuario = UserSession.UsuarioActual?.IdUsuario ?? 0;

                List<AgendaItemModel> entrenos = null;

                if (rol == "Jugador")
                    entrenos = _entrenoRepo.ObtenerEntrenosAgendaPorJugador(idUsuario);
                else
                    entrenos = _entrenoRepo.ObtenerEntrenosAgenda();

                if (entrenos != null) listaTemp.AddRange(entrenos);

                TodosLosEventos = new ObservableCollection<AgendaItemModel>(listaTemp);
                FiltrarPorFecha(FechaSeleccionada);

                if (rol != "Jugador") CargarSelectores();
            }
            catch (Exception ex) { MostrarAlerta("ERROR CARGA", ex.Message); }
        }

        private void CargarSelectores()
        {
            ComboJugadores.Clear();
            foreach (var j in _entrenoRepo.ObtenerSelectorJugadores()) ComboJugadores.Add(j);
            ComboEjercicios.Clear();
            foreach (var ej in _entrenoRepo.ObtenerSelectorEjercicios()) ComboEjercicios.Add(ej);
            ComboEmpleados.Clear();
            foreach (var em in _entrenoRepo.ObtenerSelectorEmpleados()) ComboEmpleados.Add(em);
        }

        private void FiltrarPorFecha(DateTime fecha)
        {
            TextoFecha = fecha.ToString("dddd, dd MMMM yyyy").ToUpper();
            if (TodosLosEventos == null) return;
            var fechaOnly = DateOnly.FromDateTime(fecha);
            var filtrados = TodosLosEventos.Where(e => e.Fecha == fechaOnly).OrderBy(e => e.Tipo).ToList();
            EventosVisibles.Clear();
            foreach (var e in filtrados) EventosVisibles.Add(e);
            EsDiaLibre = EventosVisibles.Count == 0;
        }

        // --- ACCIONES ---

        private void ExecuteAbrirCreador(object obj)
        {
            if (UserSession.UsuarioActual?.Rol == "Jugador")
            {
                MostrarAlerta("ACCESO DENEGADO", "Solo el staff técnico puede agendar eventos.");
                return;
            }

            FormFecha = FechaSeleccionada;
            FormTipo = "Entrenamiento";
            FormRival = "";
            FormCompeticion = "Amistoso"; // Valor por defecto hardcodeado
            FormComentarios = "";
            SelectedJugador = null;
            SelectedEjercicio = null;
            SelectedEmpleado = null;

            IsCreatorVisible = true;
        }

        private void ExecuteGuardarEvento(object obj)
        {
            string rol = UserSession.UsuarioActual?.Rol;

            if (FormTipo == "Partido")
            {
                if (rol == "Entrenador") { MostrarAlerta("PERMISOS", "El Entrenador no puede organizar Partidos."); return; }
                if (string.IsNullOrWhiteSpace(FormRival)) { MostrarAlerta("FALTAN DATOS", "Debes indicar el Rival."); return; }

                _partidoRepo.InsertarPartidoCompleto(FormFecha, FormRival, FormCompeticion);
                MostrarAlerta("ÉXITO", "Partido creado correctamente.", false);
            }
            else
            {
                if (SelectedJugador == null || SelectedEjercicio == null) { MostrarAlerta("FALTAN DATOS", "Selecciona Jugador y Ejercicio."); return; }

                int idEmp = SelectedEmpleado?.Id ?? (UserSession.UsuarioActual?.IdUsuario ?? 0);

                _entrenoRepo.InsertarEntrenoCompleto(SelectedJugador.Id, SelectedEjercicio.Id, idEmp, FormFecha, FormComentarios);
                MostrarAlerta("ÉXITO", "Entrenamiento guardado.", false);
            }
            IsCreatorVisible = false;
            CargarDatos();
        }

        private void ExecuteUpdateItem(object obj)
        {
            string rol = UserSession.UsuarioActual?.Rol;
            if (rol == "Jugador") { MostrarAlerta("SOLO LECTURA", "No puedes editar datos."); CargarDatos(); return; }

            if (obj is AgendaItemModel item)
            {
                try
                {
                    if (item.Tipo == "Partido")
                    {
                        if (rol == "Entrenador") { MostrarAlerta("ACCESO DENEGADO", "Solo Manager/Director editan partidos."); CargarDatos(); return; }

                        _partidoRepo.ActualizarResultado(item.Id, item.GolesLocal, item.GolesVisitante, item.Competicion);
                        MostrarAlerta("ACTUALIZADO", "Marcador guardado.", false);
                    }
                    else
                    {
                        _entrenoRepo.ActualizarComentarios(item.Id, item.Comentarios);
                        MostrarAlerta("ACTUALIZADO", "Comentarios actualizados.", false);
                    }
                }
                catch (Exception ex) { MostrarAlerta("ERROR", ex.Message); }
            }
        }

        private void ExecuteEliminarEvento(object obj)
        {
            if (UserSession.UsuarioActual?.Rol == "Jugador") { MostrarAlerta("DENEGADO", "No puedes borrar eventos."); return; }
            if (obj is AgendaItemModel item)
            {
                if (UserSession.UsuarioActual?.Rol == "Entrenador" && item.Tipo == "Partido") { MostrarAlerta("DENEGADO", "No puedes borrar partidos."); return; }
                if (MessageBox.Show("¿Eliminar evento?", "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        if (item.Tipo == "Partido") _partidoRepo.EliminarPartidoPorId(item.Id);
                        else _entrenoRepo.Eliminar(item.Id);
                        CargarDatos();
                    }
                    catch (Exception ex) { MostrarAlerta("ERROR", ex.Message); }
                }
            }
        }

        private void ExecuteToggleExpand(object obj) { if (obj is AgendaItemModel item) item.IsExpanded = !item.IsExpanded; }
        private void ExecuteCerrarCreador(object obj) => IsCreatorVisible = false;
        private void ExecuteCloseAlert(object obj) => IsAlertVisible = false;

        private void MostrarAlerta(string titulo, string mensaje, bool error = true)
        {
            AlertTitle = titulo;
            AlertMessage = mensaje;
            AlertColor = error ? "#FF5252" : "#4CAF50"; // Rojo (Error) o Verde (Éxito)
            IsAlertVisible = true;
        }
    }
}