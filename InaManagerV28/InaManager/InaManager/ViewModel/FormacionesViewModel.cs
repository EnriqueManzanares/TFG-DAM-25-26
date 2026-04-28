using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using InaManager.Helpers;
using InaManager.Models;
using InaManager.Repositories;

namespace InaManager.ViewModel
{
    public class CeldaTablero : ViewModelBase
    {
        private string _codigo;
        public int Fila { get; set; }
        public int Columna { get; set; }
        public bool IsLocked { get; set; }

        public string Codigo
        {
            get { return _codigo; }
            set
            {
                _codigo = value;
                OnPropertyChanged(nameof(Codigo));
                OnPropertyChanged(nameof(IsOccupied));
                OnPropertyChanged(nameof(DisplayText));
            }
        }
        public bool IsOccupied => !string.IsNullOrEmpty(Codigo);
        public string DisplayText => IsOccupied ? Codigo : "";
    }

    public class FormacionesViewModel : ViewModelBase
    {
        private MysqlFormacionRepository _repository;
        private IEmpleadoRepository _empleadoRepository;

        // CAMBIO 1: Inicializamos la colección aquí y no la volvemos a hacer 'new' nunca
        private ObservableCollection<FormacionConEntrenadorModel> _formaciones = new ObservableCollection<FormacionConEntrenadorModel>();
        private ObservableCollection<CeldaTablero> _cuadricula;
        private FormacionConEntrenadorModel _selectedFormacion;

        private Visibility _isListVisible = Visibility.Visible;
        private Visibility _isDetailVisible = Visibility.Collapsed;
        private string _nombreFormacionEdicion;
        private bool _isNewMode;

        public ObservableCollection<FormacionConEntrenadorModel> Formaciones
        {
            get => _formaciones;
            set { _formaciones = value; OnPropertyChanged(nameof(Formaciones)); }
        }

        public ObservableCollection<CeldaTablero> Cuadricula
        {
            get => _cuadricula;
            set { _cuadricula = value; OnPropertyChanged(nameof(Cuadricula)); }
        }

        public FormacionConEntrenadorModel SelectedFormacion
        {
            get => _selectedFormacion;
            set { _selectedFormacion = value; OnPropertyChanged(nameof(SelectedFormacion)); }
        }

        public Visibility IsListVisible
        {
            get => _isListVisible;
            set { _isListVisible = value; OnPropertyChanged(nameof(IsListVisible)); }
        }

        public Visibility IsDetailVisible
        {
            get => _isDetailVisible;
            set { _isDetailVisible = value; OnPropertyChanged(nameof(IsDetailVisible)); }
        }

        public string NombreFormacionEdicion
        {
            get => _nombreFormacionEdicion;
            set { _nombreFormacionEdicion = value; OnPropertyChanged(nameof(NombreFormacionEdicion)); }
        }

        public ICommand ShowDetailsCommand { get; }
        public ICommand CreateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CellClickCommand { get; }
        public ICommand AsignarFormacionCommand { get; }

        public FormacionesViewModel()
        {
            _repository = new MysqlFormacionRepository();
            _empleadoRepository = new MysqlEmpleadoRepository();

            LoadFormaciones();
            InicializarCuadricula();

            ShowDetailsCommand = new ViewModelCommand(ExecuteShowDetails);
            CreateCommand = new ViewModelCommand(ExecuteCreate);
            DeleteCommand = new ViewModelCommand(ExecuteDelete);

            BackCommand = new ViewModelCommand(obj => {
                SelectedFormacion = null;
                IsDetailVisible = Visibility.Collapsed;
                IsListVisible = Visibility.Visible;
            });

            SaveCommand = new ViewModelCommand(ExecuteSave);
            CellClickCommand = new ViewModelCommand(ExecuteCellClick);
            AsignarFormacionCommand = new ViewModelCommand(ExecuteAsignar, CanExecuteAsignar);
        }

        private bool CanExecuteAsignar(object obj)
        {
            if (_isNewMode) return false;
            var principal = Thread.CurrentPrincipal;
            if (principal == null) return false;
            return principal.IsInRole("Entrenador") || principal.IsInRole("Director");
        }

