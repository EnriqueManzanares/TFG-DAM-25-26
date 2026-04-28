using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaManager.Models
{
    public class EmpleadoAccountModel
    {
        public int IdUsuario { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Rol{ get; set; }
        public string ProfilePicture { get; set; }
        public string Email { get; set; }

    }
}
