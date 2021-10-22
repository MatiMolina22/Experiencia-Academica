using RecetasSLN.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasSLN.datos
{
    interface IRecetaDao
    {
        List<Ingredientes> ConsultarIngrediente();
        bool InsertarReceta(Receta oReceta);
        int ObtenerProximoNumero();
    }
}
