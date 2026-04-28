using InaManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InaManager.Repositories;
using System.Net;
using System.Security.Principal;
using System.Threading;
using System.Windows; // Necesario para MessageBox
using InaManager.View;

namespace InaManager.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        // Fields
        private string _username;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        // Repositorios
        private IEmpleadoRepository empleadoRepository;
        private IJugadorRepository jugadorRepository;

        // Properties
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public SecureString Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        // Commands
        public ICommand LoginCommand { get; }
        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }

        // Constructor
        public LoginViewModel()
        {
            // Inicializamos ambos repositorios
            empleadoRepository = new MysqlEmpleadoRepository();
            jugadorRepository = new MysqlJugadorRepository();

            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RecoverPasswordCommand = new ViewModelCommand(p => ExecuteRecoverPassCommand("", ""));
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
                Password == null || Password.Length < 3)
                validData = false;
            else
                validData = true;
            return validData;
        }

        private void ExecuteLoginCommand(object obj)
        {
            var credential = new NetworkCredential(Username, Password);

            // 1. Intentar validar como EMPLEADO
            bool isValidEmp = empleadoRepository.ValidateEmployee(credential);

            if (isValidEmp)
            {
                var empData = empleadoRepository.GetEmployeeByUsername(Username);

                // ASIGNACIÓN ÚNICA: Rellenamos el objeto que UserSession espera
                UserSession.UsuarioActual = new EmpleadoAccountModel
                {
                    IdUsuario = empData.Id_empleado, // Esto hará que UserSession.IdUsuario devuelva este valor
                    Username = empData.Username,
                    DisplayName = $"{empData.Nombre} {empData.Apellido}",
                    Rol = empData.Puesto, // Esto hará que UserSession.Rol devuelva este valor
                    Email = empData.Email,
                    ProfilePicture = empData.Url_imagen
                };

                // Identidad de seguridad para el hilo actual
                Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(Username),
                    new[] { empData.Puesto }
                );

                AbrirVentanaPrincipal();
            }
            else
            {
                // 2. Intentar validar como JUGADOR
                bool isValidJug = jugadorRepository.ValidateJugador(credential);

                if (isValidJug)
                {
                    var jugData = jugadorRepository.GetJugadorByUsername(Username);

                    // Verificación de primer acceso (Lógica de negocio)
                    if (jugData != null && jugData.Debe_cambiar_pass)
                    {
                        MessageBox.Show("Por seguridad, debes cambiar tu contraseña inicial.", "Primer Acceso");
                        return;
                    }

                    // ASIGNACIÓN ÚNICA: Rellenamos para el Jugador
                    UserSession.UsuarioActual = new EmpleadoAccountModel
                    {
                        IdUsuario = jugData.Id_jugador,
                        Username = jugData.Username,
                        DisplayName = $"{jugData.Nombre} {jugData.Apellido}",
                        Rol = "Jugador",
                        Email = jugData.Email,
                        ProfilePicture = jugData.Url_imagen
                    };

                    Thread.CurrentPrincipal = new GenericPrincipal(
                        new GenericIdentity(Username),
                        new[] { "Jugador" }
                    );

                    AbrirVentanaPrincipal();
                }
                else
                {
                    ErrorMessage = "Usuario o contraseña incorrectos";
                }
            }
        }
        private void AbrirVentanaPrincipal()
        {
            IsViewVisible = false;
            var mainView = new MainView();
            mainView.Show();
        }

        private void ExecuteRecoverPassCommand(string username, string mail)
        {
            throw new NotImplementedException();
        }
    }
}