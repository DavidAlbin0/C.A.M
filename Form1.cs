using System;
using System.Windows.Forms;
using CustomerAdvancementManager_CAM_;

namespace CustomerAdvancementManager_CAM_
{
    public partial class Form1 : Form
    {
        private bool isMenuCollapsed = false; // Estado del menú
        private ConexionDB conexionDB;

        public Form1()
        {
            InitializeComponent();
            conexionDB = new ConexionDB(); // Asegúrate de que esto se inicialice aquí


            // Configura el formulario principal como contenedor MDI
            this.IsMdiContainer = true;
            // Evento asociado al Click de los botones del menú
            clientesToolStripMenuItem.Click += new EventHandler(ClientesToolStripMenuItem_Click);
            pendientesToolStripMenuItem.Click += new EventHandler(PendientesToolStripMenuItem_Click);
            avancesToolStripMenuItem.Click += new EventHandler(AvancesToolStripMenuItem_Click);
            subirANubeToolStripMenuItem.Click += new EventHandler(SubirABaseToolStripMenuItem_Click);
            //dashBoardToolStripMenuItem.Click += new EventHandler(FormDashboardToolStringMenuItem_Click);

            this.Resize += new EventHandler(Form1_Resize);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Inicialmente el panel que contiene los botones está visible
            panel1.Visible = true;

            // Prueba la conexión a la base de datos al cargar el formulario
            TestDatabaseConnection();
        }
        private void TestDatabaseConnection()
        {
            try
            {
                conexionDB.TestConnection();
                MessageBox.Show("Conexión a la base de datos exitosa.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar a la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Podrías manejar aquí algún cierre de la aplicación o navegación alternativa en caso de fallo
            }
        }


        // Evento Click para el botón del menú (icono con tres líneas)
        private void btnMenu_Click(object sender, EventArgs e)
        {
            ToggleMenu(); // Alterna el estado del menú
        }

        // Método para alternar el estado del panel del menú
        private void ToggleMenu()
        {
            if (isMenuCollapsed)
            {
                // Si el menú está colapsado, expandirlo
                panel1.Visible = true;
                isMenuCollapsed = false;
            }
            else
            {
                // Si el menú está expandido, colapsarlo
                panel1.Visible = false;
                isMenuCollapsed = true;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Verifica el estado actual del panel y el menú
            bool isPanelCollapsed = panel1.Width <= 100; // El panel está colapsado si su ancho es menor o igual a 100

            if (isPanelCollapsed)
            {
                // Si el panel está colapsado, restaurar su tamaño
                panel1.Width = 260; // Tamaño original del panel
                panel1.Dock = DockStyle.Left; // Asegúrate de que el panel siga estando acoplado al lado izquierdo
            }
            else
            {
                // Si el panel no está colapsado, reducir su tamaño
                panel1.Width = 70; // Tamaño reducido del panel
                panel1.Dock = DockStyle.Left; // Asegúrate de que el panel siga estando acoplado al lado izquierdo
            }

            // Forzar el redibujado del formulario para aplicar los cambios de diseño
            this.PerformLayout();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Ajustar el panel al tamaño actual del formulario
            panel1.Height = this.ClientSize.Height;
        }

        private void subirANubeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Implementa la lógica para "Subir a la Nube" aquí
        }

        private void OpenForm(Form form, FormWindowState windowState = FormWindowState.Normal)
        {
            // Cierra cualquier formulario hijo que esté actualmente abierto
            foreach (Form openForm in this.MdiChildren)
            {
                openForm.Close();
            }

            // Configura el formulario para que se muestre sobre el formulario principal
            form.MdiParent = this;
            form.StartPosition = FormStartPosition.CenterParent;
            form.WindowState = windowState; // Establecer el estado de la ventana (maximizado por defecto)
            form.Show();
        }

        private void ClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new FormClientes(), FormWindowState.Maximized);
        }

        private void FormDashBoardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new FormDashBoard(), FormWindowState.Maximized);
        }

        private void PendientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new FormPendientes(), FormWindowState.Maximized);
        }

        private void AvancesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new FormAvances(), FormWindowState.Maximized);
        }

        private void SubirABaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new FormSubirANube(), FormWindowState.Maximized);
        }

      /*  private void FormDashboardToolStringMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new FormDashboard(), FormWindowState.Maximized);
        }*/
    }
    }
