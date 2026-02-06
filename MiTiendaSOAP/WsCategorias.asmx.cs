using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace MiTiendaSOAP
{
    /// <summary>
    /// Descripción breve de WsCategorias
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class WsCategorias : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hola a todos";
        }


        [WebMethod]
        public string CrearCategorias()
        {

            return "Hola a todos";

        }


        [WebMethod]
        public string ActualizarCategorias()
        {
            return "Hola a todos";


        }

        [WebMethod]
        public string BorrarCategorias()
        {
            return "Hola a todos";


        }

        [WebMethod]
        public string BuscarCategorias()
        {

            return "Hola a todos";

        }





    }
}
