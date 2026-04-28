using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaManager.Models
{
    public class PartidoModel
    {
        private int _id_partido;
        private DateOnly _fecha_partido;
        private string _equipo_local;
        private string _equipo_visitante;
        private int _goles_local;
        private int _goles_visitante;
        private bool _celebrado;


        public int Id_Partido
        {
            get { return _id_partido; }
            set { _id_partido = value; }
        }
        public DateOnly Fecha_Partido
        {
            get { return _fecha_partido; }
            set { _fecha_partido = value; }
        }
        public string Equipo_Local
        {
            get { return _equipo_local; }
            set { _equipo_local = value; }
        }
        public string Equipo_Visitante
        {
            get { return _equipo_visitante; }
            set { _equipo_visitante = value; }
        }
        public int Goles_Local
        {
            get { return _goles_local; }
            set { _goles_local = value; }
        }
        public int Goles_Visitante
        {
            get { return _goles_visitante; }
            set { _goles_visitante = value; }
        }
        public bool Celebrado
        {
            get { return _celebrado; }
            set { _celebrado = value; }
        }
    }
}
