using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient; // Asegúrate de incluir esta referencia
using System.Linq; // Asegúrate de incluir esta referencia para usar LINQ
using System.Windows.Forms;

namespace CustomerAdvancementManager_CAM_
{
    public partial class FormDashBoard : Form
    {
        private string connectionString = "Server=WSTSEC-280\\MSSQLSERVER01;Database=CFDB;User Id=userPrueba;Password=3.1416Xpi;";
        private List<decimal> resultadosConsultas = new List<decimal>(); // Lista para almacenar resultados de consultas
        private string searchText = ""; // Almacena el texto de búsqueda
        private int searchStartIndex = -1; // Almacena el índice de búsqueda
        public FormDashBoard()
        {
            InitializeComponent();

            comboBox1.Items.Add("--Seleccione una opción--");
            comboBox1.Items.Add("Proyectos");
            comboBox1.Items.Add("Avances");
            comboBox1.Items.Add("Contratos");
            comboBox1.Items.Add("Facturas");

            // Opcional: Seleccionar el primer elemento por defecto
            comboBox1.SelectedIndex = 0;
            dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);
            chart2.Click += new EventHandler(chart2_Click);
            textBox9.TextChanged += new EventHandler(textBox9_TextChanged);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Verifica que se haya seleccionado una fila válida
            {
                var idProyValue = dataGridView1.Rows[e.RowIndex].Cells["id_Proy"].Value;
                var nomProy = dataGridView1.Rows[e.RowIndex].Cells["nombre_obra"].Value;
                var diasCredito = dataGridView1.Rows[e.RowIndex].Cells["dias_credito"].Value;
                var estadoActual = dataGridView1.Rows[e.RowIndex].Cells["estado"].Value;
                var coordinadorProy = dataGridView1.Rows[e.RowIndex].Cells["coordinador"].Value;


                if (idProyValue != null)
                {
                    textBox1.Text = idProyValue.ToString();
                    textBox2.Text = nomProy.ToString();
                    textBox3.Text = diasCredito.ToString();
                    textBox5.Text = coordinadorProy.ToString();
                    textBox6.Text = estadoActual.ToString();

                    // Limpiar resultados previos y el gráfico
                    resultadosConsultas.Clear();
                    chart1.Series.Clear();
                    chart1.ChartAreas.Clear();
                    chart2.Series.Clear();
                    chart2.ChartAreas.Clear(); // Limpiar chart2

                    Querys(e.RowIndex); // Ejecuta la consulta y llena resultadosConsultas
                    ActualizarGrafico(); // Actualizar gráfico de columnas
                    ActualizarGraficoDona(); // Actualizar gráfico de dona
                }
                else
                {
                    MessageBox.Show("El valor de id_Proy es nulo.");
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = comboBox1.SelectedItem.ToString();
            switch (selectedValue)
            {
                case "--Seleccione una opción--":
                    dataGridView1.DataSource = null;
                    break;
                case "Proyectos":
                    CargarDatos("SELECT * FROM Proyectos");
                    break;
                case "Avances":
                    CargarDatos("SELECT * FROM Avances");
                    break;
                case "Contratos":
                    CargarDatos("SELECT * FROM B_Contratos");
                    break;
                case "Facturas":
                    CargarDatos("SELECT * FROM Facturas");
                    break;
            }
        }

        private void CargarDatos(string query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();

                    connection.Open();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}");
            }
        }

        private void Querys(int rowIndex) // Asegúrate de pasar el índice de fila como parámetro
        {
            if (rowIndex >= 0 && rowIndex < dataGridView1.Rows.Count)
            {
                var idProyValue = dataGridView1.Rows[rowIndex].Cells["id_Proy"].Value;

                if (idProyValue == null)
                {
                    MessageBox.Show("No hay datos");
                }
                else
                {
                    string idProy = textBox1.Text; // Obtén el valor del TextBox1

                    if (!string.IsNullOrEmpty(idProy)) // Verifica si TextBox1 contiene un ID
                    {
                        string queryContrato = $@"
                    SELECT 
                        total_Contrato 
                    FROM B_Contratos 
                    WHERE id_Proy = '{idProy}'";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            SqlCommand commandContrato = new SqlCommand(queryContrato, connection);
                            connection.Open();

                            using (SqlDataReader readerContrato = commandContrato.ExecuteReader())
                            {
                                while (readerContrato.Read())
                                {
                                    if (!readerContrato.IsDBNull(0)) // Verificar si el valor no es nulo
                                    {
                                        decimal totalContrato = readerContrato.GetDecimal(0);
                                        resultadosConsultas.Add(totalContrato); // Almacena el valor en la lista

                                        // Lógica para calcular la suma y la suma positiva
                                        textBox7.Text = resultadosConsultas.Sum().ToString("N4"); // Suma total en textBox7
                                        textBox8.Text = resultadosConsultas.Where(x => x > 0).Sum().ToString("N4"); // Suma de positivos en textBox8
                                    }
                                }
                            }
                        }

                        // Nueva query para buscar en la tabla Avances
                        string queryAvances = $@"
                    SELECT 
                        avance_total, porcentaje_avance 
                    FROM Avances 
                    WHERE id_Proy = '{idProy}'";

                        using (SqlConnection connectionAvances = new SqlConnection(connectionString))
                        {
                            SqlCommand commandAvances = new SqlCommand(queryAvances, connectionAvances);
                            connectionAvances.Open();

                            using (SqlDataReader readerAvances = commandAvances.ExecuteReader())
                            {
                                if (readerAvances.Read()) // Si se encuentra el ID en Avances
                                {
                                    if (!readerAvances.IsDBNull(0)) // Verifica si avance_total no es nulo
                                    {
                                        decimal avanceTotal = readerAvances.GetDecimal(0);
                                        textBox9.Text = avanceTotal.ToString("N4"); // Mostrar avance_total en textBox9
                                    }

                                    if (!readerAvances.IsDBNull(1)) // Verifica si porcentaje_avance no es nulo
                                    {
                                        decimal porcentajeAvance = readerAvances.GetDecimal(1);
                                        textBox10.Text = porcentajeAvance.ToString("N2"); // Mostrar porcentaje_avance en textBox10
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("No se encontraron avances para este ID de proyecto.");
                                }
                            }
                        }
                    }
                }
            }
        }


