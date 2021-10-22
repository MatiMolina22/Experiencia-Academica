using RecetasSLN.dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasSLN.datos
{
    class RecetaDao : IRecetaDao
    {

        private string cadenaConexion;
        public RecetaDao()
        {
            cadenaConexion = Properties.Resources.strCadena_db_receta;
        }
        public List<Ingredientes> ConsultarIngrediente()
        {
            List<Ingredientes> lst = new List<Ingredientes>();
            DataTable tabla = HelperDao.ObtenerInstancia().ConsultaSQL("SP_CONSULTAR_INGREDIENTES");
            foreach (DataRow row in tabla.Rows)
            {
                Ingredientes oIngredientes = new Ingredientes();
                oIngredientes.IngredienteId = Convert.ToInt32(row["id_ingrediente"].ToString());
                oIngredientes.Nombre = row["n_ingrediente"].ToString();
                oIngredientes.Unidad = row["unidad_medida"].ToString();
                lst.Add(oIngredientes);
            }
            return lst;
        }//listo!!
        public bool InsertarReceta(Receta oReceta) //listo
        {
			bool resultado = true;
			SqlConnection cnn = new SqlConnection();
			SqlTransaction trans = null;

			try
			{
				cnn.ConnectionString = cadenaConexion;
				cnn.Open();
				trans = cnn.BeginTransaction();
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = cnn;
				cmd.Transaction = trans;
				cmd.CommandText = "SP_INSERTAR_RECETA";
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@id_receta", oReceta.RecetaNro);
				cmd.Parameters.AddWithValue("@tipo_receta", oReceta.TipoReceta);
				cmd.Parameters.AddWithValue("@nombre", oReceta.Nombre);
				cmd.Parameters.AddWithValue("@cheff", oReceta.Cheff);
				cmd.ExecuteNonQuery();
				int detalleNro = 1;

				foreach (DetalleReceta item in oReceta.Detalles)
				{
					SqlCommand cmdDet = new SqlCommand();
					cmdDet.Connection = cnn;
					cmdDet.Transaction = trans;
					cmdDet.CommandText = "SP_INSERTAR_DETALLES";
					cmdDet.CommandType = CommandType.StoredProcedure;
					cmdDet.Parameters.AddWithValue("@id_receta", oReceta.RecetaNro);
					cmdDet.Parameters.AddWithValue("@id_ingrediente", item.Ingredientes.IngredienteId);
					cmdDet.Parameters.AddWithValue("@cantidad", item.Cantidad);
					cmdDet.ExecuteNonQuery();
					detalleNro++;
				}
				trans.Commit();
			}
			catch (Exception)
			{
				//en caso que quiera saber que error ocurre
				//MessageBox.Show("error: " + E.Message);
				trans.Rollback();
				resultado = false;
			}
			finally
			{
				if (cnn != null && cnn.State == ConnectionState.Open) cnn.Close();
			}
			return resultado;
		}
        public int ObtenerProximoNumero()
        {
			return HelperDao.ObtenerInstancia().ProximoID("SP_PROXIMO_ID", "@next");
		}
    }
}
