using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TechMart.Models;
using System.Data.SqlClient;

namespace TechMart.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly string connectionString = @"Server=DESKTOP-TG6AOTQ\SQLEXPRESS;Initial Catalog=techmartDATA;Integrated Security=True;";

        [HttpPost]
        public ActionResult AgregarProveedor(Proveedores proveedor)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO proveedores (nombre, contacto, telefono, email) VALUES (@nombre, @contacto, @telefono, @email)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", proveedor.nombre);
                        command.Parameters.AddWithValue("@contacto", proveedor.direccion);
                        command.Parameters.AddWithValue("@telefono", proveedor.telefono);
                        command.Parameters.AddWithValue("@email", proveedor.correo);

                        command.ExecuteNonQuery();
                    }
                    return RedirectToAction("Proveedores", "Proveedores");
                }
            }
            List<Proveedores> proveedoresActualizados = ObtenerProveedoresDesdeBD();
            return View(proveedoresActualizados);
        }

        public ActionResult Proveedores()
        {
            List<Proveedores> proveedores = ObtenerProveedoresDesdeBD();
            return View(proveedores);
        }

        private List<Proveedores> ObtenerProveedoresDesdeBD()
        {
            List<Proveedores> proveedores = new List<Proveedores>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM proveedores";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Proveedores proveedor = new Proveedores
                            {
                                ID = (int)reader["ID"],
                                nombre = reader["nombre"].ToString(),
                                direccion = reader["contacto"].ToString(),
                                telefono = reader["telefono"].ToString(),
                                correo = reader["email"].ToString()
                            };

                            proveedores.Add(proveedor);
                        }
                    }
                }
            }

            return proveedores;
        }

        [HttpPost]
        public ActionResult EliminarProveedor(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM proveedores WHERE ID = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Proveedores", "Proveedores");
        }

        public IActionResult EditadoDeProveedores(int id)
        {
            // Obtener el proveedor correspondiente al ID
            Proveedores proveedor = ObtenerProveedorPorID(id);
            // Devolver la vista EditarProveedores con el modelo de proveedor
            return View("EditarProveedores", proveedor);
        }
        public ActionResult EditarProveedor(int id)
        {
            // Obtener el proveedor correspondiente al ID de la base de datos
            Proveedores proveedor = ObtenerProveedorPorID(id);
            return View(proveedor);
        }

        [HttpPost]
        public ActionResult ActualizarProveedor(Proveedores proveedor)
        {
            if (ModelState.IsValid)
            {
                // Actualizar la información del proveedor en la base de datos
                ActualizarProveedorEnBD(proveedor);
                return RedirectToAction("Proveedores", "Proveedores");
            }
            // Manejar el caso donde el modelo no es válido
            return View(proveedor);
        }

        private void ActualizarProveedorEnBD(Proveedores proveedor)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE proveedores SET nombre = @nombre, contacto = @contacto, telefono = @telefono, email = @email WHERE ID = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", proveedor.ID);
                    command.Parameters.AddWithValue("@nombre", proveedor.nombre);
                    command.Parameters.AddWithValue("@contacto", proveedor.direccion);
                    command.Parameters.AddWithValue("@telefono", proveedor.telefono);
                    command.Parameters.AddWithValue("@email", proveedor.correo);

                    command.ExecuteNonQuery();
                }
            }
        }

        private Proveedores ObtenerProveedorPorID(int id)
        {
            Proveedores proveedor = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM proveedores WHERE ID = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            proveedor = new Proveedores
                            {
                                ID = (int)reader["ID"],
                                nombre = reader["nombre"].ToString(),
                                direccion = reader["contacto"].ToString(),
                                telefono = reader["telefono"].ToString(),
                                correo = reader["email"].ToString()
                            };
                        }
                    }
                }
            }

            return proveedor;
        }
    }
}
