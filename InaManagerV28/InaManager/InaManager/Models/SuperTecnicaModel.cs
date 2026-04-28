using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaManager.Models
{
    public class SuperTecnicaModel
    {
        private int _id_supertecnica;
        private string _nombre_supertecnica;
        private string _tipo_supertecnica;
        private string _afinidad_supertecnica;
        private string especialidad;
        private int potencia;
        public int Id_SuperTecnica
        {
            get { return _id_supertecnica; }
            set { _id_supertecnica = value; }
        }
        public string Nombre_SuperTecnica
        {
            get { return _nombre_supertecnica; }
            set { _nombre_supertecnica = value; }
        }
        public string Tipo_SuperTecnica
        {
            get { return _tipo_supertecnica; }
            set { _tipo_supertecnica = value; }
        }
        public string Afinidad_SuperTecnica
        {
            get { return _afinidad_supertecnica; }
            set { _afinidad_supertecnica = value; }
        }
        public string Especialidad
        {
            get { return especialidad; }
            set { especialidad = value; }
        }
        public int Potencia
        {
            get { return potencia; }
            set { potencia = value; }
        }
    }
}
