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
        private decimal _saldoActual;

        public string IbanDestinatario
        {
            get => _ibanDestinatario;
            set
            {
                _ibanDestinatario = value;
                OnPropertyChanged(nameof(IbanDestinatario));
                ValidarIban();
            }
        }

        public string Monto
        {
            get => _monto;
            set
            {
                _monto = value;
                OnPropertyChanged(nameof(Monto));
                OnPropertyChanged(nameof(SaldoDespues));
                ValidarMonto();
            }
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

        public decimal SaldoActual
        {
            get => _saldoActual;
            set { _saldoActual = value; OnPropertyChanged(nameof(SaldoActual)); }
        }

        public decimal SaldoDespues
        {
            get
            {
                if (decimal.TryParse(_monto, out var montoDecimal))
                {
                    return SaldoActual - montoDecimal;
                }
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

        private void CargarSaldoActual()
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

        private void ValidarIban()
        {
            MostrarErrorIban = false;
            ErrorIban = string.Empty;

            if (string.IsNullOrWhiteSpace(IbanDestinatario))
            {
                return;
            }

            // Validación básica de IBAN español
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

            if (string.IsNullOrWhiteSpace(Monto))
            {
                return;
            }

            if (!decimal.TryParse(Monto, out var montoDecimal))
            {
                MostrarErrorMonto = true;
                ErrorMonto = "Monto inválido. Usa formato numérico";
                return;
            }

            if (montoDecimal <= 0)
            {
                MostrarErrorMonto = true;
                ErrorMonto = "El monto debe ser mayor a 0";
                return;
            }

            if (montoDecimal > SaldoActual)
            {
                MostrarErrorMonto = true;
                ErrorMonto = "Saldo insuficiente para esta transferencia";
                return;
            }
        }

        private void ExecuteConfirmarTransferencia(object obj)
        {
            // Validar todos los campos
            ValidarIban();
            ValidarMonto();

            if (MostrarErrorIban || MostrarErrorMonto)
            {
                MostrarMensaje = true;
                Mensaje = "Por favor, corrija los errores antes de continuar";
                return;
            }

            try
            {
                int idEmpleado = UserSession.IdUsuario;
                if (idEmpleado <= 0)
                {
                    throw new Exception("Usuario no autenticado");
                }

                if (!decimal.TryParse(Monto, out var montoDecimal))
                {
                    throw new Exception("Monto inválido");
                }

                // Realizar la transferencia
                bool exitoTransferencia = _bancoRepository.RealizarTransferencia(
                    idEmpleado,
                    IbanDestinatario,
                    montoDecimal,
                    Concepto ?? "Transferencia"
                );

                if (exitoTransferencia)
                {
                    MostrarMensaje = true;
                    Mensaje = "✓ Transferencia realizada exitosamente";
                    
                    // Cerrar la ventana después de 2 segundos
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(
                        new Action(() =>
                        {
                            foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
                            {
                                if (window.GetType().Name == "TransferenciaBancariaView")
                                {
                                    window.Close();
                                    break;
                                }
                            }
                        }), 
                        System.Windows.Threading.DispatcherPriority.Normal);
                }
                else
                {
                    MostrarMensaje = true;
                    Mensaje = "✗ Error al realizar la transferencia. Intenta nuevamente";
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje = true;
                Mensaje = $"✗ Error: {ex.Message}";
            }
        }

        private void ExecuteCancelar(object obj)
        {
            foreach (System.Windows.Window w in System.Windows.Application.Current.Windows)
            {
                if (w.GetType().Name == "TransferenciaBancariaView")
                {
                    w.Close();
                    break;
                }
            }
        }
    }
}
