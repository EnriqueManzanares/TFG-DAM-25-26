using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaManager.Models
{
    public static class UserSession
    {
        // Guardamos el objeto completo del modelo
        public static EmpleadoAccountModel UsuarioActual { get; set; }

        // Atajos para no romper el código que ya escribió tu compañero
        public static int IdUsuario => UsuarioActual?.IdUsuario ?? 0;
        public static string Rol => UsuarioActual?.Rol ?? string.Empty;
    }
}
