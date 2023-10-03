using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CapaPresentacion.Utilidades;

using CapaEntidad;
using CapaNegocio;
using System.Globalization;

namespace CapaPresentacion
{
    public partial class frmUsuarios : Form
    {
        public frmUsuarios()
        {
            InitializeComponent();
        }

        private void frmUsuarios_Load(object sender, EventArgs e)
        {

            cboestado.Items.Add(new OpcionCombo() { Valor = 1 , Texto = "Activo"});  
            cboestado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "No Activo" });
            cboestado.DisplayMember = "Texto";
            cboestado.ValueMember = "Valor";
            cboestado.SelectedIndex = 0;

            List<Rol> listaRol = new CN_Rol().Listar();

            foreach (Rol item in listaRol)
            {
                cborol.Items.Add(new OpcionCombo() { Valor = item.IdRol, Texto = item.Descripcion });
                

            }
            cborol.DisplayMember = "Texto";
            cborol.ValueMember = "Valor";
            cborol.SelectedIndex = 0;

            foreach (DataGridViewColumn columna in dgvdata.Columns) {

                if (columna.Visible == true && columna.Name != "btnseleccionar") {
                    cbobusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }


            }
            cbobusqueda.DisplayMember = "Texto";
            cbobusqueda.ValueMember = "Valor";
            cbobusqueda.SelectedIndex = 0;


            //Mostrar todos los usuarios
            List<Usuario> listaUsuario = new CN_Usuario().Listar();

