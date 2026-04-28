using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaManager.Models
{
    public class EmpleadoModel
    {
        // --- CAMPOS PRIVADOS ---
        private int _id_empleado;
        private string _nombre;
        private string _apellido;
        private string _email;
        private string _username;
        private string _password;
        private int _telefono;
        private string _puesto;
        private string _especialidad;
        private int _años_experiencia;
        private string _url_imagen;
        private bool _entrenador_titular;
        private int _id_formacion_activa;
        private decimal _salario;
        public FormacionModel FormacionTactica { get; set; }

        // --- PROPIEDADES PÚBLICAS ---
        public int Id_empleado
        {
            get { return _id_empleado; }
            set { _id_empleado = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public string Apellido
        {
            get { return _apellido; }
            set { _apellido = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public int Telefono
        {
            get { return _telefono; }
            set { _telefono = value; }
        }   

        public string Puesto
        {
            get { return _puesto; }
            set { _puesto = value; }
        }

        public string Especialidad
        {
            get { return _especialidad; }
            set { _especialidad = value; }
        }

        public int Años_experiencia
        {
            get { return _años_experiencia; }
            set { _años_experiencia = value; }
        }

        public string Url_imagen
        {
            get { return _url_imagen; }
            set { _url_imagen = value; }
        }
        public bool Entrenador_titular
        {
            get { return _entrenador_titular; }
            set { _entrenador_titular = value; }
        }

        public int Id_formacion_activa
        {
            get { return _id_formacion_activa; }
            set { _id_formacion_activa = value; }
        }

        public decimal Salario
        {
            get { return _salario; }
            set { _salario = value; }
        }

        // --- CONSTRUCTORES ---

        // 1. Constructor
        public EmpleadoModel()
        {
        }

        // 2. Constructor 
        public EmpleadoModel(int id, string nombre, string apellido, string mail , int telefono , string puesto,
                        string especialidad, int experiencia, string url, bool entrenadorTitular,
                        int idFormacion, decimal salario)
        {
            this.Id_empleado = id;
            this.Nombre = nombre;
            this.Email= mail ;
            this.Telefono= telefono;
            this.Apellido = apellido;
            this.Puesto = puesto;
            this.Especialidad = especialidad;
            this.Años_experiencia = experiencia;
            this.Url_imagen = url;
            this.Entrenador_titular = entrenadorTitular;
            this.Id_formacion_activa = idFormacion;
            this.Salario = salario;
        }
    }
}