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
    public class SupertecnicasViewModel : ViewModelBase
    {
        private ISuperTecnicaRepository _superTecnicaRepository;
        private JugadorModel _jugadorActual;

        // --- VISIBILIDAD Y DATOS ---
        private Visibility _isListVisible;
        private Visibility _isDetailVisible;

        public bool EsModoJugador => _jugadorActual != null;
        public bool EsModoGlobal => _jugadorActual == null;

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

        private ObservableCollection<SuperTecnicaModel> _supertecnicas;
        public ObservableCollection<SuperTecnicaModel> Supertecnicas
        {
            get { return _supertecnicas; }
            set { _supertecnicas = value; OnPropertyChanged(nameof(Supertecnicas)); }
        }

        private SuperTecnicaModel _selectedSupertecnica;
        public SuperTecnicaModel SelectedSupertecnica
        {
            get { return _selectedSupertecnica; }
            set { _selectedSupertecnica = value; OnPropertyChanged(nameof(SelectedSupertecnica)); }
        }

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

        // --- LISTAS PARA COMBOS (Basado en tu ENUM de SQL) ---
        public List<string> ListaTipos { get; } = new List<string> { "Tiro", "Regate", "Defensa", "Parada", "Talento" };
        public List<string> ListaAfinidades { get; } = new List<string> { "Aire", "Bosque", "Fuego", "Montaña", "Neutro" };

        // --- COMANDOS ---
        public ICommand ShowDetailsCommand { get; }
        public ICommand BackToListCommand { get; }
        public ICommand NuevaTecnicaCommand { get; }
        public ICommand SaveChangesCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CloseAlertCommand { get; }

        public SupertecnicasViewModel(JugadorModel jugadorScope = null)
        {
            _superTecnicaRepository = new MysqlSuperTecnicaRepository();
            _jugadorActual = jugadorScope;

            LoadData();

            ShowDetailsCommand = new ViewModelCommand(ExecuteShowDetails);
            BackToListCommand = new ViewModelCommand(ExecuteBackToList);
            NuevaTecnicaCommand = new ViewModelCommand(ExecuteNuevaTecnica);
            SaveChangesCommand = new ViewModelCommand(ExecuteSaveChanges);
            DeleteCommand = new ViewModelCommand(ExecuteDelete);
            CloseAlertCommand = new ViewModelCommand(ExecuteCloseAlert);

            IsListVisible = Visibility.Visible;
            IsDetailVisible = Visibility.Collapsed;
        }

        private void LoadData()
        {
            if (EsModoGlobal)
            {
                var list = _superTecnicaRepository.GetAll();
                Supertecnicas = new ObservableCollection<SuperTecnicaModel>(list);
            }
            else
            {
                // Aquí deberías filtrar por el ID del jugador si tu repo lo soporta
                var list = _superTecnicaRepository.GetAll();
                Supertecnicas = new ObservableCollection<SuperTecnicaModel>(list);
            }
        }

        private string GetCurrentRol()
        {
            if (UserSession.UsuarioActual != null) return UserSession.UsuarioActual.Rol;
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

        private void ExecuteShowDetails(object obj)
        {
            if (obj is SuperTecnicaModel tecnica)
            {
                SelectedSupertecnica = tecnica;
                IsListVisible = Visibility.Collapsed;
                IsDetailVisible = Visibility.Visible;
            }
        }

        private void ExecuteBackToList(object obj)
        {
            IsListVisible = Visibility.Visible;
            IsDetailVisible = Visibility.Collapsed;
            SelectedSupertecnica = null;
        }

        // --- CORRECCIÓN SOLICITADA AQUI ---
        private void ExecuteNuevaTecnica(object obj)
        {
            if (GetCurrentRol() == "Jugador")
            {
                MostrarAlerta("ACCESO DENEGADO", "Como jugador no puedes inventar nuevas supertécnicas en la base de datos.");
                return;
            }

            // Campos actualizados según tu SQL y variables privadas
            SelectedSupertecnica = new SuperTecnicaModel
            {
                Nombre_SuperTecnica = "",
                Tipo_SuperTecnica = "Tiro",      // Default válido del ENUM
                Afinidad_SuperTecnica = "Fuego", // Default válido del ENUM
                Especialidad = "-",              // Valor por defecto SQL
                Potencia = 0
            };
            IsListVisible = Visibility.Collapsed;
            IsDetailVisible = Visibility.Visible;
        }

        private void ExecuteSaveChanges(object obj)
        {
            if (GetCurrentRol() == "Jugador")
            {
                MostrarAlerta("SOLO LECTURA", "No tienes permisos para modificar los datos de las técnicas.");
                return;
            }

            if (SelectedSupertecnica == null) return;

            try
            {
                if (SelectedSupertecnica.Id_SuperTecnica == 0)
                {
                    _superTecnicaRepository.Add(SelectedSupertecnica);
                    MostrarAlerta("ÉXITO", "Nueva supertécnica registrada.", false);
                }
                else
                {
                    _superTecnicaRepository.Update(SelectedSupertecnica);
                    MostrarAlerta("ÉXITO", "Datos de la técnica actualizados.", false);
                }
                ExecuteBackToList(null);
            }
            catch (Exception ex)
            {
                MostrarAlerta("ERROR DE SISTEMA", ex.Message);
            }
        }

        private void ExecuteDelete(object obj)
        {
            if (GetCurrentRol() == "Jugador")
            {
                MostrarAlerta("ACCIÓN BLOQUEADA", "No tienes permisos para eliminar supertécnicas.");
                return;
            }

            if (SelectedSupertecnica == null) return;

            var result = MessageBox.Show("¿Eliminar definitivamente?", "Confirmar", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (EsModoGlobal)
                    {
                        _superTecnicaRepository.Delete(SelectedSupertecnica.Id_SuperTecnica);
                        Supertecnicas.Remove(SelectedSupertecnica);
                        MostrarAlerta("ELIMINADA", "La técnica ha sido borrada.", false);
                    }
                    else
                    {
                        // Lógica para desvincular de jugador si fuera necesario
                    }
                    ExecuteBackToList(null);
                }
                catch (Exception ex) { MostrarAlerta("ERROR", ex.Message); }
            }
        }
    }
}