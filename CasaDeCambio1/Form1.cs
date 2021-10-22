using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CasaDeCambio1
{
    public partial class form1 : Form
    {
        SqlConnection conexion = new SqlConnection();
        SqlCommand comando = new SqlCommand();
        DataTable tabla = new DataTable();
        public form1()
        {
            InitializeComponent();
            this.cons1.SetToolTip(btnConsulta1, "Mostrar los tipos de transacciones realizadas entre las fechas 2020 y 2021 y ordenar el nombre de los cajeros de forma alfabética.");
            this.cons2.SetToolTip(btnConsulta2, "Clientes que hicieron transacciones con forma de pago “efectivo” entre el 20 / 03 / 2019 y 15 / 06 / 2020");
            this.cons3.SetToolTip(btnConsulta3, "Mostrar recibos realizados por los cajeros mayores de 25 años a los clientes con tipo de identificación “pasaporte” en los años 2019 y 2020.");
            this.cons4.SetToolTip(btnConsulta4, "Transacciones hechas a clientes menores de 35 años y con motivo de “Viaje familiar");
           // this.cons5.SetToolTip(btnConsulta5, "Mostrar clientes que vivan en la Provincia de Buenos Aires, que hayan comprado dolares y que sea con motivo de “Viaje al exterior");
            this.cons5.SetToolTip(btnConsulta6, "Mostrar clientes que vivan en la Provincia de Córdoba y que el motivo de transacción sea por “asuntos personales");
            this.cons6.SetToolTip(btnConsulta7, "Mostrar el valor de venta y compra de las transacciones con motivo “asuntos personales” para los clientes que tengan apellidos entre la “A y la M”");
            this.cons7.SetToolTip(btnConsulta8, "Listar el nombre de la calle, del barrio, y de la localidad de los cajeros nacidos entre febrero de 1999 y julio de 2001");
            this.cons8.SetToolTip(btnConsulta9, "Cajeros que hayan atendido clientes en los 3 primeros meses del año 2020 y aquello que no han realizado ninguna atención.");
            this.cons9.SetToolTip(btnConsulta10,"Mostrar provincia, localidad, barrio y calle de los clientes con apellidos que empiecen con “A,D,J y K” que hayan tengan como fecha de nacimiento mayor a 1990. También mostrar a los clientes que tengan NULL en el campo “teléfono”");
        }




        private void Form1_Load(object sender, EventArgs e)
        {
            conexion.ConnectionString = @"Data Source=DESKTOP-1D93NK6\SQLEXPRESS01;Initial Catalog=casa_de_cambio_5;Integrated Security=True";
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
           
            conexion.Close();
            
        }

        private void cboConsultas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnConsulta1_Click(object sender, EventArgs e)
        {   /*2-Mostrar los tipos de transacciones realizadas entre las fechas 2020 y 2021 y ordenar
              el nombre de los cajeros de forma alfabética.
             * */
            tabla = new DataTable();
            conexion.ConnectionString = @"Data Source=DESKTOP-1D93NK6\SQLEXPRESS01;Initial Catalog=casa_de_cambio_5;Integrated Security=True";
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
            comando.CommandText = @"select tt.nombre 'Tipo de transaccion',t.id_transaccion 'numero de transaccion',r.fecha_hora 'Fecha y Hora', c.apellido + ' ' + c.nombre as 'Cajero'
from transacciones t 
join recibos r on t.id_recibo=r.id_recibo 
join cajeros c on c.id_cajero=r.id_cajero
join tipos_de_transaccion tt on t.id_tipo_transaccion=tt.id_tipo_transaccion
where year(fecha_hora)between 2020 and 2021 
order by 4 asc";
            tabla.Load(comando.ExecuteReader());
            dataGridView1.ReadOnly = true;
            dataGridView1.DataSource = tabla;
            conexion.Close();

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnConsulta2_Click(object sender, EventArgs e)
        {/*4-Clientes que hicieron transacciones con forma de pago “efectivo” entre el
              20/03/2019 y 15/06/2020
          * */
            tabla = new DataTable();  
            conexion.ConnectionString = @"Data Source=DESKTOP-1D93NK6\SQLEXPRESS01;Initial Catalog=casa_de_cambio_5;Integrated Security=True";
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
            comando.CommandText = @"set dateformat dmy
select upper(apellido) + ', '+ nombre 'Cliente', r.id_recibo 'Número de recibo',
forma_pago 'Forma de Pago', fecha_hora 'Fecha'
from recibos r
join transacciones t on r.id_recibo = t.id_recibo
join clientes c on r.id_cliente = c.id_cliente
join formas_pago f on t.id_forma_pago=f.id_forma_pago
where f.forma_pago like 'Efectivo'
and fecha_hora between '20/03/2019' and '15/06/2020'
";
            tabla.Load(comando.ExecuteReader());
            conexion.Close();
            dataGridView1.ReadOnly = true;
            dataGridView1.DataSource = tabla;
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnConsulta3_Click(object sender, EventArgs e)
        {/*12 - Mostrar recibos realizados por los cajeros mayores de 25 años a los clientes con tipo
                de identificación “pasaporte” en los años 2019 y 2020.
          * */
            tabla = new DataTable();
            conexion.ConnectionString = @"Data Source=DESKTOP-1D93NK6\SQLEXPRESS01;Initial Catalog=casa_de_cambio_5;Integrated Security=True";
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
            comando.CommandText = @"SET DATEFORMAT dmy
                                    SELECT re.id_recibo 'Numero de recibo', upper(ca.apellido)+' '+ca.nombre 'Cajero', upper(c.apellido)+' '+c.nombre 'Cliente',re.fecha_hora 'Fecha y Hora'
                                    FROM recibos re 
                                    INNER JOIN cajeros ca ON re.id_cajero = ca.id_cajero
                                    INNER JOIN clientes c ON re.id_cliente = c.id_cliente
                                    INNER JOIN tipos_identificacion tid ON c.id_identificacion = tid.id_identificacion
                                    WHERE FLOOR((CAST (GetDate() AS INTEGER) - CAST(ca.fec_nac AS INTEGER)) / 365.25) > 25
                                    AND tid.tipo_identificacion = 'Pasaporte extranjero' 
                                    AND (re.fecha_hora BETWEEN '01/01/2019' AND '31/12/2020') ";
            tabla.Load(comando.ExecuteReader());
            conexion.Close();
            dataGridView1.ReadOnly = true;
            dataGridView1.DataSource = tabla;
        }

        private void btnConsulta4_Click(object sender, EventArgs e)
        {/*19 - Transacciones hechas a clientes menores de 35 años y con motivo de “Viaje familiar”.
          */

            tabla = new DataTable();
            conexion.ConnectionString = @"Data Source=DESKTOP-1D93NK6\SQLEXPRESS01;Initial Catalog=casa_de_cambio_5;Integrated Security=True";
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
            comando.CommandText = @"select t.id_transaccion 'Numero de transaccion', c.apellido+' '+c.nombre 'Cliente',fec_nac 'Fecha de Nacimiento',fecha_hora 'Fecha y Hora',motivo
from transacciones t, recibos r, clientes c, motivos m
where t.id_recibo=r.id_recibo 
and r.id_cliente=c.id_cliente
and t.id_motivo=m.id_motivo
and FLOOR((CAST (GetDate() AS INTEGER) - CAST(c.fec_nac AS INTEGER)) / 365.25) < 35
and m.motivo like 'Viaje familiar%'
 ";
            tabla.Load(comando.ExecuteReader());
            conexion.Close();
            dataGridView1.ReadOnly = true;
            dataGridView1.DataSource = tabla;
        }

        private void btnConsulta5_Click(object sender, EventArgs e)
        {/*20 - Mostrar clientes que vivan en la Provincia de Buenos Aires, que hayan comprado
                dolares y que sea con motivo de “Viaje al exterior”.
          */
            //tabla = new DataTable();
            //conexion.ConnectionString = @"Data Source=DESKTOP-1D93NK6\SQLEXPRESS01;Initial Catalog=casa_de_cambio_5;Integrated Security=True";
            //conexion.Open();
            //comando.Connection = conexion;
            //comando.CommandType = CommandType.Text;
            //comando.CommandText = @"select c.apellido+' '+c.nombre 'Cliente', ti.tipo_identificacion 'Tipo', c.nro_identificacion
            //                        from transacciones t, tipos_moneda tm, recibos r, clientes c, provincias p, motivos m, tipos_identificacion ti
            //                        where t.id_tipo_moneda_egreso=tm.id_tipo_moneda
            //                        and t.id_recibo=r.id_recibo 
            //                        and t.id_motivo=m.id_motivo
            //                        and r.id_recibo=c.id_cliente
            //                        and c.id_cliente=p.id_provincia
            //                        and c.id_identificacion=ti.id_identificacion
            //                        and provincia like 'Buenos%'
            //                        and motivo like 'Viaje al ext%' ";
            //tabla.Load(comando.ExecuteReader());
            //conexion.Close();
            //dataGridView1.ReadOnly = true;
            //dataGridView1.DataSource = tabla;
        }

        private void btnConsulta6_Click(object sender, EventArgs e)
        {
            /*10 - Mostrar clientes que vivan en la Provincia de Córdoba y 
             * que el motivo de transacción sea por “asuntos personales”
             */
            tabla = new DataTable();
            conexion.ConnectionString = @"Data Source=DESKTOP-1D93NK6\SQLEXPRESS01;Initial Catalog=casa_de_cambio_5;Integrated Security=True";
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
            comando.CommandText = @"select tr.id_transaccion 'Numero de transaccion', cl.razon_social, tid.tipo_identificacion TipoID, cl.nro_identificacion, cl.apellido, cl.nombre, mo.motivo, pro.provincia
from transacciones tr
INNER JOIN recibos re on tr.id_recibo = re.id_recibo
INNER JOIN clientes cl on re.id_cliente = cl.id_cliente
INNER JOIN calles cal on cal.id_calle = cl.id_calle
INNER JOIN barrios ba on ba.id_barrio = cal.id_barrio
INNER JOIN localidades lo on lo.id_localidad = ba.id_localidad
INNER JOIN provincias pro on pro.id_provincia = lo.id_provincia
INNER JOIN motivos mo on tr.id_motivo = mo.id_motivo
INNER JOIN tipos_identificacion tid ON cl.id_identificacion = tid.id_identificacion
WHERE pro.provincia LIKE 'C_rdoba'
AND mo.motivo = 'Asuntos personales' 
";
            tabla.Load(comando.ExecuteReader());
            conexion.Close();
            dataGridView1.ReadOnly = true;
            dataGridView1.DataSource = tabla;
        }

        private void btnConsulta7_Click(object sender, EventArgs e)
        {//19 ---1
           // --Mostrar el valor de venta y compra de las transacciones con motivo “asuntos
           // --personales” para los clientes que tengan apellidos entre la “A y la M”

            tabla = new DataTable();
            conexion.ConnectionString = @"Data Source=DESKTOP-1D93NK6\SQLEXPRESS01;Initial Catalog=casa_de_cambio_5;Integrated Security=True";
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
            comando.CommandText = @"select cli.apellido + '  ' + cli.nombre 'Cliente', tr.id_transaccion 'Numero de transaccion', tr.monto_egreso, tme.nombre 'moneda egreso' , tr.monto_ingreso, tmi.nombre 'moneda ingreso', m.motivo  
from transacciones tr
join motivos m on tr.id_motivo=m.id_motivo 
join recibos re on tr.id_recibo = re.id_recibo
join clientes cli on cli.id_cliente = re.id_cliente
join tipos_moneda tme on tr.id_tipo_moneda_egreso = tme.id_tipo_moneda
join tipos_moneda tmi on tr.id_tipo_moneda_ingreso = tmi.id_tipo_moneda
where m.motivo = 'Asuntos personales' 
and apellido like '[A-M]%'

                                     ";
            tabla.Load(comando.ExecuteReader());
            conexion.Close();
            dataGridView1.ReadOnly = true;
            dataGridView1.DataSource = tabla;
        }

        private void btnConsulta8_Click(object sender, EventArgs e)
        { //15 - Listar el nombre de la calle, del barrio, y de la localidad de los cajeros nacidos entre febrero de 1999 y julio de 2001
            tabla = new DataTable();
            conexion.ConnectionString = @"Data Source=DESKTOP-1D93NK6\SQLEXPRESS01;Initial Catalog=casa_de_cambio_5;Integrated Security=True";
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
            comando.CommandText = @"select ca.nombre+' '+ca.apellido Cajero, c.nombre 'Calle', ba.barrio 'Barrio', lo.localidad 'Localidad', ca.fec_nac 'Fecha de Nacimiento'
from cajeros ca
join calles c on c.id_calle=ca.id_calle
join barrios ba on ba.id_barrio=c.id_barrio
join localidades lo on lo.id_localidad=ba.id_localidad 
where ca.fec_nac between '1999/2/1' and '2001/7/1'";
            tabla.Load(comando.ExecuteReader());
            conexion.Close();
            dataGridView1.ReadOnly = true;
            dataGridView1.DataSource = tabla;
        }

        private void btnConsulta9_Click(object sender, EventArgs e)
        {//5-Cajeros que hayan atendido clientes en los 3 primeros meses del año 2020 
            tabla = new DataTable();
            conexion.ConnectionString = @"Data Source=DESKTOP-1D93NK6\SQLEXPRESS01;Initial Catalog=casa_de_cambio_5;Integrated Security=True";
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
            comando.CommandText = @"set dateformat dmy select upper(c.apellido) + ', '+ c.nombre Cajero, r.id_recibo 'Número de recibo', r.fecha_hora 'Fecha de Atencion'
from recibos r right outer join cajeros c on r.id_cajero = c.id_cajero
where r.fecha_hora BETWEEN '01-01-2020' and '31-03-2020' or r.fecha_hora is null
order by Cajero ";
            tabla.Load(comando.ExecuteReader());
            conexion.Close();
            dataGridView1.ReadOnly = true;
            dataGridView1.DataSource = tabla;
        }

        private void btnConsulta10_Click(object sender, EventArgs e)
        { //13-Mostrar provincia, localidad, barrio y calle de los clientes con apellidos que empiecen con “A,D,J y K” que hayan tengan
          ////como fecha de nacimiento mayor a 1990. También mostrar a los clientes que tengan NULL en el campo “teléfono”.  
            tabla = new DataTable();
            conexion.ConnectionString = @"Data Source=DESKTOP-1D93NK6\SQLEXPRESS01;Initial Catalog=casa_de_cambio_5;Integrated Security=True";
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
            comando.CommandText = @"select apellido+' '+cl.nombre 'Cliente', provincia 'Provincia', localidad 'Localida', barrio 'Barrio', ca.nombre 'Calle'
                                    from clientes cl
                                    join calles ca on cl.id_calle=ca.id_calle
                                    join barrios ba on ca.id_barrio=ba.id_barrio
                                    join localidades lo on ba.id_localidad=lo.id_localidad
                                    join provincias pr on lo.id_provincia=pr.id_provincia
                                    where cl.apellido like '[A,D,J,K]%' and year (fec_nac) = 1990 and cl.telefono is null
                                     ";
            tabla.Load(comando.ExecuteReader());
            conexion.Close();
            dataGridView1.ReadOnly = true;
            dataGridView1.DataSource = tabla;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            
                DialogResult res = MessageBox.Show("¿Estas seguro de que queres salir?", "Cerrar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (res == DialogResult.Yes) { this.Dispose(); }

            

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Rows.Clear();
        }

        private void btnCreditos_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"112659 - Mangini, Agustín
113044 - Miliano González, Ivo
113191 - Moisés, Franco
112837 - Molina, Matías Gabriel
112942 - Monje, Sofía Florencia
113491 - Monticoli, Pablo Javier
113076 - Vega, Nehuen", "Integrantes");
        }
    }
}
