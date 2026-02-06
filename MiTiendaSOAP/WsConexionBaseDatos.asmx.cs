using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MySql.Data.MySqlClient;

namespace MiTiendaSOAP
{
    /// <summary>
    /// Descripción breve de WsConexionBaseDatos
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class WsConexionBaseDatos : System.Web.Services.WebService
    {

        /// <summary>
        /// Este método se encarga de comprobar si mi conexión es correcta.
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string PruebaConexion()
        {
            string connString = "Server=localhost;Database=tiendasoap;Uid=root;Pwd=;";
            using (MySqlConnection conexion = new MySqlConnection(connString))
            {
                try
                {
                    conexion.Open();
                    conexion.Close();
                    return "Conexión correcta";
                 
                }
                catch (Exception ex)
                {
                    return "Conexión incorrecta";
                }
            }
        }



        



        /// <summary>
        /// Este método es el encargado de comprobar si la conexión con la base de datos es exitosa. 
        /// </summary>
        /// <returns></returns>
       
        public string PruebaConexionV1()
        {

            // 1. Cadena de conexión
            string connString = "Server=localhost;Database=tiendasoap;Uid=root;Pwd=;";

            using (MySqlConnection conexion = new MySqlConnection(connString))
            {
                try
                {
                    conexion.Open();
                  //  Console.WriteLine("Conexión exitosa!");

                    string query = "SELECT nombreCategoria FROM categorias";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        // 3. Uso de parámetros para seguridad
                       //  cmd.Parameters.AddWithValue("@activo", 1);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
          //                      Console.WriteLine($"ID: {reader["id"]}, Nombre: {reader["nombre"]}");
                            }
                        }
                    }
                    return "Conexión correcta";
                }
                catch (Exception ex)
                {

                    return "Conexión incorrecta";
                    //            Console.WriteLine("Error: " + ex.Message);
                }
            }



           
        }
    }
}
