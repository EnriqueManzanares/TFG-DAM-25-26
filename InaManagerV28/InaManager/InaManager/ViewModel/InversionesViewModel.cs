using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
using InaManager.Models;
using InaManager.Repositories;

namespace InaManager.ViewModel
{
    public class InversionesViewModel : ViewModelBase
    {
        private readonly IInversionRepository _inversionRepository;
        private readonly DispatcherTimer _timer;

        public Action OnInversionRealizada { get; set; }

        // ── Tab activa ────────────────────────────────────────────────────
        private string _tabActiva = "Mercado";
        public string TabActiva
        {
            get => _tabActiva;
            set { _tabActiva = value; OnPropertyChanged(nameof(TabActiva)); OnPropertyChanged(nameof(IsTabMercado)); OnPropertyChanged(nameof(IsTabCartera)); }
        }
        public bool IsTabMercado => TabActiva == "Mercado";
        public bool IsTabCartera => TabActiva == "Cartera";

        // ── Colecciones ───────────────────────────────────────────────────
        private ObservableCollection<ActivoItemVM> _activos;
        public ObservableCollection<ActivoItemVM> Activos
        {
            get => _activos;
            set { _activos = value; OnPropertyChanged(nameof(Activos)); }
        }

        private ObservableCollection<CarteraItemVM> _cartera;
        public ObservableCollection<CarteraItemVM> Cartera
        {
            get => _cartera;
            set { _cartera = value; OnPropertyChanged(nameof(Cartera)); }
        }

        // ── Activo seleccionado (Mercado) ─────────────────────────────────
        private ActivoItemVM _activoSeleccionado;
        public ActivoItemVM ActivoSeleccionado
        {
            get => _activoSeleccionado;
            set
            {
                _activoSeleccionado = value;
                OnPropertyChanged(nameof(ActivoSeleccionado));
                OnPropertyChanged(nameof(HayActivoSeleccionado));
                OnPropertyChanged(nameof(UnidadesARecibir));
                MostrarMensaje = false;
            }
        }
        public bool HayActivoSeleccionado => ActivoSeleccionado != null;

        // ── Formulario compra ─────────────────────────────────────────────
        private string _eurosAInvertir;
        public string EurosAInvertir
        {
            get => _eurosAInvertir;
            set { _eurosAInvertir = value; OnPropertyChanged(nameof(EurosAInvertir)); OnPropertyChanged(nameof(UnidadesARecibir)); MostrarMensaje = false; }
        }

        public string UnidadesARecibir
        {
            get
            {
                if (ActivoSeleccionado == null) return "0.00000000";
                if (!decimal.TryParse(EurosAInvertir, out var euros) || euros <= 0) return "0.00000000";
                if (ActivoSeleccionado.PrecioActual <= 0) return "0.00000000";
                return (euros / ActivoSeleccionado.PrecioActual).ToString("F8");
            }
        }

        // ── Feedback ──────────────────────────────────────────────────────
        private string _mensaje;
        public string Mensaje
        {
            get => _mensaje;
            set { _mensaje = value; OnPropertyChanged(nameof(Mensaje)); }
        }

        private bool _mostrarMensaje;
        public bool MostrarMensaje
        {
            get => _mostrarMensaje;
            set { _mostrarMensaje = value; OnPropertyChanged(nameof(MostrarMensaje)); }
        }

        private bool _mensajeExitoso;
        public bool MensajeExitoso
        {
            get => _mensajeExitoso;
            set { _mensajeExitoso = value; OnPropertyChanged(nameof(MensajeExitoso)); }
        }

        private bool _hayCartera;
        public bool HayCartera
        {
            get => _hayCartera;
            set { _hayCartera = value; OnPropertyChanged(nameof(HayCartera)); }
        }

        // ── Comandos ──────────────────────────────────────────────────────
        public ICommand ComprarCommand          { get; }
        public ICommand VenderCommand           { get; }
        public ICommand VenderTodoCommand       { get; }
        public ICommand ToggleFormVentaCommand  { get; }
        public ICommand TabMercadoCommand       { get; }
        public ICommand TabCarteraCommand       { get; }

