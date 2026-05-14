using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using InaManager.Models;
using InaManager.Repositories;

namespace InaManager.ViewModel
{
    public class CrearAnuncioViewModel : ViewModelBase
    {
        private readonly IMercadoRepository _mercadoRepo;
        private readonly IJugadorRepository _jugadorRepo;
        private ObservableCollection<JugadorModel> _misJugadores;
        private JugadorModel _jugadorSeleccionado;
        private decimal _precioClub;
        private int _dias = 7;

        public ObservableCollection<JugadorModel> MisJugadores
        {
            get { return _misJugadores; }
            set { _misJugadores = value; OnPropertyChanged(nameof(MisJugadores)); }
        }

        public JugadorModel JugadorSeleccionado
        {
            get { return _jugadorSeleccionado; }
            set 
            { 
                _jugadorSeleccionado = value; 
                OnPropertyChanged(nameof(JugadorSeleccionado));
                OnPropertyChanged(nameof(ClausulaFormato));
            }
        }

        public decimal PrecioClub
        {
            get { return _precioClub; }
            set { _precioClub = value; OnPropertyChanged(nameof(PrecioClub)); }
        }

        public int Dias
        {
            get { return _dias; }
            set { _dias = value; OnPropertyChanged(nameof(Dias)); }
        }

        public string ClausulaFormato
        {
            get 
            { 
                if (_jugadorSeleccionado == null) return "Selecciona un jugador";
                return $"${_jugadorSeleccionado.Clausula_rescision:N2}";
            }
        }

        public ICommand CrearAnuncioCommand { get; private set; }
        public ICommand CancelarCommand { get; private set; }

        public CrearAnuncioViewModel()
        {
            _mercadoRepo = new MysqlMercadoRepository();
            _jugadorRepo = new MysqlJugadorRepository();
            MisJugadores = new ObservableCollection<JugadorModel>();

            CrearAnuncioCommand = new ViewModelCommand(ExecuteCrearAnuncio);
            CancelarCommand = new ViewModelCommand(ExecuteCancelar);

            CargarMisJugadores();
        }

        private void CargarMisJugadores()
        {
            try
            {
                // DEBUG: Verificar valor de UserSession.Id_Equipo
                System.Diagnostics.Debug.WriteLine($"[CrearAnuncioViewModel] UserSession.Id_Equipo = {UserSession.Id_Equipo}");
                System.Diagnostics.Debug.WriteLine($"[CrearAnuncioViewModel] UserSession.UsuarioActual = {UserSession.UsuarioActual?.DisplayName}");

                // Obtener jugadores del equipo del usuario actual
                if (UserSession.Id_Equipo > 0)
                {
                    var jugadores = _jugadorRepo.GetByTeam();
                    System.Diagnostics.Debug.WriteLine($"[CrearAnuncioViewModel] Se obtuvieron {jugadores.Count()} jugadores");
                    MisJugadores.Clear();
                    foreach (var jugador in jugadores)
                    {
                        MisJugadores.Add(jugador);
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[CrearAnuncioViewModel] UserSession.Id_Equipo está en 0 o menor");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cargando jugadores: {ex.Message}");
            }
        }

        private void ExecuteCrearAnuncio(object obj)
        {
            try
            {
                if (UserSession.Id_Equipo <= 0)
                {
                    MessageBox.Show("No se pudo detectar tu equipo. Por favor, cierra sesión y vuelve a iniciar.", 
                                    "Error de sesión", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (MisJugadores.Count == 0)
                {
                    MessageBox.Show("No tienes jugadores en tu equipo para crear anuncios.", 
                                    "Sin jugadores", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (JugadorSeleccionado == null)
                {
                    MessageBox.Show("Por favor selecciona un jugador.", "Validación", 
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Dias <= 0 || Dias > 90)
                {
                    MessageBox.Show("Los días deben estar entre 1 y 90.", "Validación", 
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Crear el anuncio en la BD
                _mercadoRepo.CrearAnuncio(
                    JugadorSeleccionado.Id_jugador,
                    UserSession.Id_Equipo,
                    PrecioClub,
                    Dias
                );

                MessageBox.Show(
                    $"¡Anuncio creado exitosamente!\n\n{JugadorSeleccionado.Nombre} {JugadorSeleccionado.Apellido}\n" +
                    $"Duración: {Dias} días",
                    "Éxito",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // Cerrar la ventana
                if (obj is Window window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error", 
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCancelar(object obj)
        {
            if (obj is Window window)
            {
                window.DialogResult = false;
                window.Close();
            }
        }
    }
}