        private void FormDashBoard_Load(object sender, EventArgs e)
        {
            // Puedes realizar configuraciones iniciales si es necesario
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Lógica para manejar cambios en textBox1 si es necesario
        }

        private void ActualizarGrafico()
        {
            // Limpiar todas las series existentes
            chart1.Series.Clear();

            // Limpiar todas las áreas del gráfico
            chart1.ChartAreas.Clear();
            chart1.ChartAreas.Add(new System.Windows.Forms.DataVisualization.Charting.ChartArea("ChartArea1"));

            // Crear una nueva serie para los datos actuales
            var serie = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Resultados",
                Color = System.Drawing.Color.Blue,
                IsValueShownAsLabel = true, // Muestra el valor en la columna
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column // Cambiado a columna
            };

            // Asegúrate de que hay datos en resultadosConsultas antes de agregar al gráfico
            if (resultadosConsultas.Count > 0)
            {
                // Agregar los resultados a la serie
                for (int i = 0; i < resultadosConsultas.Count; i++)
                {
                    // Cambiar el texto en el eje X para que se vea vertical
                    serie.Points.AddXY($"Contrato {i + 1}", resultadosConsultas[i]);
                }

                // Agregar la serie al gráfico
                chart1.Series.Add(serie);

                // Configurar los ejes
                chart1.ChartAreas[0].AxisX.Title = "Contrato";
                chart1.ChartAreas[0].AxisY.Title = "Valores";
                chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -90; // Rotar etiquetas del eje X para mejor legibilidad
            }
            else
            {
                MessageBox.Show("No hay datos para mostrar en el gráfico.");
            }
        }

        private void ActualizarGraficoDona()
        {
            // Limpiar todas las series existentes
            chart2.Series.Clear();

            // Limpiar todas las áreas del gráfico
            chart2.ChartAreas.Clear();
            chart2.ChartAreas.Add(new System.Windows.Forms.DataVisualization.Charting.ChartArea("ChartArea1"));

            // Crear una nueva serie para el gráfico de dona
            var serieDona = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "ResultadosDona",
                Color = System.Drawing.Color.Green,
                IsValueShownAsLabel = true, // Muestra el valor en la dona
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut // Cambiado a dona
            };

            // Asegúrate de que hay datos en resultadosConsultas antes de agregar al gráfico
            if (resultadosConsultas.Count > 0)
            {
                // Agregar los resultados a la serie de dona
                for (int i = 0; i < resultadosConsultas.Count; i++)
                {
                    serieDona.Points.AddXY($"Contrato {i + 1}", resultadosConsultas[i]);
                }

                // Agregar la serie al gráfico de dona
                chart2.Series.Add(serieDona);

                // Configurar el título del gráfico
                chart2.Titles.Clear();
                chart2.Titles.Add("Resultados en Dona");
            }
  
        }

        private void chart2_Click(object sender, EventArgs e)
        {
            // Lógica al hacer clic en el gráfico de dona, si es necesario
            MessageBox.Show("Hola, buenas tardes :)");
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            searchText = textBox9.Text; // Almacena el texto ingresado en el TextBox
            searchStartIndex = -1; // Reiniciar el índice de búsqueda
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                searchStartIndex = -1; // Reiniciar el índice de búsqueda
                FindNextMatch(); // Llama a la función para buscar
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                FindNextMatch(); // Llama a la función para buscar la siguiente coincidencia
            }
        }

        private void FindNextMatch()
        {
            for (int i = searchStartIndex + 1; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // Selecciona la celda encontrada
                        dataGridView1.CurrentCell = cell;
                        dataGridView1.Rows[i].Selected = true;
                        dataGridView1.FirstDisplayedScrollingRowIndex = i; // Desplaza la fila a la vista
                        searchStartIndex = i; // Actualiza el índice de búsqueda
                        return;
                    }
                }
            }

            // Si no se encuentra ninguna coincidencia después de la fila actual, empezar desde el principio
            for (int i = 0; i <= searchStartIndex; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // Selecciona la celda encontrada
                        dataGridView1.CurrentCell = cell;
                        dataGridView1.Rows[i].Selected = true;
                        dataGridView1.FirstDisplayedScrollingRowIndex = i; // Desplaza la fila a la vista
                        searchStartIndex = i; // Actualiza el índice de búsqueda
                        return;
                    }
                }
            }

            MessageBox.Show("No se encontraron más coincidencias."); // Mensaje si no hay coincidencias
        }


    }
}