            foreach (Usuario item in listaUsuario)
            {

                dgvdata.Rows.Add(new object[] {"",item.IdUsuario,item.Documento,item.Nombre,item.Apellido,item.Correo,item.Telefono,item.Direccion,item.Edad,item.FechaNacimiento,item.Clave,
                item.oRol.IdRol,
                item.oRol.Descripcion,
                // item.Estado == true ? 1 : 0 ,       //item.Estado == true ? 1 : 0 ,
                item.Estado == true ? "Activo" : "No Activo"  //item.Estado == true ? "Activo" : "No Activo" 
            });

            }
            




        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            // Verificar si algún campo requerido está vacío
            if (string.IsNullOrWhiteSpace(txtdocumento.Text) || string.IsNullOrWhiteSpace(txtnombre.Text)
                || string.IsNullOrWhiteSpace(txtapellido.Text) || string.IsNullOrWhiteSpace(txtcorreo.Text)
                || string.IsNullOrWhiteSpace(txttelefono.Text) || string.IsNullOrWhiteSpace(txtdireccion.Text)
                || string.IsNullOrWhiteSpace(txtedad.Text) || string.IsNullOrWhiteSpace(txtfechanacimiento.Text)
                || string.IsNullOrWhiteSpace(txtclave.Text) || cborol.SelectedItem == null || cboestado.SelectedItem == null)
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos antes de guardar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Evita que se continúe con la operación de guardado
            }

            Usuario objusuario = new Usuario()
            {
                IdUsuario = Convert.ToInt32(txtid.Text),
                Documento = txtdocumento.Text,
                Nombre = txtnombre.Text,
                Apellido = txtapellido.Text,
                Correo = txtcorreo.Text,
                Telefono = txttelefono.Text,
                Direccion = txtdireccion.Text,
                Edad = Convert.ToInt32(txtedad.Text),
                FechaNacimiento = txtfechanacimiento.Text,
                Clave = txtclave.Text,
                oRol = new Rol() { IdRol = Convert.ToInt32(((OpcionCombo)cborol.SelectedItem).Valor) },
                Estado = Convert.ToInt32(((OpcionCombo)cboestado.SelectedItem).Valor) == 1 ? true : false
            };

            if (objusuario.IdUsuario == 0)
            {
                int idusuariogenerado = new CN_Usuario().Registrar(objusuario, out mensaje);

                if (idusuariogenerado != 0)
                {
                    dgvdata.Rows.Add(new object[] {"",idusuariogenerado,txtdocumento.Text,txtnombre.Text,txtapellido.Text,txtcorreo.Text,txttelefono.Text,txtdireccion.Text,txtedad.Text,txtfechanacimiento.Text,txtclave.Text,
            ((OpcionCombo)cborol.SelectedItem).Valor.ToString(),
            ((OpcionCombo)cborol.SelectedItem).Texto.ToString(),
            ((OpcionCombo)cboestado.SelectedItem).Valor.ToString(),
            ((OpcionCombo)cboestado.SelectedItem).Texto.ToString(),
            });

                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje);
                }

            }
            else
            {
                bool resultado = new CN_Usuario().Editar(objusuario, out mensaje);

                if (resultado)
                {
                    DataGridViewRow row = dgvdata.Rows[Convert.ToInt32(txtindice.Text)];
                    row.Cells["Id"].Value = txtid.Text;
                    row.Cells["Documento"].Value = txtdocumento.Text;
                    row.Cells["Nombre"].Value = txtnombre.Text;
                    row.Cells["Apellido"].Value = txtapellido.Text;
                    row.Cells["Correo"].Value = txtcorreo.Text;
                    row.Cells["Telefono"].Value = txttelefono.Text;
                    row.Cells["Direccion"].Value = txtdireccion.Text;
                    row.Cells["Edad"].Value = txtedad.Text;
                    row.Cells["FechaNacimiento"].Value = txtfechanacimiento.Text;
                    row.Cells["Clave"].Value = txtclave.Text;
                    row.Cells["IdRol"].Value = ((OpcionCombo)cborol.SelectedItem).Valor.ToString();
                    row.Cells["Rol"].Value = ((OpcionCombo)cborol.SelectedItem).Texto.ToString();
                    row.Cells["EstadoValor"].Value = ((OpcionCombo)cboestado.SelectedItem).Valor.ToString();
                    row.Cells["Estado"].Value = ((OpcionCombo)cboestado.SelectedItem).Texto.ToString();

                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje);
                }
            }

        }






        private void Limpiar()
        {
            txtindice.Text = "-1";
            txtid.Text = "0";
            txtdocumento.Text = "";
            txtnombre.Text = "";
            txtapellido.Text = "";
            txtcorreo.Text = "";
            txtedad.Text = "";
            txtfechanacimiento.Text = "";
            txttelefono.Text = "";
            txtdireccion.Text = "";
            txtclave.Text = "";
            txtconfirmarclave.Text = "";
            cborol.SelectedIndex = 0;
            cboestado.SelectedIndex = 0;

            txtdocumento.Select();

        }

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 0) {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.sign_check_icon_34365.Width;
                var h = Properties.Resources.sign_check_icon_34365.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Width - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.sign_check_icon_34365, new Rectangle(x, y, w, h));
                e.Handled = true;

            }


        }

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvdata.Columns[e.ColumnIndex].Name == "btnseleccionar") {

                int indice = e.RowIndex;

                if (indice >= 0) {

                    txtindice.Text = indice.ToString();
                    txtid.Text = dgvdata.Rows[indice].Cells["Id"].Value.ToString();
                    txtdocumento.Text = dgvdata.Rows[indice].Cells["Documento"].Value.ToString();
                    txtnombre.Text = dgvdata.Rows[indice].Cells["Nombre"].Value.ToString();
                    txtapellido.Text = dgvdata.Rows[indice].Cells["Apellido"].Value.ToString();
                    txtcorreo.Text = dgvdata.Rows[indice].Cells["Correo"].Value.ToString();
                    txttelefono.Text = dgvdata.Rows[indice].Cells["Telefono"].Value.ToString();
                    txtdireccion.Text = dgvdata.Rows[indice].Cells["Direccion"].Value.ToString();
                    txtedad.Text = dgvdata.Rows[indice].Cells["Edad"].Value.ToString();
                    txtfechanacimiento.Text = dgvdata.Rows[indice].Cells["FechaNacimiento"].Value.ToString();
                    txtclave.Text = dgvdata.Rows[indice].Cells["Clave"].Value.ToString();
                    txtconfirmarclave.Text = dgvdata.Rows[indice].Cells["Clave"].Value.ToString();

                    foreach (OpcionCombo oc in cborol.Items) {

                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvdata.Rows[indice].Cells["IdRol"].Value)) {
                            int indice_combo = cborol.Items.IndexOf(oc);
                            cborol.SelectedIndex = indice_combo;
                            break;

                        }

                    }

                    foreach(OpcionCombo oc in cborol.Items) { //cboestado.Items  cambiar por rol

                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvdata.Rows[indice].Cells["IdRol"].Value)) //["EstadoValor"] cambiar por rol
                        {
                            int indice_combo = cborol.Items.IndexOf(oc);  // int indice_combo = cboestado.Items.IndexOf(oc); cambiar por rol
                            cborol.SelectedIndex = indice_combo;   // cboestado.SelectedIndex = indice_combo; cambiar por rol
                            break;

                        }

                    }


                }

            }


        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtid.Text) != 0) {
                if (MessageBox.Show("¿Desea eliminar el usuario?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {

                    string mensaje = string.Empty;
                    Usuario objusuario = new Usuario()
                    {
                        IdUsuario = Convert.ToInt32(txtid.Text)
                    };



                    bool respuesta = new CN_Usuario().Eliminar(objusuario, out mensaje);

                    if (respuesta)
                    {
                        dgvdata.Rows.RemoveAt(Convert.ToInt32(txtindice.Text));
                    }
                    else {
                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }

            }


        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cbobusqueda.SelectedItem).Valor.ToString();

            if (dgvdata.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvdata.Rows)
                {

                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtbusqueda.Text.Trim().ToUpper()))
                    {
                        row.Visible = true;
                    }
                    else
                        row.Visible = false;
                
                 }

            }
        }

        private void btnlimpiarbuscador_Click(object sender, EventArgs e)
        {
            txtbusqueda.Text = "";
            foreach (DataGridViewRow row in dgvdata.Rows)
            {

                row.Visible = true;

            }
        }

        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            txtdocumento.Clear();
            txtnombre.Clear();
            txtapellido.Clear();
            txtcorreo.Clear();
            txttelefono.Clear();
            txtdireccion.Clear();
            txtedad.Clear();
            txtfechanacimiento.Clear();
            txtdireccion.Clear();
            txtclave.Clear();
            txtconfirmarclave.Clear();
            cborol.SelectedIndex = 0;
            cboestado.SelectedIndex = 0;

        }

    

        private void txtdocumento_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verificar si el carácter no es un número ni un carácter de control
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Evitar que se escriba el carácter no válido
                MessageBox.Show("Por favor, ingrese solo números.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       





        private void txtcorreo_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir letras, números y caracteres especiales
            if (char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == '@' || e.KeyChar == '.' || e.KeyChar == '_' || e.KeyChar == '\b')
            {
                // Si se ingresó un carácter válido o se presionó Backspace, no hacemos nada
                return;
            }

            // Validar si el campo de correo contiene "@gmail.com" o "@outlook.es"
            string correo = txtcorreo.Text + e.KeyChar; // Agregar el carácter actual al texto existente
            if (correo.EndsWith("@gmail.com") || correo.EndsWith("@outlook.es"))
            {
                // Si el correo coincide con uno de los formatos permitidos, permitir la entrada
                return;
            }

            // Si no se cumple ninguna de las condiciones anteriores, evitar la entrada y mostrar un mensaje de error
            e.Handled = true;
            MessageBox.Show("Por favor, ingrese un correo válido con formato '@gmail.com' o '@outlook.es'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }




        private void txtfechanacimiento_TextChanged(object sender, EventArgs e)
        {
            string fechaNacimiento = txtfechanacimiento.Text;

            if (fechaNacimiento.Length == 10 && EsFechaValida(fechaNacimiento))
            {
                // La fecha de nacimiento es válida.
                // Puedes realizar acciones adicionales aquí si es necesario.
                DateTime fecha = DateTime.ParseExact(fechaNacimiento, "yyyy-MM-dd", CultureInfo.InvariantCulture);

      

                // Aquí puedes realizar otras acciones según tus necesidades
                MessageBox.Show($"La fecha de nacimiento es válida.");
            }
        }


        //Verifica que se respete el formato de una fecha al momento de ingresarlo
        static bool EsFechaValida(string fecha)
        {
            DateTime fechaNacimiento;
            // Intentamos analizar la cadena como una fecha en el formato específico.
            if (DateTime.TryParseExact(fecha, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaNacimiento))
            {
                return true; // La fecha es válida en el formato deseado.
            }
            else
            {
                return false; // La fecha no es válida o no está en el formato correcto.
            }
        }






        // Verifica que no se pueda ingresar numeros y solo sean letras y espacios
        private void txtapellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir letras y espacios en blanco
            if (char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == '\b')
            {
                // Si se ingresó una letra o espacio válido, no hacemos nada
                return;
            }

            e.Handled = true; // Evitar que se escriba el carácter no válido
            MessageBox.Show("Por favor, ingrese solo letras y espacios en el campo de apellido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Verifica que solo se ingresen letras y y espacios ademas de no quedar vacio
        private void txtapellido_TextChanged(object sender, EventArgs e)
        {
           
        }

        

        private void txtnombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir letras y espacios en blanco
            if (char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == '\b')
            {
                // Si se ingresó una letra o espacio válido, no hacemos nada
                return;
            }

            e.Handled = true; // Evitar que se escriba el carácter no válido
            MessageBox.Show("Por favor, ingrese solo letras y espacios en el campo de apellido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void txtnombre_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txttelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verificar si el carácter no es un número ni un carácter de control
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Evitar que se escriba el carácter no válido
                MessageBox.Show("Por favor, ingrese solo números.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtedad_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verificar si el carácter no es un número ni un carácter de control
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Evitar que se escriba el carácter no válido
                MessageBox.Show("Por favor, ingrese solo números.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





    }
}
