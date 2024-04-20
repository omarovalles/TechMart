using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace TechMart.Controllers
{
	public class LoginController : Controller
	{
		static string connectionString = @"Server=DESKTOP-TG6AOTQ\SQLEXPRESS;Initial Catalog=techmartDATA;Integrated Security=True;";

		[HttpPost]
		public ActionResult Index(string nombre, string contraseña)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();

				string query = "SELECT cargo FROM usuarios WHERE nombre = @nombre AND contraseña = @contraseña";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@nombre", nombre);
					command.Parameters.AddWithValue("@contraseña", contraseña);
					string cargo = command.ExecuteScalar()?.ToString();

					if (cargo == "administrador")
					{
						// Redirigir a la vista del administrador
						return RedirectToAction("Adminlobby", "Home");
					}
					else if(cargo == "usuarionormal")
					{
						//Redirigir a la vista de usuario normal
						return RedirectToAction("NormalLobby", "Home");
					}
					else
					{
						// Credenciales incorrectas o usuario no encontrado
						ViewBag.ErrorMessage = "Credenciales incorrectas o usuario no encontrado.";
						return View();
					}
				}
			}
		}

		public IActionResult Adminlobby()
		{
			// Aquí puedes poner el código para la vista Adminlobby
			return View();
		}
	}
}
