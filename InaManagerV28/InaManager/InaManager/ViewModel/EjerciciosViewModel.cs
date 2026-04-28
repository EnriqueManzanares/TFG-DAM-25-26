using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using InaManager.Models;
using InaManager.Repositories;

namespace InaManager.ViewModel
{
    public class EjerciciosViewModel : ViewModelBase
    {
        private MysqlEjercicioRepository _repository;

        // --- SISTEMA DE ALERTAS ---
        private bool _isAlertVisible;
        private string _alertMessage;
        private string _alertTitle;
        private string _alertColor;

        public bool IsAlertVisible
        {
            get { return _isAlertVisible; }
            set { _isAlertVisible = value; OnPropertyChanged(nameof(IsAlertVisible)); }
        }
        public string AlertMessage { get => _alertMessage; set { _alertMessage = value; OnPropertyChanged(nameof(AlertMessage)); } }
        public string AlertTitle { get => _alertTitle; set { _alertTitle = value; OnPropertyChanged(nameof(AlertTitle)); } }
        public string AlertColor { get => _alertColor; set { _alertColor = value; OnPropertyChanged(nameof(AlertColor)); } }

        // --- VISIBILIDAD Y DATOS ---
        private Visibility _isListVisible = Visibility.Visible;
        public Visibility IsListVisible
        {
            get => _isListVisible;
            set { _isListVisible = value; OnPropertyChanged(nameof(IsListVisible)); }
        }

        private Visibility _isDetailVisible = Visibility.Collapsed;
        public Visibility IsDetailVisible
        {
            get => _isDetailVisible;
            set { _isDetailVisible = value; OnPropertyChanged(nameof(IsDetailVisible)); }
        }

        public ObservableCollection<EjercicioModel> ListaEjercicios { get; set; }

        private EjercicioModel _selectedEjercicio;
        public EjercicioModel SelectedEjercicio
        {
            get => _selectedEjercicio;
            set { _selectedEjercicio = value; OnPropertyChanged(nameof(SelectedEjercicio)); }
        }

        private string _tituloDetalle;
        public string TituloDetalle
        {
            get => _tituloDetalle;
            set { _tituloDetalle = value; OnPropertyChanged(nameof(TituloDetalle)); }
        }

        // Comandos declarados
        public ICommand AddNewCommand { get; set; }
        public ICommand ShowDetailsCommand { get; set; }
        public ICommand BackToListCommand { get; set; }
        public ICommand SaveChangesCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand DeleteFromGridCommand { get; set; }
        public ICommand CloseAlertCommand { get; set; }

        public EjerciciosViewModel()
        {
            _repository = new MysqlEjercicioRepository();
            ListaEjercicios = new ObservableCollection<EjercicioModel>();

            // 1. INICIALIZAMOS LOS COMANDOS
            AddNewCommand = new ViewModelCommand(ExecuteAddNewCommand);
            ShowDetailsCommand = new ViewModelCommand(ExecuteShowDetailsCommand);
            BackToListCommand = new ViewModelCommand(ExecuteBackToListCommand);
            SaveChangesCommand = new ViewModelCommand(ExecuteSaveChangesCommand);
            DeleteCommand = new ViewModelCommand(ExecuteDeleteCommand);
            DeleteFromGridCommand = new ViewModelCommand(ExecuteDeleteFromGridCommand);
            CloseAlertCommand = new ViewModelCommand(ExecuteCloseAlert);

            CargarEjercicios();
        }

        // --- MÉTODOS DE SEGURIDAD ---
        private string GetCurrentRol()
        {
            if (UserSession.UsuarioActual != null)
                return UserSession.UsuarioActual.Rol;
            return "Invitado";
        }

        private void MostrarAlerta(string titulo, string mensaje, bool esError = true)
        {
            AlertTitle = titulo;
            AlertMessage = mensaje;
            AlertColor = esError ? "#FF5555" : "#4CAF50";
            IsAlertVisible = true;
        }

        private void ExecuteCloseAlert(object obj) => IsAlertVisible = false;

        private void CargarEjercicios()
        {
            ListaEjercicios.Clear();
            var datos = _repository.ObtenerTodos();
            foreach (var item in datos)
            {
                ListaEjercicios.Add(item);
            }
        }

        // --- LÓGICA DE LOS BOTONES ---

