using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows; // Necesario para Visibility
using System.Windows.Input;
using InaManager.Models;
using InaManager.Repositories;

namespace InaManager.ViewModel
{
    public class CeldaEsquema : ViewModelBase
    {
        private JugadorModel _jugador;
        private bool _esPosicionValida;
        private string _nombrePosicion;

        public int Row { get; set; }
        public int Column { get; set; }

        public bool EsPosicionValida
        {
            get => _esPosicionValida;
            set { _esPosicionValida = value; OnPropertyChanged(nameof(EsPosicionValida)); }
        }

        public string NombrePosicion
        {
            get => _nombrePosicion;
            set { _nombrePosicion = value; OnPropertyChanged(nameof(NombrePosicion)); }
        }

        public JugadorModel Jugador
        {
            get => _jugador;
            set { _jugador = value; OnPropertyChanged(nameof(Jugador)); }
        }
    }

    public class PlantillaViewModel : ViewModelBase
    {
        private readonly IJugadorRepository _jugadorRepository;
        private readonly IEmpleadoRepository _empleadoRepository;

        private string _mensajeEstado;
        private EmpleadoModel _entrenador;

        // --- PROPIEDADES DE CONTROL DE ROL ---
        private Visibility _bancoVisibility;
        private bool _isReadOnlyMode;

        public Visibility BancoVisibility
        {
            get { return _bancoVisibility; }
            set { _bancoVisibility = value; OnPropertyChanged(nameof(BancoVisibility)); }
        }

        public bool IsReadOnlyMode
        {
            get { return _isReadOnlyMode; }
            set { _isReadOnlyMode = value; OnPropertyChanged(nameof(IsReadOnlyMode)); }
        }
        // -------------------------------------

        // --- PROPIEDADES PARA EL CAMBIO DE MISTER ---
        public ObservableCollection<EmpleadoModel> ListaMisters { get; set; }
        private bool _mostrarSelectorMister;
        public bool MostrarSelectorMister
        {
            get => _mostrarSelectorMister;
            set { _mostrarSelectorMister = value; OnPropertyChanged(nameof(MostrarSelectorMister)); }
        }
        // --------------------------------------------------

        public ObservableCollection<CeldaEsquema> GridCampo { get; set; }
        public ObservableCollection<CeldaEsquema> Banquillo { get; set; }
        public ObservableCollection<JugadorModel> Reservas { get; set; }

        public string MensajeEstado
        {
            get => _mensajeEstado;
            set { _mensajeEstado = value; OnPropertyChanged(nameof(MensajeEstado)); }
        }

        public EmpleadoModel Entrenador
        {
            get => _entrenador;
            set { _entrenador = value; OnPropertyChanged(nameof(Entrenador)); }
        }

        public PlantillaViewModel()
        {
            _jugadorRepository = new MysqlJugadorRepository();
            _empleadoRepository = new MysqlEmpleadoRepository();

            GridCampo = new ObservableCollection<CeldaEsquema>();
            Banquillo = new ObservableCollection<CeldaEsquema>();
            Reservas = new ObservableCollection<JugadorModel>();
            ListaMisters = new ObservableCollection<EmpleadoModel>();

            // Configurar permisos antes de cargar nada
            ConfigurarPermisos();

            InicializarEstructuraVisual();
            CargarDatos();
        }

        private void ConfigurarPermisos()
        {
            // Asumimos que UserSession tiene el usuario actual
            if (UserSession.UsuarioActual != null && UserSession.UsuarioActual.Rol == "Jugador")
            {
                // Si es jugador: No ve el banco de reservas y activamos el escudo
                BancoVisibility = Visibility.Collapsed;
                IsReadOnlyMode = true;
            }
            else
            {
                // Resto de roles (Entrenador, Manager, Director): Todo visible y editable
                BancoVisibility = Visibility.Visible;
                IsReadOnlyMode = false;
            }
        }

        private void InicializarEstructuraVisual()
        {
            GridCampo.Clear();
            for (int r = 0; r < 6; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    GridCampo.Add(new CeldaEsquema { Row = r, Column = c, EsPosicionValida = false, Jugador = null });
                }
            }

            Banquillo.Clear();
            for (int i = 0; i < 5; i++)
            {
                Banquillo.Add(new CeldaEsquema { NombrePosicion = "BANQUILLO", EsPosicionValida = true, Jugador = null });
            }
        }

        public void CargarDatos()
        {
            try
            {
                MensajeEstado = "Cargando plantilla...";

                // Cargar todos los empleados para el selector
                var todosLosEmpleados = _empleadoRepository.GetAllEmployees().ToList();
                ListaMisters.Clear();
                foreach (var emp in todosLosEmpleados) ListaMisters.Add(emp);

                // Identificar al titular
                Entrenador = todosLosEmpleados.FirstOrDefault(e => e.Entrenador_titular);

                foreach (var celda in GridCampo) { celda.Jugador = null; celda.EsPosicionValida = false; }
                foreach (var celda in Banquillo) { celda.Jugador = null; }
                Reservas.Clear();

                if (Entrenador?.FormacionTactica == null) return;

                foreach (var posString in Entrenador.FormacionTactica.Posiciones)
                {
                    if (string.IsNullOrWhiteSpace(posString)) continue;
                    string posLimpia = posString.Trim().ToUpper();
                    var coords = ObtenerCoordenadasVisuales(posLimpia);
                    var celda = GridCampo.FirstOrDefault(x => x.Row == coords.Item1 && x.Column == coords.Item2);
                    if (celda != null) { celda.EsPosicionValida = true; celda.NombrePosicion = posLimpia; }
                }

                var todosJugadores = _jugadorRepository.GetAll().ToList();
                int t = 0, b = 0, r = 0;

                foreach (var j in todosJugadores)
                {
                    if (j.Es_titular)
                    {
                        var destino = GridCampo.FirstOrDefault(c => c.EsPosicionValida && c.Jugador == null && c.NombrePosicion == j.Posicion.Trim().ToUpper());
                        if (destino != null) { destino.Jugador = j; t++; }
                        else { j.Es_titular = false; Reservas.Add(j); r++; }
                    }
                    else if (j.Esta_convocado)
                    {
                        var hueco = Banquillo.FirstOrDefault(cb => cb.Jugador == null);
                        if (hueco != null) { hueco.Jugador = j; b++; }
                        else { j.Esta_convocado = false; Reservas.Add(j); r++; }
                    }
                    else
                    {
                        Reservas.Add(j);
                        r++;
                    }
                }
                MensajeEstado = $"Formación: {Entrenador.FormacionTactica.Nombre}.";
            }
            catch (Exception ex) { MensajeEstado = "Error: " + ex.Message; }
        }

