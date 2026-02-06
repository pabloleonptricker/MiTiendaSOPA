using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using MySql.Data.MySqlClient;

namespace MiTiendaSOAP
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class TiendaService : System.Web.Services.WebService
    {
        private string connString = ConfigurationManager.ConnectionStrings["TiendaDB"].ConnectionString;

        #region Usuarios

        [WebMethod]
        public string ValidarUsuario(string nombreUsuario, string contraseña)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT UsuarioID, Nombre, Apellido, Email FROM Usuarios WHERE NombreUsuario = @user AND Contraseña = @pass";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", nombreUsuario);
                        cmd.Parameters.AddWithValue("@pass", contraseña);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return $"ID: {reader["UsuarioID"]}, Nombre: {reader["Nombre"]} {reader["Apellido"]}, Email: {reader["Email"]}";
                            }
                        }
                    }
                    return "Error: Credenciales incorrectas.";
                }
                catch (Exception ex)
                {
                    RegistrarLogError("ValidarUsuario", ex.Message);
                    return "Error de conexión.";
                }
            }
        }

        [WebMethod]
        public string RegistrarUsuario(string nombreUsuario, string contraseña, string nombre, string apellido, string email)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // Verificar duplicados
                    string checkQuery = "SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @user OR Email = @email";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@user", nombreUsuario);
                        checkCmd.Parameters.AddWithValue("@email", email);
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                            return "Error: El usuario o email ya existe.";
                    }

                    string query = "INSERT INTO Usuarios (NombreUsuario, Contraseña, Nombre, Apellido, Email) VALUES (@user, @pass, @name, @surname, @email)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", nombreUsuario);
                        cmd.Parameters.AddWithValue("@pass", contraseña);
                        cmd.Parameters.AddWithValue("@name", nombre);
                        cmd.Parameters.AddWithValue("@surname", apellido);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.ExecuteNonQuery();
                    }
                    return "Usuario registrado correctamente.";
                }
                catch (Exception ex)
                {
                    RegistrarLogError("RegistrarUsuario", ex.Message);
                    return "Error al registrar usuario.";
                }
            }
        }

        [WebMethod]
        public string ActualizarUsuario(int usuarioID, string nombre, string apellido, string email)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Usuarios SET Nombre = @name, Apellido = @surname, Email = @email WHERE UsuarioID = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", usuarioID);
                        cmd.Parameters.AddWithValue("@name", nombre);
                        cmd.Parameters.AddWithValue("@surname", apellido);
                        cmd.Parameters.AddWithValue("@email", email);
                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0 ? "Usuario actualizado." : "Error: Usuario no encontrado.";
                    }
                }
                catch (Exception ex)
                {
                    RegistrarLogError("ActualizarUsuario", ex.Message);
                    return "Error al actualizar.";
                }
            }
        }

        [WebMethod]
        public string EliminarUsuario(int usuarioID)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM Usuarios WHERE UsuarioID = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", usuarioID);
                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0 ? "Usuario eliminado." : "Error: Usuario no encontrado.";
                    }
                }
                catch (Exception ex)
                {
                    RegistrarLogError("EliminarUsuario", ex.Message);
                    return "Error al eliminar.";
                }
            }
        }

        [WebMethod]
        public List<string> ObtenerUsuarios()
        {
            List<string> usuarios = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT UsuarioID, NombreUsuario, Email FROM Usuarios";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                usuarios.Add($"{reader["UsuarioID"]} - {reader["NombreUsuario"]} ({reader["Email"]})");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    RegistrarLogError("ObtenerUsuarios", ex.Message);
                }
            }
            return usuarios;
        }

        #endregion

        #region Productos

        [WebMethod]
        public string CrearProducto(string nombre, string descripcion, decimal precio, int stock, int categoriaID)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Productos (Nombre, Descripción, Precio, Stock, CategoriaID) VALUES (@name, @desc, @price, @stock, @catId)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", nombre);
                        cmd.Parameters.AddWithValue("@desc", descripcion);
                        cmd.Parameters.AddWithValue("@price", precio);
                        cmd.Parameters.AddWithValue("@stock", stock);
                        cmd.Parameters.AddWithValue("@catId", categoriaID);
                        cmd.ExecuteNonQuery();
                    }
                    return "Producto creado correctamente.";
                }
                catch (Exception ex)
                {
                    RegistrarLogError("CrearProducto", ex.Message);
                    return "Error al crear producto.";
                }
            }
        }

        [WebMethod]
        public string ActualizarProducto(int productoID, string nombre, string descripcion, decimal precio, int stock)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Productos SET Nombre = @name, Descripción = @desc, Precio = @price, Stock = @stock WHERE ProductoID = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", productoID);
                        cmd.Parameters.AddWithValue("@name", nombre);
                        cmd.Parameters.AddWithValue("@desc", descripcion);
                        cmd.Parameters.AddWithValue("@price", precio);
                        cmd.Parameters.AddWithValue("@stock", stock);
                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0 ? "Producto actualizado." : "Error: Producto no encontrado.";
                    }
                }
                catch (Exception ex)
                {
                    RegistrarLogError("ActualizarProducto", ex.Message);
                    return "Error al actualizar.";
                }
            }
        }

        [WebMethod]
        public string EliminarProducto(int productoID)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM Productos WHERE ProductoID = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", productoID);
                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0 ? "Producto eliminado." : "Error: Producto no encontrado.";
                    }
                }
                catch (Exception ex)
                {
                    RegistrarLogError("EliminarProducto", ex.Message);
                    return "Error al eliminar.";
                }
            }
        }

        [WebMethod]
        public List<string> ObtenerProductos()
        {
            List<string> productos = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ProductoID, Nombre, Precio, Stock FROM Productos";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productos.Add($"{reader["ProductoID"]} - {reader["Nombre"]} - ${reader["Precio"]} (Stock: {reader["Stock"]})");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    RegistrarLogError("ObtenerProductos", ex.Message);
                }
            }
            return productos;
        }

        [WebMethod]
        public List<string> BuscarProductos(string termino)
        {
            List<string> productos = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ProductoID, Nombre FROM Productos WHERE Nombre LIKE @term";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@term", "%" + termino + "%");
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productos.Add($"{reader["ProductoID"]} - {reader["Nombre"]}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    RegistrarLogError("BuscarProductos", ex.Message);
                }
            }
            return productos;
        }

        #endregion

        #region Pedidos

        [WebMethod]
        public string CrearPedido(int usuarioID, List<int> productosIDs, List<int> cantidades)
        {
            if (productosIDs.Count != cantidades.Count) return "Error: Datos de pedido inconsistentes.";

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                try
                {
                    // 1. Crear Pedido
                    string queryPedido = "INSERT INTO Pedidos (UsuarioID, FechaPedido, Estado) VALUES (@userId, NOW(), 'Pendiente'); SELECT LAST_INSERT_ID();";
                    MySqlCommand cmdPedido = new MySqlCommand(queryPedido, conn, trans);
                    cmdPedido.Parameters.AddWithValue("@userId", usuarioID);
                    int pedidoID = Convert.ToInt32(cmdPedido.ExecuteScalar());

                    for (int i = 0; i < productosIDs.Count; i++)
                    {
                        int prodId = productosIDs[i];
                        int cant = cantidades[i];

                        // 2. Obtener Precio y Stock actual
                        string queryInfo = "SELECT Precio, Stock FROM Productos WHERE ProductoID = @id";
                        MySqlCommand cmdInfo = new MySqlCommand(queryInfo, conn, trans);
                        cmdInfo.Parameters.AddWithValue("@id", prodId);
                        decimal precio = 0;
                        int stockActual = 0;
                        using (MySqlDataReader reader = cmdInfo.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                precio = Convert.ToDecimal(reader["Precio"]);
                                stockActual = Convert.ToInt32(reader["Stock"]);
                            }
                            else throw new Exception($"Producto {prodId} no encontrado.");
                        }

                        if (stockActual < cant) throw new Exception($"Stock insuficiente para el producto {prodId}.");

                        // 3. Insertar Detalle
                        string queryDetalle = "INSERT INTO DetallePedidos (PedidoID, ProductoID, Cantidad, PrecioUnitario) VALUES (@pId, @prodId, @cant, @price)";
                        MySqlCommand cmdDetalle = new MySqlCommand(queryDetalle, conn, trans);
                        cmdDetalle.Parameters.AddWithValue("@pId", pedidoID);
                        cmdDetalle.Parameters.AddWithValue("@prodId", prodId);
                        cmdDetalle.Parameters.AddWithValue("@cant", cant);
                        cmdDetalle.Parameters.AddWithValue("@price", precio);
                        cmdDetalle.ExecuteNonQuery();

                        // 4. Actualizar Stock
                        string queryStock = "UPDATE Productos SET Stock = Stock - @cant WHERE ProductoID = @prodId";
                        MySqlCommand cmdStock = new MySqlCommand(queryStock, conn, trans);
                        cmdStock.Parameters.AddWithValue("@cant", cant);
                        cmdStock.Parameters.AddWithValue("@prodId", prodId);
                        cmdStock.ExecuteNonQuery();
                    }

                    trans.Commit();
                    return $"Pedido #{pedidoID} creado con éxito.";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    RegistrarLogError("CrearPedido", ex.Message);
                    return "Error al procesar el pedido: " + ex.Message;
                }
            }
        }

        [WebMethod]
        public List<string> ObtenerPedidosPorUsuario(int usuarioID)
        {
            List<string> pedidos = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT PedidoID, FechaPedido, Estado FROM Pedidos WHERE UsuarioID = @uid";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", usuarioID);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                pedidos.Add($"ID: {reader["PedidoID"]} - Fecha: {reader["FechaPedido"]} - Estado: {reader["Estado"]}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    RegistrarLogError("ObtenerPedidosPorUsuario", ex.Message);
                }
            }
            return pedidos;
        }

        [WebMethod]
        public string ActualizarEstadoPedido(int pedidoID, string nuevoEstado)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Pedidos SET Estado = @estado WHERE PedidoID = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", pedidoID);
                        cmd.Parameters.AddWithValue("@estado", nuevoEstado);
                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0 ? "Estado actualizado." : "Pedido no encontrado.";
                    }
                }
                catch (Exception ex)
                {
                    RegistrarLogError("ActualizarEstadoPedido", ex.Message);
                    return "Error al actualizar estado.";
                }
            }
        }

        [WebMethod]
        public List<string> HistorialCompras(int usuarioID)
        {
            List<string> historial = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT p.PedidoID, p.FechaPedido, dp.ProductoID, pr.Nombre, dp.Cantidad, dp.PrecioUnitario 
                                     FROM Pedidos p 
                                     JOIN DetallePedidos dp ON p.PedidoID = dp.PedidoID 
                                     JOIN Productos pr ON dp.ProductoID = pr.ProductoID 
                                     WHERE p.UsuarioID = @uid";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", usuarioID);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                historial.Add($"Pedido: {reader["PedidoID"]} | {reader["FechaPedido"]} | Prod: {reader["Nombre"]} | Cant: {reader["Cantidad"]} | Precio: {reader["PrecioUnitario"]}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    RegistrarLogError("HistorialCompras", ex.Message);
                }
            }
            return historial;
        }

        #endregion

        #region Logs

        [WebMethod]
        public void RegistrarLogError(string evento, string mensaje)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Logs (Evento, Tipo) VALUES (@msg, 'Error')";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@msg", $"En {evento}: {mensaje}");
                        cmd.ExecuteNonQuery();
                    }
                }
                catch { /* Silent fail to avoid recursion */ }
            }
        }

        #endregion
    }
}
