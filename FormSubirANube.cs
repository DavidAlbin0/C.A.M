﻿using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using OfficeOpenXml;
using System.Linq;
using System.Data.SqlClient;

namespace CustomerAdvancementManager_CAM_
{
    public partial class FormSubirANube : Form
    {
        private TextBox[] textBoxes;
        private string searchText = string.Empty;
        private int searchStartIndex = -1;
        private Label[] labels;
        private string connectionString = "Server=WSTSEC-280\\MSSQLSERVER01;Database=CFDB;User Id=userPrueba;Password=3.1416Xpi;";


        public FormSubirANube()
        {
            InitializeComponent();
            ConfigureDataGridView();
            FilterRows();


            // Inicializar el array de TextBox
            textBoxes = new TextBox[] { textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10, textBox11, textBox12, textBox13 };
            labels = new Label[] { label1, label2, label3, label4, label5, label6, label7, label8, label9, label10, label12, label13 };
            // Set the license context for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Adjust according to your license type


        }

        private void ConfigureDataGridView()
        {
            dataGridView1.Dock = DockStyle.Fill; // Hacer que el DataGridView llene el formulario
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView1.GridColor = Color.Black;

            // Estilo para la selección
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.LightBlue; // Color cuando la fila está seleccionada
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black; // Color del texto cuando la fila está seleccionada

            // Estilo para las filas alternas
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 255);
            dataGridView1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

            // Estilo para las filas normales
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;

            // Desactivar el redimensionamiento de columnas
            dataGridView1.AllowUserToResizeColumns = false;

            // Hacer que el DataGridView sea de solo lectura
            dataGridView1.ReadOnly = true;

            // Asegurar que al hacer clic en una celda se seleccione toda la fila
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false; // Opcional: evita la selección de múltiples filas

            // Evitar el ordenamiento de columnas
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // Configurar el estilo del encabezado
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 149, 237); // Color de fondo del encabezado (ej. Cornflower Blue)
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // Color del texto del encabezado
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single; // Estilo de borde del encabezado

            // Desactivar la interacción del encabezado
            dataGridView1.AllowUserToOrderColumns = false; // Desactivar la capacidad de ordenar columnas
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing; // Desactivar el redimensionamiento del encabezado
            dataGridView1.ColumnHeadersVisible = true; // Asegurarse de que el encabezado sea visible

            // Agregar el manejador de eventos para la selección de fila
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // Verificar que haya una fila seleccionada
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int columnCount = Math.Min(dataGridView1.Columns.Count, textBoxes.Length);

                // Hacer visibles solo los TextBox y Labels que se usen
                for (int i = 0; i < columnCount; i++)
                {
                    // Mostrar el TextBox y Label correspondiente
                    textBoxes[i].Visible = true;
                    if (i < labels.Length)
                    {
                        labels[i].Visible = true;
                        labels[i].Text = dataGridView1.Columns[i].HeaderText; // Opcional: Actualizar el texto del Label con el encabezado de la columna
                    }

                    // Asignar el valor del DataGridView al TextBox
                    if (selectedRow.Cells.Count > i && selectedRow.Cells[i].Value != null)
                    {
                        textBoxes[i].Text = selectedRow.Cells[i].Value.ToString();
                    }
                    else
                    {
                        textBoxes[i].Text = string.Empty;
                    }
                }

