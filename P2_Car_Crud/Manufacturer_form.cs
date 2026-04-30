using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace P2_Car_Crud
{
    public partial class Manufacturer_form : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        public Manufacturer_form()
        {
            InitializeComponent();
        }
        private void Manufacturer_form_Load(object sender, EventArgs e)
        {
            LoadManufacturers();
        }
        void LoadManufacturers()
        {
            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                con.Open();

                DataTable dt = new DataTable();

                using (var cmd = new SqliteCommand("SELECT * FROM Manufacturer", con))
                using (var reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }

                dataGridView1.DataSource = dt;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text == "" || txtDescription.Text == "")
                {
                    MessageBox.Show("Please fill all fields");
                    return;
                }
                using (SqliteConnection con = new SqliteConnection(connectionString))
                {
                    con.Open();

                    string query = @"INSERT INTO Manufacturer
                                (Name, Description)
                                VALUES (@name, @Description)";

                    SqliteCommand cmd = new SqliteCommand(query, con);

                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Manufacturer added successfully");
                    txtName.Clear();
                    txtDescription.Clear();
                    LoadManufacturers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
