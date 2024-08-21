using System.Data.SqlClient;
using System;
using System.Windows.Forms; // Asegúrate de incluir esta referencia


public class ConexionDB
{
    private string connectionString;

    public ConexionDB()
    {
        connectionString = "Server=WSTSEC-280\\MSSQLSERVER01;Database=Prueba;User Id=userPrueba;Password=3.1416Xpiuu;";
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }

    public void TestConnection()
    {
        using (SqlConnection connection = GetConnection())
        {
            try
            {
                connection.Open();
                // MessageBox.Show("Conexión exitosa.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                //  MessageBox.Show("Error al conectar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