        private void ExecuteAsignar(object obj)
        {
            if (obj is int idFormacion)
            {
                var principal = Thread.CurrentPrincipal;
                string currentUsername = principal?.Identity?.Name;
                if (string.IsNullOrEmpty(currentUsername)) return;

                string rolUsuario = principal.IsInRole("Director") ? "Director" : "Entrenador";

                try
                {
                    _empleadoRepository.AsignarTactica(currentUsername, rolUsuario, idFormacion);
                    MessageBox.Show($"La formación ha sido activada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadFormaciones();
                    SelectedFormacion = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al asignar la formación: {ex.Message}", "Error");
                }
            }
        }

        // CAMBIO 2: Método LoadFormaciones reescrito
        // En vez de reemplazar la lista, la limpiamos y rellenamos.
        // Esto evita que el DataGrid pierda el norte visualmente.
        private void LoadFormaciones()
        {
            var lista = _repository.GetAll().Cast<FormacionConEntrenadorModel>().ToList();

            Formaciones.Clear();
            foreach (var item in lista)
            {
                Formaciones.Add(item);
            }
        }

        private void InicializarCuadricula()
        {
            Cuadricula = new ObservableCollection<CeldaTablero>();
            for (int r = 0; r < 6; r++)
                for (int c = 0; c < 5; c++)
                    Cuadricula.Add(new CeldaTablero { Fila = r, Columna = c });
        }

        private void ExecuteShowDetails(object obj)
        {
            if (obj is FormacionConEntrenadorModel formacion)
            {
                SelectedFormacion = formacion;
                _isNewMode = false;
                NombreFormacionEdicion = formacion.Nombre;

                LimpiarTablero();

                foreach (var codigoPos in formacion.Posiciones)
                    ColocarFichaEnTablero(codigoPos);

                IsListVisible = Visibility.Collapsed;
                IsDetailVisible = Visibility.Visible;
            }
        }

        private void ExecuteCreate(object obj)
        {
            SelectedFormacion = new FormacionConEntrenadorModel();
            _isNewMode = true;
            NombreFormacionEdicion = "Nueva Formación";

            LimpiarTablero();
            ColocarFichaEnTablero("PR");

            IsListVisible = Visibility.Collapsed;
            IsDetailVisible = Visibility.Visible;
        }

        private void LimpiarTablero()
        {
            foreach (var celda in Cuadricula)
            {
                celda.Codigo = null;
                celda.IsLocked = false;
            }
        }

        private void ColocarFichaEnTablero(string codigo)
        {
            if (MapeoPosiciones.Coordenadas.ContainsKey(codigo))
            {
                var coords = MapeoPosiciones.Coordenadas[codigo];
                var celda = Cuadricula.FirstOrDefault(c => c.Fila == coords.Fila && c.Columna == coords.Columna);
                if (celda != null)
                {
                    celda.Codigo = codigo;
                    if (codigo == "PR") celda.IsLocked = true;
                }
            }
        }

        private void ExecuteCellClick(object obj)
        {
            if (obj is CeldaTablero celda)
            {
                if (celda.IsLocked) return;

                if (celda.IsOccupied)
                {
                    celda.Codigo = null;
                }
                else
                {
                    if (Cuadricula.Count(c => c.IsOccupied) >= 11) return;

                    string pos = MapeoPosiciones.ObtenerPosicionDesdeCoordenada(celda.Fila, celda.Columna);
                    if (pos != null) celda.Codigo = pos;
                }
            }
        }

        private void ExecuteSave(object obj)
        {
            var fichas = Cuadricula.Where(c => c.IsOccupied).Select(c => c.Codigo).ToList();

            if (fichas.Count != 11) { MessageBox.Show("Deben ser 11 jugadores."); return; }
            if (!fichas.Contains("PR")) { MessageBox.Show("Falta el Portero."); return; }

            SelectedFormacion.Nombre = NombreFormacionEdicion;
            SelectedFormacion.Posiciones = fichas;

            try
            {
                if (_isNewMode) _repository.Add(SelectedFormacion);
                else _repository.Update(SelectedFormacion);

                LoadFormaciones();

                SelectedFormacion = null;

                IsDetailVisible = Visibility.Collapsed;
                IsListVisible = Visibility.Visible;
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void ExecuteDelete(object obj)
        {
            if (obj is FormacionConEntrenadorModel f && MessageBox.Show("¿Borrar?", "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _repository.Delete(f.Id_formacion);
                LoadFormaciones();
            }
        }
    }
}