                // Ocultar los TextBox y Labels que no se usen
                for (int i = columnCount; i < textBoxes.Length; i++)
                {
                    textBoxes[i].Visible = false;
                    if (i < labels.Length)
                    {
                        labels[i].Visible = false;
                    }
                }
            }
        }


        private void InitializeLabels()
        {
            foreach (var label in labels)
            {
                label.Visible = false;
            }
        }

        private void InitializeTextBoxes()
        {
            foreach (var textBox in textBoxes)
            {
                textBox.Visible = false;
            }
        }

        private void UpdateLabels(int numberOfLabels)
        {
            // Agregar Labels adicionales si es necesario
            for (int i = labels.Length; i < numberOfLabels; i++)
            {
                Label newLabel = new Label();
                newLabel.Name = $"label{i}";
                newLabel.Location = new Point(10, 10 + (i * 30)); // Ajustar ubicación según sea necesario
                newLabel.Text = $"Column {i}"; // Ajustar texto según sea necesario
                this.Controls.Add(newLabel);
            }

            // Eliminar Labels adicionales si es necesario
            for (int i = numberOfLabels; i < labels.Length; i++)
            {
                Label labelToRemove = labels[i];
                this.Controls.Remove(labelToRemove);
                labelToRemove.Dispose();
            }
        }

        private void UpdateTextBoxes(int numberOfTextBoxes)
        {
            // Agregar TextBox adicionales si es necesario
            for (int i = textBoxes.Length; i < numberOfTextBoxes; i++)
            {
                TextBox newTextBox = new TextBox();
                newTextBox.Name = $"textBox{i}";
                newTextBox.Location = new Point(10, 10 + (i * 30)); // Ajustar ubicación según sea necesario
                this.Controls.Add(newTextBox);
            }

            // Eliminar TextBox adicionales si es necesario
            for (int i = numberOfTextBoxes; i < textBoxes.Length; i++)
            {
                TextBox textBoxToRemove = textBoxes[i];
                this.Controls.Remove(textBoxToRemove);
                textBoxToRemove.Dispose();
            }
        }

        private void DataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            // Aplicar color de fondo en la fila seleccionada
            if (dataGridView1.Rows[e.RowIndex].Selected)
            {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
            }
            else
            {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // Aplicar color de fondo en la fila seleccionada
            if (dataGridView1.Rows[e.RowIndex].Selected)
            {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
            }
            else
            {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Open file dialog to select Excel file
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    // Obtener el nombre del archivo
                    string fileName = System.IO.Path.GetFileName(filePath);

                    // Actualizar el contenido de label11
                    label11.Text = fileName;

                    // Load the selected Excel file into the DataGridView
                    LoadExcelFileIntoDataGridView(filePath);
                }
            }
        }


        private void FilterRows()
        {
            // Asegúrate de que haya al menos dos columnas en el DataGridView
            if (dataGridView1.Columns.Count > 1)
            {
                // Iterar hacia atrás para evitar problemas al eliminar filas durante la iteración
                for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
                {
                    DataGridViewRow row = dataGridView1.Rows[i];
                    string cellValue = row.Cells[1].Value?.ToString(); // Obtener el valor de la segunda columna (índice 1)

                    // Verificar si el valor de la celda contiene "Total" seguido de un número o "Total General"
                    if (cellValue != null && (cellValue.StartsWith("Total") || cellValue.Equals("Total General", StringComparison.OrdinalIgnoreCase)))
                    {
                        string numberPart = cellValue.Substring(5).Trim(); // Obtener la parte después de "Total"

                        // Verificar si es un número o si es exactamente "Total General"
                        if (int.TryParse(numberPart, out _) || cellValue.Equals("Total General", StringComparison.OrdinalIgnoreCase))
                        {
                            // Eliminar la fila
                            dataGridView1.Rows.RemoveAt(i);
                        }
                    }
                }
            }
        }


        private void LoadExcelFileIntoDataGridView(string filePath)
        {
            // Clear existing data
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            // Define labels array
            labels = new Label[] { label1, label2, label3, label4, label5, label6, label7, label8, label9, label10, label12, label13 };

            // Open the Excel package
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];  // Cargar la primera hoja del archivo
                int columnCount = worksheet.Dimension.Columns;
                int rowCount = worksheet.Dimension.Rows;

                // Add columns to DataGridView with renamed headers
                for (int col = 1; col <= columnCount; col++)
                {
                    string headerText = worksheet.Cells[1, col].Text;

                    // Cambiar los nombres de los encabezados según se necesite
                    switch (headerText)
                    {
                        case "AÑO":
                            headerText = "anio_Proy";
                            break;
                        case "No. PROY":
                            headerText = "id_Proy";
                            break;
                        case "OBRA":
                            headerText = "obra";
                            break;
                        case "AVANCE":
                            headerText = "avance";
                            break;
                        case "DIRECTO":
                            headerText = "directo";
                            break;
                        case "INDIRECTO":
                            headerText = "indirecto";
                            break;
                        case "TOTAL":
                            headerText = "total";
                            break;
                        case "C/GASTOS":
                            headerText = "c_Gastos";
                            break;
                        case "UTILIDAD-PERDIDA":
                            headerText = "utilidad_Perdida";
                            break;
                        case "CORD":
                            headerText = "cord";
                            break;
                        case "TIPO2":
                            headerText = "tipo2";
                            break;
                        case "Suma de AVANCE":
                            headerText = "total_Avance";
                            break;
                        case "Suma de DIRECTO":
                            headerText = "total_Directo";
                            break;
                        case "Suma de INDIRECTO":
                            headerText = "total_Indirecto";
                            break;
                        case "Suma de TOTAL":
                            headerText = "total_Suma";
                            break;
                        case "Suma de C/GASTOS":
                            headerText = "total_c_Gastos";
                            break;
                        case "Suma de UTILIDAD-PERDIDA":
                            headerText = "total_uti_perd";
                            break;
                        case "tipo2":
                            headerText = "estatus";
                            break;
                        case "Dias credito":
                            headerText = "dias_credito";
                            break;
                        default:
                            break;
                    }

                    // Add columns to DataGridView
                    dataGridView1.Columns.Add($"Column{col}", headerText);

                    // Update labels with header texts
                    if (col - 1 < labels.Length)
                    {
                        labels[col - 1].Text = headerText;
                    }
                }

                // Add rows to DataGridView
                for (int row = 2; row <= rowCount; row++)
                {
                    DataGridViewRow newRow = new DataGridViewRow();
                    newRow.CreateCells(dataGridView1);

                    for (int col = 1; col <= columnCount; col++)
                    {
                        newRow.Cells[col - 1].Value = worksheet.Cells[row, col].Text;
                    }

                    dataGridView1.Rows.Add(newRow);
                }

                // Apply filters
                FilterRows();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            searchText = textBox1.Text;
            searchStartIndex = -1; // Reiniciar el índice de búsqueda
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                searchStartIndex = -1; // Reiniciar el índice de búsqueda
                FindNextMatch();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                FindNextMatch();
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
                        dataGridView1.CurrentCell = cell;
                        dataGridView1.Rows[i].Selected = true;
                        dataGridView1.FirstDisplayedScrollingRowIndex = i; // Desplazar la fila a la vista
                        searchStartIndex = i; // Actualizar el índice de búsqueda
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
                        dataGridView1.CurrentCell = cell;
                        dataGridView1.Rows[i].Selected = true;
                        dataGridView1.FirstDisplayedScrollingRowIndex = i; // Desplazar la fila a la vista
                        searchStartIndex = i; // Actualizar el índice de búsqueda
                        return;
                    }
                }
            }

            MessageBox.Show("No se encontraron más coincidencias.");
        }


        private void button2_Click(object sender, EventArgs e)
        {
            // Filtrar las filas que contienen la palabra "Total"
            FilterRows();
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void FormSubirANube_Load(object sender, EventArgs e)
        {

        }


        private void SaveDataGridViewToExcel(DataGridView dataGridView, string filePath)
        {
            // Crear un nuevo paquete Excel
            using (var package = new ExcelPackage())
            {
                // Crear una hoja de trabajo
                var worksheet = package.Workbook.Worksheets.Add("Hoja1");

                // Añadir los encabezados de las columnas
                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = dataGridView.Columns[i].HeaderText;
                }

                // Añadir las filas del DataGridView
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = dataGridView.Rows[i].Cells[j].Value?.ToString();
                    }
                }

                // Guardar el archivo
                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
            }

            MessageBox.Show("Archivo guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void subirCostosAcum()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow) // Excluir la fila nueva (vacía)
                        {
                            string query = "INSERT INTO Costos_Acumulados4 (id_Proy, anio_Proy, obra, avance, directo, indirecto, total, c_Gastos, utilidad_Perdida, cord, estatus) " +
                                           "VALUES (@id_Proy, @anio_Proy, @obra, @avance, @directo, @indirecto, @total, @c_Gastos, @utilidad_Perdida, @cord, @estatus)";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                // Limpiar los parámetros anteriores antes de añadir nuevos
                                command.Parameters.Clear();

                                // Validar y convertir id_Proy (INT)
                                if (int.TryParse(row.Cells[0].Value?.ToString(), out int idProy))
                                {
                                    command.Parameters.AddWithValue("@id_Proy", idProy);
                                }
                                else
                                {
                                    MessageBox.Show($"El valor de 'id_Proy' no es un número entero válido en la fila: {row.Index}");
                                    continue; // Saltar esta fila y continuar con la siguiente
                                }

                                // Validar y convertir anio_Proy (INT)
                                if (int.TryParse(row.Cells[1].Value?.ToString(), out int anioProy))
                                {
                                    command.Parameters.AddWithValue("@anio_Proy", anioProy);
                                }
                                else
                                {
                                    MessageBox.Show($"El valor de 'anio_Proy' no es un número entero válido en la fila: {row.Index}");
                                    continue; // Saltar esta fila y continuar con la siguiente
                                }

                                // Validar y convertir valores numéricos (FLOAT)
                                if (decimal.TryParse(row.Cells[3].Value?.ToString(), out decimal avance) &&
                                    decimal.TryParse(row.Cells[4].Value?.ToString(), out decimal directo) &&
                                    decimal.TryParse(row.Cells[5].Value?.ToString(), out decimal indirecto) &&
                                    decimal.TryParse(row.Cells[6].Value?.ToString(), out decimal total) &&
                                    decimal.TryParse(row.Cells[7].Value?.ToString(), out decimal cGastos) &&
                                    decimal.TryParse(row.Cells[8].Value?.ToString(), out decimal utilidadPerdida)
                                    )
                                {
                                    // Asignar los parámetros al comando SQL
                                    command.Parameters.AddWithValue("@obra", row.Cells[2].Value ?? (object)DBNull.Value);
                                    command.Parameters.AddWithValue("@avance", avance);
                                    command.Parameters.AddWithValue("@directo", directo);
                                    command.Parameters.AddWithValue("@indirecto", indirecto);
                                    command.Parameters.AddWithValue("@total", total);
                                    command.Parameters.AddWithValue("@c_Gastos", cGastos);
                                    command.Parameters.AddWithValue("@utilidad_Perdida", utilidadPerdida);
                                    command.Parameters.AddWithValue("@cord", row.Cells[9].Value ?? (object)DBNull.Value);
                                    command.Parameters.AddWithValue("@estatus", row.Cells[10].Value ?? (object)DBNull.Value);



                                    command.ExecuteNonQuery();
                                }
                                else
                                {
                                    MessageBox.Show($"Uno o más valores numéricos no son válidos en la fila: {row.Index}");
                                    continue;
                                }
                            }
                        }
                    }

                    MessageBox.Show("Datos insertados correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al insertar datos: {ex.Message}");
            }
        }

        private void subirsumaCostosAcum()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow) // Excluir la fila nueva (vacía)
                        {
                            string query = "INSERT INTO Acumulados_Totales (id_Proy, total_Avance, total_Directo, total_Indirecto, total__Suma, total_c_Gastos, total_uti_perd) " +
                                           "VALUES (@id_Proy, @total_Avance, @total_Directo, @total_Indirecto, @total__Suma, @total_c_Gastos, @total_uti_perd)";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                // Limpiar los parámetros anteriores antes de añadir nuevos
                                command.Parameters.Clear();

                                // Validar y convertir id_Proy (INT)
                                if (int.TryParse(row.Cells[0].Value?.ToString(), out int idProy))
                                {
                                    command.Parameters.AddWithValue("@id_Proy", idProy);
                                }

                                // Validar y convertir valores numéricos (FLOAT)
                                if (decimal.TryParse(row.Cells[2].Value?.ToString(), out decimal avance) &&
                                    decimal.TryParse(row.Cells[3].Value?.ToString(), out decimal directo) &&
                                    decimal.TryParse(row.Cells[4].Value?.ToString(), out decimal indirecto) &&
                                    decimal.TryParse(row.Cells[5].Value?.ToString(), out decimal total) &&
                                    decimal.TryParse(row.Cells[6].Value?.ToString(), out decimal cGastos) &&
                                    decimal.TryParse(row.Cells[7].Value?.ToString(), out decimal utilidadPerdida)
                                    )
                                {
                                    // Asignar los parámetros al comando SQL
                                    command.Parameters.AddWithValue("@total_Avance", avance);
                                    command.Parameters.AddWithValue("@total_Directo", directo);
                                    command.Parameters.AddWithValue("@total_Indirecto", indirecto);
                                    command.Parameters.AddWithValue("@total__Suma", total);
                                    command.Parameters.AddWithValue("@total_c_Gastos", cGastos);
                                    command.Parameters.AddWithValue("@total_uti_perd", utilidadPerdida);

                                    command.ExecuteNonQuery();
                                }
                                else
                                {
                                    MessageBox.Show($"Uno o más valores numéricos no son válidos en la fila: {row.Index}");
                                    continue;
                                }
                            }
                        }
                    }

                    MessageBox.Show("Datos insertados correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al insertar datos: {ex.Message}");
            }
        }

        private void subirContratos()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow && !EsFilaClaveVacia(row)) // Excluir la fila nueva (vacía) y validar campos clave
                        {
                            string query = "INSERT INTO B_Contratos (id_Contrato, id_Empresa, id_Proy, tipo_Contrato, nombre_Contrato, observaciones, fecha_Factura, fecha_Deposito, RBO_FAC_NUM, total_Contrato, estimacion, fecha_de_Corte, fecha_de_Pago) " +
                                           "VALUES (@id_Contrato, @id_Empresa, @id_Proy, @tipo_Contrato, @nombre_Contrato, @observaciones, @fecha_Factura, @fecha_Deposito, @RBO_FAC_NUM, @total_Contrato, @estimacion, @fecha_de_Corte, @fecha_de_Pago)";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.Clear();

                                // Validar y asignar valores de las columnas clave (no se permite DBNull.Value)
                                command.Parameters.AddWithValue("@id_Contrato", int.Parse(row.Cells[0].Value.ToString())); // id_Contrato es PK, debe tener valor
                                command.Parameters.AddWithValue("@id_Empresa", int.Parse(row.Cells[1].Value.ToString()));  // id_Empresa es FK, debe tener valor
                                command.Parameters.AddWithValue("@id_Proy", int.Parse(row.Cells[2].Value.ToString()));     // id_Proy es FK, debe tener valor

                                // Validar y asignar valores opcionales (pueden ser DBNull.Value si están vacíos)
                                command.Parameters.AddWithValue("@tipo_Contrato", string.IsNullOrEmpty(row.Cells[3].Value?.ToString()) ? (object)DBNull.Value : row.Cells[3].Value?.ToString());
                                command.Parameters.AddWithValue("@nombre_Contrato", string.IsNullOrEmpty(row.Cells[4].Value?.ToString()) ? (object)DBNull.Value : row.Cells[4].Value?.ToString());
                                command.Parameters.AddWithValue("@observaciones", string.IsNullOrEmpty(row.Cells[5].Value?.ToString()) ? (object)DBNull.Value : row.Cells[5].Value?.ToString());
                                command.Parameters.AddWithValue("@fecha_Factura", DateTime.TryParse(row.Cells[6].Value?.ToString(), out DateTime fechaFactura) ? (object)fechaFactura : DBNull.Value);
                                command.Parameters.AddWithValue("@fecha_Deposito", DateTime.TryParse(row.Cells[7].Value?.ToString(), out DateTime fechaDeposito) ? (object)fechaDeposito : DBNull.Value);
                                command.Parameters.AddWithValue("@RBO_FAC_NUM", int.TryParse(row.Cells[8].Value?.ToString(), out int rboFacNum) ? (object)rboFacNum : DBNull.Value);
                                command.Parameters.AddWithValue("@total_Contrato", decimal.TryParse(row.Cells[9].Value?.ToString(), out decimal totalContrato) ? (object)totalContrato : DBNull.Value);
                                command.Parameters.AddWithValue("@estimacion", string.IsNullOrEmpty(row.Cells[10].Value?.ToString()) ? (object)DBNull.Value : row.Cells[10].Value?.ToString());
                                command.Parameters.AddWithValue("@fecha_de_Corte", DateTime.TryParse(row.Cells[11].Value?.ToString(), out DateTime fechaCorte) ? (object)fechaCorte : DBNull.Value);
                                command.Parameters.AddWithValue("@fecha_de_Pago", DateTime.TryParse(row.Cells[12].Value?.ToString(), out DateTime fechaPago) ? (object)fechaPago : DBNull.Value);

                                // Ejecutar la consulta SQL
                                command.ExecuteNonQuery();
                            }
                        }
                    }

                    MessageBox.Show("Datos insertados correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al insertar datos: {ex.Message}");
            }
        }

        // Método auxiliar para verificar si las columnas clave (id_Contrato, id_Empresa, id_Proy) están vacías o contienen solo espacios en blanco
        private bool EsFilaClaveVacia(DataGridViewRow row)
        {
            // Verificar si alguna de las celdas de las columnas clave está vacía o contiene solo espacios en blanco
            if (string.IsNullOrWhiteSpace(row.Cells[0].Value?.ToString()) || // id_Contrato (PK)
                string.IsNullOrWhiteSpace(row.Cells[1].Value?.ToString()) || // id_Empresa (FK)
                string.IsNullOrWhiteSpace(row.Cells[2].Value?.ToString()))   // id_Proy (FK)
            {
                return true; // Si alguna de las columnas clave está vacía o tiene espacios en blanco
            }
            return false; // Si todas las columnas clave tienen valores válidos
        }



        private void subirProyectos()
        {
            int filasProcesadas = 0; // Contador para las filas procesadas correctamente

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow) // Excluir la fila nueva (vacía)
                        {
                            // Asegúrate de declarar y asignar el valor a idProy aquí
                            int idProy;
                            if (!int.TryParse(row.Cells[0].Value?.ToString(), out idProy))
                            {
                                // Mostrar solo un mensaje si el ID no es válido
                                MessageBox.Show($"ID de Proyecto no válido en la fila: {row.Index}");
                                continue; // Si el ID no es válido, continuar con la siguiente fila
                            }

                            // Verificar si existe el id_Proy en la tabla
                            string checkQuery = "SELECT COUNT(1) FROM Proyectos WHERE id_Proy = @id_Proy";
                            using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                            {
                                checkCommand.Parameters.Clear();
                                checkCommand.Parameters.AddWithValue("@id_Proy", idProy);

                                int exists = (int)checkCommand.ExecuteScalar(); // Devuelve 1 si existe, 0 si no

                                if (exists > 0)
                                {
                                    // Realizar UPDATE
                                    string updateQuery = "UPDATE Proyectos SET " +
                                                         "anio_Inicio = @anio_Inicio, " +
                                                         "estado = @estado, " +
                                                         "nombre_obra = @nombre_obra, " +
                                                         "coordinador = @coordinador, " +
                                                         "dias_credito = @dias_credito " +
                                                         "WHERE id_Proy = @id_Proy";

                                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                                    {
                                        updateCommand.Parameters.Clear();
                                        updateCommand.Parameters.AddWithValue("@id_Proy", idProy);

                                        // Validar y convertir anio_Inicio (DATE)
                                        if (DateTime.TryParse(row.Cells[1].Value?.ToString(), out DateTime anioInicio))
                                        {
                                            updateCommand.Parameters.AddWithValue("@anio_Inicio", anioInicio);
                                        }
                                        else
                                        {
                                            updateCommand.Parameters.AddWithValue("@anio_Inicio", DBNull.Value); // Permitir valor nulo
                                        }

                                        // Asignar estado (VARCHAR), permitir valor nulo
                                        string estado = row.Cells[2].Value?.ToString();
                                        updateCommand.Parameters.AddWithValue("@estado", string.IsNullOrEmpty(estado) ? (object)DBNull.Value : estado);

                                        // Asignar nombre_obra (VARCHAR) - No permitir valor nulo
                                        string nombreObra = row.Cells[3].Value?.ToString();
                                        if (string.IsNullOrEmpty(nombreObra))
                                        {
                                            MessageBox.Show($"Nombre de obra no válido en la fila: {row.Index}");
                                            continue; // Continuar sin procesar esta fila
                                        }
                                        updateCommand.Parameters.AddWithValue("@nombre_obra", nombreObra);

                                        // Asignar coordinador (VARCHAR), permitir valor nulo
                                        string coordinador = row.Cells[4].Value?.ToString();
                                        updateCommand.Parameters.AddWithValue("@coordinador", string.IsNullOrEmpty(coordinador) ? (object)DBNull.Value : coordinador);

                                        // Validar y convertir dias_credito (INT)
                                        if (int.TryParse(row.Cells[5].Value?.ToString(), out int diasCredito))
                                        {
                                            updateCommand.Parameters.AddWithValue("@dias_credito", diasCredito);
                                        }
                                        else
                                        {
                                            updateCommand.Parameters.AddWithValue("@dias_credito", DBNull.Value); // Permitir valor nulo
                                        }

                                        // Ejecutar la consulta
                                        updateCommand.ExecuteNonQuery();
                                        filasProcesadas++; // Incrementar el contador de filas procesadas
                                    }
                                }
                                else
                                {
                                    // Realizar INSERT
                                    string insertQuery = "INSERT INTO Proyectos (id_Proy, anio_Inicio, estado, nombre_obra, coordinador, dias_credito) " +
                                                         "VALUES (@id_Proy, @anio_Inicio, @estado, @nombre_obra, @coordinador, @dias_credito)";

                                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                                    {
                                        insertCommand.Parameters.Clear();
                                        insertCommand.Parameters.AddWithValue("@id_Proy", idProy);

                                        // Validar y convertir anio_Inicio (DATE)
                                        if (DateTime.TryParse(row.Cells[1].Value?.ToString(), out DateTime anioInicio))
                                        {
                                            insertCommand.Parameters.AddWithValue("@anio_Inicio", anioInicio);
                                        }
                                        else
                                        {
                                            insertCommand.Parameters.AddWithValue("@anio_Inicio", DBNull.Value); // Permitir valor nulo
                                        }

                                        // Asignar estado (VARCHAR), permitir valor nulo
                                        string estado = row.Cells[2].Value?.ToString();
                                        insertCommand.Parameters.AddWithValue("@estado", string.IsNullOrEmpty(estado) ? (object)DBNull.Value : estado);

                                        // Asignar nombre_obra (VARCHAR) - No permitir valor nulo
                                        string nombreObra = row.Cells[3].Value?.ToString();
                                        if (string.IsNullOrEmpty(nombreObra))
                                        {
                                            MessageBox.Show($"Nombre de obra no válido en la fila: {row.Index}");
                                            continue; // Continuar sin procesar esta fila
                                        }
                                        insertCommand.Parameters.AddWithValue("@nombre_obra", nombreObra);

                                        // Asignar coordinador (VARCHAR), permitir valor nulo
                                        string coordinador = row.Cells[4].Value?.ToString();
                                        insertCommand.Parameters.AddWithValue("@coordinador", string.IsNullOrEmpty(coordinador) ? (object)DBNull.Value : coordinador);

                                        // Validar y convertir dias_credito (INT)
                                        if (int.TryParse(row.Cells[5].Value?.ToString(), out int diasCredito))
                                        {
                                            insertCommand.Parameters.AddWithValue("@dias_credito", diasCredito);
                                        }
                                        else
                                        {
                                            insertCommand.Parameters.AddWithValue("@dias_credito", DBNull.Value); // Permitir valor nulo
                                        }

                                        // Ejecutar la consulta
                                        insertCommand.ExecuteNonQuery();
                                        filasProcesadas++; // Incrementar el contador de filas procesadas
                                    }
                                }
                            }
                        }
                    }

                    // Mostrar el mensaje de éxito una vez al final del proceso
                    if (filasProcesadas > 0)
                    {
                        MessageBox.Show($"{filasProcesadas} datos procesados correctamente.");
                    }
                    else
                    {
                        MessageBox.Show("No se procesaron datos.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar datos: {ex.Message}");
            }
        }


        private decimal ConvertirADecimal(string valor)
        {
            if (string.IsNullOrEmpty(valor))
            {
                throw new ArgumentException("El valor no puede ser nulo o vacío.");
            }

            // Eliminar el separador de miles (usualmente punto) y reemplazar la coma por punto
            valor = valor.Replace(".", "").Replace(",", ".");

            // Intentar convertir la cadena a decimal
            if (decimal.TryParse(valor, out decimal resultado))
            {
                return resultado;
            }
            else
            {
                throw new FormatException($"El valor '{valor}' no tiene un formato válido.");
            }
        }

        private void SubirAvances()
        {
            int filasProcesadas = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow) // Excluir la fila nueva
                        {
                            // Verificar si id_Avance es nulo o vacío
                            if (string.IsNullOrWhiteSpace(row.Cells[0].Value?.ToString()))
                            {
                                MessageBox.Show($"El valor de id_Avance es nulo o vacío en la fila: {row.Index}. Se omitirá esta fila.");
                                continue; // Saltar a la siguiente fila
                            }

                            // Asegurarse de que el id de Proyecto es un entero válido
                            if (int.TryParse(row.Cells[1].Value?.ToString(), out int idProy)) // Cambiado a index 1 para id_Proy
                            {
                                try
                                {
                                    // Conversión de valores a decimal utilizando la nueva función
                                    if (string.IsNullOrWhiteSpace(row.Cells[2].Value?.ToString()))
                                    {
                                        MessageBox.Show($"El valor de avance_total es nulo o vacío en la fila: {row.Index}. Se omitirá esta fila.");
                                        continue; // Saltar a la siguiente fila
                                    }
                                    decimal avanceTotal = ConvertirADecimal(row.Cells[2].Value?.ToString()); // Cambiado a index 2

                                    if (string.IsNullOrWhiteSpace(row.Cells[3].Value?.ToString()))
                                    {
                                        MessageBox.Show($"El valor de avance_indirecto es nulo o vacío en la fila: {row.Index}. Se omitirá esta fila.");
                                        continue; // Saltar a la siguiente fila
                                    }
                                    decimal avanceIndirecto = ConvertirADecimal(row.Cells[3].Value?.ToString()); // Cambiado a index 3

                                    if (string.IsNullOrWhiteSpace(row.Cells[4].Value?.ToString()))
                                    {
                                        MessageBox.Show($"El valor de porcentaje_avance es nulo o vacío en la fila: {row.Index}. Se omitirá esta fila.");
                                        continue; // Saltar a la siguiente fila
                                    }
                                    decimal porcentajeAvance = ConvertirADecimal(row.Cells[4].Value?.ToString()); // Cambiado a index 4

                                    // Validar fecha_avance como VARCHAR
                                    string fechaAvance = row.Cells[5].Value?.ToString(); // Cambiado a index 5
                                    if (string.IsNullOrWhiteSpace(fechaAvance))
                                    {
                                        MessageBox.Show($"El valor de fecha_avance es nulo o vacío en la fila: {row.Index}. Se omitirá esta fila.");
                                        continue; // Saltar a la siguiente fila
                                    }

                                    // Validar límites
                                    if (avanceTotal >= -99999999999999.9999m && avanceTotal <= 99999999999999.9999m &&
                                        avanceIndirecto >= -99999999999999.9999m && avanceIndirecto <= 99999999999999.9999m &&
                                        porcentajeAvance >= 0 && porcentajeAvance <= 100)
                                    {
                                        // Aquí va la lógica para insertar en la base de datos
                                        string query = "INSERT INTO Avances (id_Proy, avance_total, avance_indirecto, porcentaje_avance, fecha_avance) " +
                                                       "VALUES (@id_Proy, @avance_total, @avance_indirecto, @porcentaje_avance, @fecha_avance)";

                                        using (SqlCommand command = new SqlCommand(query, connection))
                                        {
                                            command.Parameters.AddWithValue("@id_Proy", idProy);
                                            command.Parameters.AddWithValue("@avance_total", avanceTotal);
                                            command.Parameters.AddWithValue("@avance_indirecto", avanceIndirecto);
                                            command.Parameters.AddWithValue("@porcentaje_avance", porcentajeAvance);
                                            command.Parameters.AddWithValue("@fecha_avance", fechaAvance); // Almacenar como VARCHAR

                                            command.ExecuteNonQuery();
                                        }

                                        filasProcesadas++;
                                    }
                                    else
                                    {
                                        MessageBox.Show($"Valores fuera de rango en la fila: {row.Index}");
                                    }
                                }
                                catch (FormatException ex)
                                {
                                    MessageBox.Show($"Error de formato en la fila {row.Index}: {ex.Message}");
                                }
                            }
                            else
                            {
                                MessageBox.Show($"ID de Proyecto no válido en la fila: {row.Index}");
                            }
                        }
                    }

                    MessageBox.Show($"{filasProcesadas} filas procesadas correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar datos: {ex.Message}");
            }
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            // Abrir un diálogo de guardado para que el usuario elija la ubicación del archivo
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Guardar como Excel";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveDataGridViewToExcel(dataGridView1, saveFileDialog.FileName);
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            SubirAvances();
        }

        private void FormSubirANube_Load_1(object sender, EventArgs e)
        {

        }
    }
}
