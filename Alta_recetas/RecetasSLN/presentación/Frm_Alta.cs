
using RecetasSLN.datos;
using RecetasSLN.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecetasSLN
{

    //nombre : MATIAS MOLINA  - LEGAJO : 112837
    public partial class Frm_Alta : Form
    {
        private Receta oReceta;
        private RecetaDao gestor;
        private Accion modo;
        public enum Accion
        {
            Create,
            Read,
            Update,
            Delete
        }

        public Frm_Alta()
        {
            InitializeComponent();
            oReceta = new Receta();
            gestor = new RecetaDao();

        }
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                oReceta.Nombre = txtNombre.Text;
                oReceta.Cheff = txtCheff.Text;
                oReceta.RecetaNro = gestor.ObtenerProximoNumero();
                oReceta.TipoReceta = cboTipo.SelectedIndex + 1;
                if (gestor.InsertarReceta(oReceta))
                {
                    MessageBox.Show("Se grabó la receta con exito!", "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Limpiar();
                    lblNro.Text = "Receta N° " + gestor.ObtenerProximoNumero();
                    CalcularTotales();
                }
               CalcularTotales();   
            }
            else
            {
                MessageBox.Show("No se pudo grabar la Receta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CalcularTotales()
        {
            lblTotal_ing.Text = oReceta.CalcularTotal().ToString();
        }//listo
        private bool Validar()
        {
            if (txtNombre.Text == "")
            {
                MessageBox.Show("Debe especificar un NOMBRE DE RECETA.", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNombre.Focus();
                return false;
            }
            if (cboTipo.Text.Equals(string.Empty))
            {
                MessageBox.Show("Debe seleccionar un tipo de receta", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboTipo.Focus();
                return false;
            }
            if (dgvDetalles.Rows.Count == 0)
            {
                MessageBox.Show("Debe ingresar al menos un detalle.", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboProducto.Focus();
                return false;
            }
            return true;
        }
        private void Limpiar()
        {
            oReceta = new Receta();
            txtNombre.Text = "";
            cboTipo.SelectedIndex = 0;
            cboProducto.SelectedIndex = 0;
            nudCantidad.Value = 1;
            dgvDetalles.Rows.Clear();
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cancelar?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();

            }
            else
            {
                return;
            }
        }
        private void Frm_Alta_Presupuesto_Load(object sender, EventArgs e)
        {
            nudCantidad.Text = "1";
            if (modo.Equals(Accion.Create))
            {
                CargarCombo();//listo
                lblNro.Text += gestor.ObtenerProximoNumero();//listo
                txtNombre.Text = "Nombre de la receta";
                txtCheff.Text = "Nombre del cheff";
                this.CargarCombo();//listo
            }
        }
        private void CargarCombo()
        {
            List<Ingredientes> lst = gestor.ConsultarIngrediente();
            cboProducto.DataSource = lst;
            cboProducto.ValueMember = "IngredienteId";
            cboProducto.DisplayMember = "Nombre";
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cboTipo.Text.Equals(string.Empty))
            {
                MessageBox.Show("Debe seleccionar un tipo de receta", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboTipo.Focus();
                return;
            }
            if (cboProducto.Text.Equals(string.Empty))
            {
                MessageBox.Show("Debe seleccionar un producto", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
           // ExisteProductoEnGrilla(cboProducto.Text);
            foreach (DataGridViewRow row in dgvDetalles.Rows)
            {
                if (row.Cells["ingrediente"].Value.ToString().Equals(cboProducto.Text))
                {
                    MessageBox.Show("Este ingrediente ya se encuentra en la tabla", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            Ingredientes oIngrediente = (Ingredientes)cboProducto.SelectedItem;
            DetalleReceta detalle = new DetalleReceta();
            detalle.Ingredientes = oIngrediente;
            detalle.Cantidad = Convert.ToInt32(nudCantidad.Text);
            oReceta.AgregarDetalle(detalle);
            dgvDetalles.Rows.Add(new object[] { oIngrediente.IngredienteId, oIngrediente.Nombre, detalle.Cantidad });

            CalcularTotales();

        }//VER!!!!
        private bool ExisteProductoEnGrilla(string text)
        {
            foreach (DataGridViewRow fila in dgvDetalles.Rows)
            {
                if (fila.Cells["ingrediente"].Value.Equals(text))
                    return true;
            }
            return false;
        }
        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalles.CurrentCell.ColumnIndex == 3)
            {
                oReceta.QuitarDetalle(dgvDetalles.CurrentRow.Index);
                dgvDetalles.Rows.Remove(dgvDetalles.CurrentRow);
                
            }
        }
    }
}
