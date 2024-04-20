using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechMart.Domain
{
    public class Articulos
    {
        public int ID { get; set; }
        public string nombre { get; set; }

        public decimal precio { get; set; }

        public int stock { get; set; }

        public string descripcion { get; set; }
    }
}
