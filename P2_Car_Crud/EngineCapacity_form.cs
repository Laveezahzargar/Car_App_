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
    public partial class EngineCapacity_form : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        public EngineCapacity_form()
        {
            InitializeComponent();
        }
        private void EngineCapacity_form_Load(object sender, EventArgs e)
        {
            LoadEngineCapacity();
        }
        void LoadEngineCapacity()
        {
            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                con.Open();

                DataTable dt = new DataTable();

                using (var cmd = new SqliteCommand(
                    "SELECT Id, Capacity FROM EngineCapacity", con))
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
                if (txtName.Text == "" || txtDescription.Text == "" || txtCapacity.Text == "")
                {
                    MessageBox.Show("Please fill all fields");
                    return;
                }
                using (SqliteConnection con = new SqliteConnection(connectionString))
                {
                    con.Open();

                    string query = @"INSERT INTO EngineCapacity 
                                (Name, Description, Capacity)
                                VALUES (@name, @description, @capacity)";

                    SqliteCommand cmd = new SqliteCommand(query, con);

                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@description", txtDescription.Text);
                    cmd.Parameters.AddWithValue("@capacity", txtCapacity.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Engine capacity added successfully");
                    txtName.Clear();
                    txtDescription.Clear();
                    txtCapacity.Clear();
                    LoadEngineCapacity();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
