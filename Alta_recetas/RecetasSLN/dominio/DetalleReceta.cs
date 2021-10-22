using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasSLN.dominio
{
    class DetalleReceta
    {
       public Ingredientes Ingredientes { get; set; }
		
		public int Cantidad { get; set; }

		public DetalleReceta(Ingredientes ingrediente, int cantidad)
		{
			Ingredientes = ingrediente;
			Cantidad = cantidad;
		}
		public DetalleReceta()
		{
			Ingredientes = null;
			Cantidad = 0;
		}

	}
}
