using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Configuration;

namespace SisCur.Models
{
    public class UsuarioModel
    {
        public int IDUsuario { get; set; }

        public string NombrePersona { get; set; }

        public string NombreUsuario { get; set; }

        public string NombreRol { get; set; }

        public string NombreTipoUsuario { get; set; }

    }
}