        // Botón "+ NUEVO EJERCICIO"
        private void ExecuteAddNewCommand(object obj)
        {
            // RESTRICCIÓN: Jugador no puede crear
            if (GetCurrentRol() == "Jugador")
            {
                MostrarAlerta("ACCESO DENEGADO", "No tienes permisos para crear ejercicios tácticos.");
                return;
            }

            SelectedEjercicio = new EjercicioModel(); // Creamos uno vacío
            TituloDetalle = "NUEVO EJERCICIO";
            IsListVisible = Visibility.Collapsed;
            IsDetailVisible = Visibility.Visible;
        }

        // Botón "EDITAR" (de la tabla)
        private void ExecuteShowDetailsCommand(object obj)
        {
            if (obj is EjercicioModel ejercicio)
            {
                // Hacemos una copia para no editar directamente en la lista hasta dar a Guardar
                SelectedEjercicio = new EjercicioModel
                {
                    id_ejercicio = ejercicio.id_ejercicio,
                    nombre = ejercicio.nombre,
                    categoria = ejercicio.categoria,
                    descripcion = ejercicio.descripcion
                };
                TituloDetalle = "EDITAR EJERCICIO";
                IsListVisible = Visibility.Collapsed;
                IsDetailVisible = Visibility.Visible;
            }
        }

        // Botón "VOLVER / CANCELAR"
        private void ExecuteBackToListCommand(object obj)
        {
            IsListVisible = Visibility.Visible;
            IsDetailVisible = Visibility.Collapsed;
        }

        // Botón "GUARDAR DATOS"
        private void ExecuteSaveChangesCommand(object obj)
        {
            // RESTRICCIÓN: Jugador no puede guardar
            if (GetCurrentRol() == "Jugador")
            {
                MostrarAlerta("SOLO LECTURA", "No tienes permisos para modificar el catálogo de ejercicios.");
                return;
            }

            try
            {
                if (SelectedEjercicio != null)
                {
                    if (SelectedEjercicio.id_ejercicio == 0)
                    {
                        // Si el ID es 0, es que es nuevo
                        _repository.Insertar(SelectedEjercicio);
                        MostrarAlerta("ÉXITO", "Nuevo ejercicio registrado.", false);
                    }
                    else
                    {
                        // Si ya tiene ID, lo actualizamos
                        _repository.Actualizar(SelectedEjercicio);
                        MostrarAlerta("ÉXITO", "Datos del ejercicio actualizados.", false);
                    }
                    CargarEjercicios(); // Recargamos la tabla
                    ExecuteBackToListCommand(null); // Volvemos a la lista
                }
            }
            catch (Exception ex)
            {
                MostrarAlerta("ERROR BBDD", ex.Message);
            }
        }

        // Botón "BORRAR / DESCARTAR" (dentro del formulario)
        private void ExecuteDeleteCommand(object obj)
        {
            // RESTRICCIÓN: Jugador no puede borrar
            if (GetCurrentRol() == "Jugador")
            {
                MostrarAlerta("ACCIÓN BLOQUEADA", "No tienes permisos para eliminar ejercicios.");
                return;
            }

            if (SelectedEjercicio != null && SelectedEjercicio.id_ejercicio != 0)
            {
                var result = MessageBox.Show($"¿Seguro que quieres eliminar '{SelectedEjercicio.nombre}'?", "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _repository.Eliminar(SelectedEjercicio.id_ejercicio);
                        CargarEjercicios();
                        MostrarAlerta("ELIMINADO", "Ejercicio borrado del sistema.", false);
                    }
                    catch (Exception ex) { MostrarAlerta("ERROR", ex.Message); }
                }
            }
            ExecuteBackToListCommand(null);
        }

        // Botón "ELIMINAR" (directo en la tabla)
        private void ExecuteDeleteFromGridCommand(object obj)
        {
            // RESTRICCIÓN: Jugador no puede borrar
            if (GetCurrentRol() == "Jugador")
            {
                MostrarAlerta("ACCIÓN BLOQUEADA", "No tienes permisos para eliminar ejercicios.");
                return;
            }

            if (obj is EjercicioModel ejercicio)
            {
                var result = MessageBox.Show($"¿Seguro que quieres eliminar el ejercicio '{ejercicio.nombre}'?", "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _repository.Eliminar(ejercicio.id_ejercicio);
                        CargarEjercicios();
                        MostrarAlerta("ELIMINADO", "Ejercicio borrado del sistema.", false);
                    }
                    catch (Exception ex) { MostrarAlerta("ERROR", ex.Message); }
                }
            }
        }
    }
}