using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TechMart.Models;
using System.Data.SqlClient;
using System.Diagnostics;
using TechMart.Persistence;

namespace TechMart.Controllers
{
    public class ProductsController : Controller
    {
        private readonly string connectionString = @"Server=DESKTOP-TG6AOTQ\SQLEXPRESS;Initial Catalog=techmartDATA;Integrated Security=True;";

        // Método para agregar un producto
        [HttpPost]
        public ActionResult AgregarProducto(Productos producto)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO productos (nombre, precio, stock, descripcion) VALUES (@nombre, @precio, @stock, @descripcion)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", producto.nombre);
                        command.Parameters.AddWithValue("@precio", producto.precio);
                        command.Parameters.AddWithValue("@stock", producto.stock);
                        command.Parameters.AddWithValue("@descripcion", producto.descripcion);

                        command.ExecuteNonQuery();
                    }
                    return RedirectToAction("Inventario", "Products");
                }
            }
            List<Productos> productosActualizados = ObtenerProductosDesdeBD();
            return View(productosActualizados);
        }

        // Vista para mostrar el inventario de productos
        public ActionResult Inventario()
        {
            List<Productos> productos = ObtenerProductosDesdeBD();
            return View(productos);
        }

        // Método para obtener los productos desde la base de datos
        private List<Productos> ObtenerProductosDesdeBD()
        {
            List<Productos> productos = new List<Productos>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM productos";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Productos producto = new Productos
                            {
                                ID = (int)reader["ID"],
                                nombre = reader["nombre"].ToString(),
                                precio = (decimal)reader["precio"],
                                stock = (int)reader["stock"],
                                descripcion = (string)reader["descripcion"]
                            };

                            productos.Add(producto);
                        }
                    }
                }
            }

            return productos;
        }

        // Método para eliminar un producto
        [HttpPost]
        public ActionResult EliminarProducto(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM productos WHERE ID = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Inventario", "Products");
        }

        public IActionResult EditadoDeProductos(int id)
        {
            Productos producto = ObtenerProductoPorID(id);
            return View("EditarProducto", producto);
        }

        // Método para mostrar la vista de editar producto
        public ActionResult EditarProducto(int id)
        {
            // Obtener el producto correspondiente al ID de la base de datos
            Productos producto = ObtenerProductoPorID(id);
            return View(producto);
        }

        // Método para actualizar un producto
        [HttpPost]
        public ActionResult ActualizarProducto(Productos producto)
        {
            if (ModelState.IsValid)
            {
                // Actualizar la información del producto en la base de datos
                ActualizarProductoEnBD(producto);
                return RedirectToAction("Inventario", "Products");
            }
            // Manejar el caso donde el modelo no es válido
            return View(producto);
        }

        // Método para actualizar un producto en la base de datos
        private void ActualizarProductoEnBD(Productos producto)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE productos SET nombre = @nombre, precio = @precio, stock = @stock, descripcion = @descripcion WHERE ID = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", producto.ID);
                    command.Parameters.AddWithValue("@nombre", producto.nombre);
                    command.Parameters.AddWithValue("@precio", producto.precio);
                    command.Parameters.AddWithValue("@stock", producto.stock);
                    command.Parameters.AddWithValue("@descripcion", producto.descripcion);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Método para obtener un producto por ID desde la base de datos
        private Productos ObtenerProductoPorID(int id)
        {
            Productos producto = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM productos WHERE ID = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            producto = new Productos
                            {
                                ID = (int)reader["ID"],
                                nombre = reader["nombre"].ToString(),
                                precio = (decimal)reader["precio"],
                                stock = (int)reader["stock"],
                                descripcion = (string)reader["descripcion"]
                            };
                        }
                    }
                }
            }

            return producto;
        }

        // Método para mostrar la vista de editar productos
        public IActionResult EditarDeProductos(int id)
        {
            // Obtener el producto correspondiente al ID
            Productos producto = ObtenerProductoPorID(id);
            // Devolver la vista EditarProductos con el modelo de producto
            return View("EditarProductos", producto);
        }
    }
}
