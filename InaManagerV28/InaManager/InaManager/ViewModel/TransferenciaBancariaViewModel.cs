using System;
using System.Windows.Input;
using InaManager.Models;
using InaManager.Repositories;

namespace InaManager.ViewModel
{
    public class TransferenciaBancariaViewModel : ViewModelBase
    {
        private readonly IBancoRepository _bancoRepository;
        private string _ibanDestinatario;
        private string _monto;
        private string _concepto;
        private string _errorIban;
        private string _errorMonto;
        private string _mensaje;
        private bool _mostrarErrorIban;
        private bool _mostrarErrorMonto;
        private bool _mostrarMensaje;
        private bool _transferenciaExitosa;
        private decimal _saldoActual;

        /// <summary>
        /// Callback que el padre (BancoViewModel) asigna para volver a Mi Cuenta al cancelar o confirmar.
        /// </summary>
        public Action OnCerrar { get; set; }

        public string IbanDestinatario
        {
            get => _ibanDestinatario;
            set { _ibanDestinatario = value; OnPropertyChanged(nameof(IbanDestinatario)); ValidarIban(); }
        }

        public string Monto
        {
            get => _monto;
            set { _monto = value; OnPropertyChanged(nameof(Monto)); OnPropertyChanged(nameof(SaldoDespues)); ValidarMonto(); }
        }

        public string Concepto
        {
            get => _concepto;
            set { _concepto = value; OnPropertyChanged(nameof(Concepto)); }
        }

        public string ErrorIban
        {
            get => _errorIban;
            set { _errorIban = value; OnPropertyChanged(nameof(ErrorIban)); }
        }

        public string ErrorMonto
        {
            get => _errorMonto;
            set { _errorMonto = value; OnPropertyChanged(nameof(ErrorMonto)); }
        }

        public string Mensaje
        {
            get => _mensaje;
            set { _mensaje = value; OnPropertyChanged(nameof(Mensaje)); }
        }

        public bool MostrarErrorIban
        {
            get => _mostrarErrorIban;
            set { _mostrarErrorIban = value; OnPropertyChanged(nameof(MostrarErrorIban)); }
        }

        public bool MostrarErrorMonto
        {
            get => _mostrarErrorMonto;
            set { _mostrarErrorMonto = value; OnPropertyChanged(nameof(MostrarErrorMonto)); }
        }

        public bool MostrarMensaje
        {
            get => _mostrarMensaje;
            set { _mostrarMensaje = value; OnPropertyChanged(nameof(MostrarMensaje)); }
        }

        public bool TransferenciaExitosa
        {
            get => _transferenciaExitosa;
            set { _transferenciaExitosa = value; OnPropertyChanged(nameof(TransferenciaExitosa)); }
        }

        public decimal SaldoActual
        {
            get => _saldoActual;
            set { _saldoActual = value; OnPropertyChanged(nameof(SaldoActual)); }
        }

        public decimal SaldoDespues
        {
            get
            {
                if (decimal.TryParse(_monto, out var m)) return SaldoActual - m;
                return SaldoActual;
            }
        }

        public ICommand ConfirmarTransferenciaCommand { get; }
        public ICommand CancelarCommand { get; }

        public TransferenciaBancariaViewModel()
        {
            _bancoRepository = new MysqlBancoRepository();
            ConfirmarTransferenciaCommand = new ViewModelCommand(ExecuteConfirmarTransferencia);
            CancelarCommand = new ViewModelCommand(ExecuteCancelar);
            CargarSaldoActual();
        }

        public void CargarSaldoActual()
        {
            try
            {
                int idEmpleado = UserSession.IdUsuario;
                if (idEmpleado > 0)
                {
                    var cuenta = _bancoRepository.ObtenerCuentaPorEmpleado(idEmpleado);
                    SaldoActual = cuenta.Saldo;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje = true;
                Mensaje = $"Error al cargar saldo: {ex.Message}";
            }
        }

        private void Resetear()
        {
            IbanDestinatario = string.Empty;
            Monto = string.Empty;
            Concepto = string.Empty;
            MostrarMensaje = false;
            MostrarErrorIban = false;
            MostrarErrorMonto = false;
            TransferenciaExitosa = false;
        }

        private void ValidarIban()
        {
            MostrarErrorIban = false;
            ErrorIban = string.Empty;
            if (string.IsNullOrWhiteSpace(IbanDestinatario)) return;
            if (IbanDestinatario.Length != 24 || !IbanDestinatario.StartsWith("ES"))
            {
                MostrarErrorIban = true;
                ErrorIban = "IBAN debe ser español (24 caracteres, comenzar con ES)";
            }
        }

        private void ValidarMonto()
        {
            MostrarErrorMonto = false;
            ErrorMonto = string.Empty;
            if (string.IsNullOrWhiteSpace(Monto)) return;
            if (!decimal.TryParse(Monto, out var montoDecimal))
            {
                MostrarErrorMonto = true; ErrorMonto = "Monto inválido. Usa formato numérico"; return;
            }
            if (montoDecimal <= 0)
            {
                MostrarErrorMonto = true; ErrorMonto = "El monto debe ser mayor a 0"; return;
            }
            if (montoDecimal > SaldoActual)
            {
                MostrarErrorMonto = true; ErrorMonto = "Saldo insuficiente para esta transferencia";
            }
        }

        private void ExecuteConfirmarTransferencia(object obj)
        {
            ValidarIban();
            ValidarMonto();

            if (MostrarErrorIban || MostrarErrorMonto)
            {
                MostrarMensaje = true;
                TransferenciaExitosa = false;
                Mensaje = "Por favor, corrija los errores antes de continuar";
                return;
            }

            try
            {
                int idEmpleado = UserSession.IdUsuario;
                if (idEmpleado <= 0) throw new Exception("Usuario no autenticado");
                if (!decimal.TryParse(Monto, out var montoDecimal)) throw new Exception("Monto inválido");

                bool exito = _bancoRepository.RealizarTransferencia(idEmpleado, IbanDestinatario, montoDecimal, Concepto ?? "Transferencia");

                if (exito)
                {
                    MostrarMensaje = true;
                    TransferenciaExitosa = true;
                    Mensaje = "✓ Transferencia realizada exitosamente";

                    // Volver a Mi Cuenta tras 1.5 segundos y refrescar
                    System.Threading.Tasks.Task.Delay(1500).ContinueWith(_ =>
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                        {
                            Resetear();
                            CargarSaldoActual();
                            OnCerrar?.Invoke();
                        });
                    });
                }
                else
                {
                    MostrarMensaje = true;
                    TransferenciaExitosa = false;
                    Mensaje = "✗ Error al realizar la transferencia. Intenta nuevamente";
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje = true;
                TransferenciaExitosa = false;
                Mensaje = $"✗ Error: {ex.Message}";
            }
        }

        private void ExecuteCancelar(object obj)
        {
            Resetear();
            OnCerrar?.Invoke();
        }
    }
}