        // ── Constructor ───────────────────────────────────────────────────
        public InversionesViewModel()
        {
            _inversionRepository = new MysqlInversionRepository();

            ComprarCommand         = new ViewModelCommand(ExecuteComprar);
            VenderCommand          = new ViewModelCommand(ExecuteVender);
            VenderTodoCommand      = new ViewModelCommand(ExecuteVenderTodo);
            ToggleFormVentaCommand = new ViewModelCommand(ExecuteToggleFormVenta);
            TabMercadoCommand      = new ViewModelCommand(_ => TabActiva = "Mercado");
            TabCarteraCommand      = new ViewModelCommand(_ => { TabActiva = "Cartera"; CargarCartera(); });

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(30) };
            _timer.Tick += (s, e) => ActualizarPrecios();
            _timer.Start();

            CargarDatos();
        }

        // ── Carga ─────────────────────────────────────────────────────────
        public void CargarDatos()
        {
            try
            {
                var activos = _inversionRepository.ObtenerActivos();
                var col = new ObservableCollection<ActivoItemVM>();
                foreach (var a in activos) col.Add(new ActivoItemVM(a));
                Activos = col;
                CargarCartera();
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"CargarDatos: {ex.Message}"); }
        }

        public void CargarCartera()
        {
            try
            {
                int id = UserSession.IdUsuario;
                bool esJugador = UserSession.Rol == "Jugador";
                var posiciones = _inversionRepository.ObtenerCarteraUsuario(id, esJugador);
                var col = new ObservableCollection<CarteraItemVM>();
                foreach (var p in posiciones) col.Add(new CarteraItemVM(p));
                Cartera = col;
                HayCartera = Cartera.Count > 0;
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"CargarCartera: {ex.Message}"); }
        }

        private void ActualizarPrecios()
        {
            if (Activos != null) foreach (var a in Activos) a.RefrescarPrecio();
            if (Cartera != null) foreach (var c in Cartera) c.RefrescarPrecio();
            OnPropertyChanged(nameof(UnidadesARecibir));
        }

        // ── Ejecutar comandos ─────────────────────────────────────────────
        private void ExecuteToggleFormVenta(object obj)
        {
            if (obj is CarteraItemVM item)
            {
                bool nuevoEstado = !item.MostrarFormVenta;
                foreach (var c in Cartera) c.MostrarFormVenta = false;
                item.MostrarFormVenta = nuevoEstado;
            }
        }

        private void ExecuteComprar(object obj)
        {
            MostrarMensaje = false;
            if (ActivoSeleccionado == null) { MostrarError("Selecciona un activo primero."); return; }
            if (!decimal.TryParse(EurosAInvertir, out var euros) || euros <= 0) { MostrarError("Introduce un importe válido mayor que 0."); return; }
            try
            {
                int id = UserSession.IdUsuario;
                bool esJugador = UserSession.Rol == "Jugador";
                decimal precio = ActivoSeleccionado.PrecioActual;
                decimal cantidad = euros / precio;
                bool ok = _inversionRepository.ComprarActivo(id, esJugador, ActivoSeleccionado.Model.Id_activo, cantidad, precio, euros);
                if (ok)
                {
                    MensajeExitoso = true;
                    Mensaje = $"✓ Has comprado {cantidad:F6} {ActivoSeleccionado.Model.Simbolo} por {euros:C2}";
                    MostrarMensaje = true;
                    EurosAInvertir = string.Empty;
                    CargarCartera();
                    OnInversionRealizada?.Invoke();
                }
                else MostrarError("No se pudo completar la compra. Comprueba tu saldo.");
            }
            catch (Exception ex) { MostrarError($"Error: {ex.Message}"); }
        }

        private void ExecuteVender(object obj)
        {
            if (obj is CarteraItemVM item)
            {
                if (!decimal.TryParse(item.CantidadVender, out var cant) || cant <= 0) { MostrarError("Introduce una cantidad válida para vender."); return; }
                if (cant > item.Model.Cantidad) { MostrarError($"No puedes vender más de {item.Model.Cantidad:F6} {item.Model.SimboloActivo}."); return; }
                EjecutarVenta(item, cant);
            }
        }

        private void ExecuteVenderTodo(object obj)
        {
            if (obj is CarteraItemVM item)
                EjecutarVenta(item, item.Model.Cantidad);
        }

        private void EjecutarVenta(CarteraItemVM item, decimal cantidad)
        {
            try
            {
                int id = UserSession.IdUsuario;
                bool esJugador = UserSession.Rol == "Jugador";
                decimal precio = item.PrecioActual;
                bool ok = _inversionRepository.VenderActivoParcial(id, esJugador, item.Model.Id_activo, cantidad, precio);
                if (ok)
                {
                    MensajeExitoso = true;
                    Mensaje = $"✓ Vendidas {cantidad:F6} {item.Model.SimboloActivo} por {cantidad * precio:C2}";
                    MostrarMensaje = true;
                    CargarCartera();
                    OnInversionRealizada?.Invoke();
                }
                else MostrarError("No se pudo completar la venta.");
            }
            catch (Exception ex) { MostrarError($"Error: {ex.Message}"); }
        }

        private void MostrarError(string msg) { MensajeExitoso = false; Mensaje = msg; MostrarMensaje = true; }

        public void Detener() => _timer?.Stop();
    }

    // ─── Wrapper de activo con precio en tiempo real ──────────────────────
    public class ActivoItemVM : ViewModelBase
    {
        public ActivoInversionModel Model { get; }

        private decimal _precioActual;
        public decimal PrecioActual
        {
            get => _precioActual;
            set { _precioActual = value; OnPropertyChanged(nameof(PrecioActual)); OnPropertyChanged(nameof(VariacionPct)); OnPropertyChanged(nameof(VariacionTexto)); OnPropertyChanged(nameof(EsPositivo)); }
        }

        public double  VariacionPct   => Model.PrecioBase > 0 ? (double)((PrecioActual - Model.PrecioBase) / Model.PrecioBase * 100) : 0;
        public string  VariacionTexto => VariacionPct >= 0 ? $"+{VariacionPct:F2}%" : $"{VariacionPct:F2}%";
        public bool    EsPositivo     => VariacionPct >= 0;

        public ActivoItemVM(ActivoInversionModel model) { Model = model; RefrescarPrecio(); }

        public void RefrescarPrecio() => PrecioActual = CalcularPrecio(Model.PrecioBase, Model.Volatilidad, Model.Id_activo);

        public static decimal CalcularPrecio(decimal precioBase, decimal volatilidad, int idActivo)
        {
            double horas  = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 3600.0;
            double phase  = idActivo * 1.7320508;
            double factor = 1.0 + (double)volatilidad * Math.Sin(horas * 0.8 + phase);
            return Math.Round(precioBase * (decimal)Math.Max(0.01, factor), 2);
        }
    }

    // ─── Wrapper de posición de cartera con P&L y formulario de venta ────
    public class CarteraItemVM : ViewModelBase
    {
        public InversionModel Model { get; }

        private decimal _precioActual;
        public decimal PrecioActual
        {
            get => _precioActual;
            set { _precioActual = value; OnPropertyChanged(nameof(PrecioActual)); OnPropertyChanged(nameof(ValorActual)); OnPropertyChanged(nameof(GananciaPerdida)); OnPropertyChanged(nameof(GananciaPct)); OnPropertyChanged(nameof(EsGanancia)); OnPropertyChanged(nameof(GananciaPerdidaTexto)); OnPropertyChanged(nameof(GananciaPctTexto)); }
        }

        public decimal ValorActual       => Math.Round(Model.Cantidad * PrecioActual, 2);
        public decimal GananciaPerdida   => ValorActual - Math.Round(Model.Cantidad * Model.PrecioCompra, 2);
        public double  GananciaPct       => Model.PrecioCompra > 0 ? (double)((PrecioActual - Model.PrecioCompra) / Model.PrecioCompra * 100) : 0;
        public bool    EsGanancia        => GananciaPerdida >= 0;
        public string  GananciaPctTexto      => GananciaPct >= 0 ? $"+{GananciaPct:F2}%" : $"{GananciaPct:F2}%";
        public string  GananciaPerdidaTexto  => GananciaPerdida >= 0 ? $"+{GananciaPerdida:C2}" : $"{GananciaPerdida:C2}";

        // ── Formulario de venta parcial ───────────────────────────────────
        private bool _mostrarFormVenta;
        public bool MostrarFormVenta
        {
            get => _mostrarFormVenta;
            set { _mostrarFormVenta = value; OnPropertyChanged(nameof(MostrarFormVenta)); }
        }

        private string _cantidadVender;
        public string CantidadVender
        {
            get => _cantidadVender;
            set { _cantidadVender = value; OnPropertyChanged(nameof(CantidadVender)); }
        }

        public CarteraItemVM(InversionModel model) { Model = model; RefrescarPrecio(); }

        public void RefrescarPrecio() => PrecioActual = ActivoItemVM.CalcularPrecio(Model.PrecioBase, Model.Volatilidad, Model.Id_activo);
    }
}
