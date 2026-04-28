using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaManager.Models
{
    public class SponsorModel
    {
        private int _id_sponsor;
        private string _nombre_empresa;
        private string _sector;
        private decimal _aporte_economico;
        private DateTime _fecha_inicio;
        private DateTime _fecha_fin;
        private string _url_logo;
        
        public int Id_Sponsor
        {
            get { return _id_sponsor; }
            set { _id_sponsor = value; }
        }
        public string Nombre_Empresa
        {
            get { return _nombre_empresa; }
            set { _nombre_empresa = value; }
        }
        public string Sector
        {
            get { return _sector; }
            set { _sector = value; }
        }
        public decimal Aporte_Economico
        {
            get { return _aporte_economico; }
            set { _aporte_economico = value; }
        }
        public string Url_Logo
        {
            get { return _url_logo; }
            set { _url_logo = value; }
        }
        public DateTime Fecha_Inicio
        {
            get { return _fecha_inicio; }
            set { _fecha_inicio = value; }
        }
        public DateTime Fecha_Fin
        {
            get { return _fecha_fin; }
            set { _fecha_fin = value; }
        }


    }
}
