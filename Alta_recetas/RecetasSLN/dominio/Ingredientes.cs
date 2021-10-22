using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasSLN.dominio
{
    class Ingredientes
    {
		
		public int IngredienteId { get; set; }
		public string Nombre { get; set; }
		public string Unidad { get; set; }
	
		public Ingredientes()
		{
			IngredienteId = 0;
			Nombre=string.Empty;
			Unidad = "";
		}

        public Ingredientes(int ingredienteId, string nombre, string unidad)
        {
            IngredienteId = ingredienteId;
            Nombre = nombre;
            this.Unidad = unidad;
        }

        public override string ToString()
		{
			return Nombre;
		}
	}
}