        private Tuple<int, int> ObtenerCoordenadasVisuales(string posicion)
        {
            switch (posicion)
            {
                case "EI": return Tuple.Create(0, 0);
                case "DCI": return Tuple.Create(0, 1);
                case "DC": return Tuple.Create(0, 2);
                case "DCD": return Tuple.Create(0, 3);
                case "ED": return Tuple.Create(0, 4);
                case "II": return Tuple.Create(1, 1);
                case "MCO": return Tuple.Create(1, 2);
                case "ID": return Tuple.Create(1, 3);
                case "MI": return Tuple.Create(2, 0);
                case "MCI": return Tuple.Create(2, 1);
                case "MCC": case "MC": return Tuple.Create(2, 2);
                case "MCD": return Tuple.Create(2, 3);
                case "MD": return Tuple.Create(2, 4);
                case "CI": return Tuple.Create(3, 0);
                case "MCDI": return Tuple.Create(3, 1);
                case "MCDC": return Tuple.Create(3, 2);
                case "MCDD": return Tuple.Create(3, 3);
                case "CD": return Tuple.Create(3, 4);
                case "LI": return Tuple.Create(4, 0);
                case "DFCI": return Tuple.Create(4, 1);
                case "DFC": return Tuple.Create(4, 2);
                case "DFCD": return Tuple.Create(4, 3);
                case "LD": return Tuple.Create(4, 4);
                case "PR": case "POR": return Tuple.Create(5, 2);
                default: return Tuple.Create(5, 0);
            }
        }

        public void MoverJugador(JugadorModel jugador, object destino)
        {
            // Bloqueo de seguridad adicional por si falla la UI
            if (IsReadOnlyMode) return;

            if (jugador == null) return;
            var origenC = GridCampo.FirstOrDefault(c => c.Jugador?.Id_jugador == jugador.Id_jugador);
            if (origenC != null) origenC.Jugador = null;
            var origenB = Banquillo.FirstOrDefault(c => c.Jugador?.Id_jugador == jugador.Id_jugador);
            if (origenB != null) origenB.Jugador = null;
            if (Reservas.Contains(jugador)) Reservas.Remove(jugador);

            bool esTit = false; bool esConv = false; string posFinal = jugador.Posicion;

            if (destino is CeldaEsquema celda && celda.EsPosicionValida && celda.NombrePosicion != "BANQUILLO")
            {
                if (celda.Jugador != null) { var s = celda.Jugador; Reservas.Add(s); ActualizarJugadorBD(s, s.Posicion, false, false); }
                celda.Jugador = jugador;
                esTit = true; esConv = true; posFinal = celda.NombrePosicion;
            }
            else if (destino is CeldaEsquema celdaB && celdaB.NombrePosicion == "BANQUILLO")
            {
                if (celdaB.Jugador != null) { var s = celdaB.Jugador; Reservas.Add(s); ActualizarJugadorBD(s, s.Posicion, false, false); }
                celdaB.Jugador = jugador;
                esTit = false; esConv = true;
            }
            else
            {
                Reservas.Add(jugador);
                esTit = false; esConv = false;
            }
            ActualizarJugadorBD(jugador, posFinal, esTit, esConv);
        }

        private void ActualizarJugadorBD(JugadorModel j, string pos, bool tit, bool conv)
        {
            j.Posicion = pos; j.Es_titular = tit; j.Esta_convocado = conv;
            _jugadorRepository.UpdateJugador(j);
        }

        // --- LÓGICA DE CAMBIO DE MISTER ---
        public void SeleccionarNuevoMister(EmpleadoModel nuevo)
        {
            // Seguridad adicional
            if (IsReadOnlyMode) { MostrarSelectorMister = false; return; }

            if (nuevo == null || nuevo.Id_empleado == Entrenador?.Id_empleado)
            {
                MostrarSelectorMister = false;
                return;
            }

            try
            {
                // 1. Quitar titularidad al antiguo en BD (si existe)
                if (Entrenador != null)
                {
                    Entrenador.Entrenador_titular = false;
                    _empleadoRepository.UpdateEmployee(Entrenador); // Necesitas este método en tu Repo
                }

                // 2. Poner titularidad al nuevo en BD
                nuevo.Entrenador_titular = true;
                _empleadoRepository.UpdateEmployee(nuevo);

                // 3. Recargar todo para que cambie la formación visual
                CargarDatos();
                MostrarSelectorMister = false;
            }
            catch (Exception ex) { MensajeEstado = "Error al cambiar mister: " + ex.Message; }
        }
    }
}