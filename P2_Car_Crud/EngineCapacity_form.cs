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
        int selectedEngineId = 0;
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
                    "SELECT Id,Name,Description,Capacity FROM EngineCapacity", con))
                using (var reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
                dataGridViewEngine.DataSource = dt;

                if (!dataGridViewEngine.Columns.Contains("ViewCars"))
                {
                    DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                    btn.Name = "ViewCars";
                    btn.HeaderText = "Cars";
                    btn.Text = "View Cars";
                    btn.UseColumnTextForButtonValue = true;

                    dataGridViewEngine.Columns.Add(btn);
                }
                if (!dataGridViewEngine.Columns.Contains("Edit"))
                {
                    DataGridViewButtonColumn editBtn = new DataGridViewButtonColumn();
                    editBtn.Name = "Edit";
                    editBtn.Text = "Edit";
                    editBtn.UseColumnTextForButtonValue = true;
                    dataGridViewEngine.Columns.Add(editBtn);
                }

                if (!dataGridViewEngine.Columns.Contains("Delete"))
                {
                    DataGridViewButtonColumn deleteBtn = new DataGridViewButtonColumn();
                    deleteBtn.Name = "Delete";
                    deleteBtn.Text = "Delete";
                    deleteBtn.UseColumnTextForButtonValue = true;
                    dataGridViewEngine.Columns.Add(deleteBtn);
                }
            }
        }
        private void dataGridViewEngine_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int id = Convert.ToInt32(dataGridViewEngine.Rows[e.RowIndex].Cells["Id"].Value);

            string columnName = dataGridViewEngine.Columns[e.ColumnIndex].Name;

            if (columnName == "ViewCars")
            {
                Car_form carForm = new Car_form(null, id);
                carForm.Show();
            }

            else if (columnName == "Edit")
            {
                txtName.Text = dataGridViewEngine.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                txtDescription.Text = dataGridViewEngine.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                txtCapacity.Text = dataGridViewEngine.Rows[e.RowIndex].Cells["Capacity"].Value.ToString();

                selectedEngineId = id;
                button1.Text = "Update";
            }

            else if (columnName == "Delete")
            {
                DeleteEngine(id);
            }
        }
        void DeleteEngine(int id)
        {
            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                con.Open();

                SqliteCommand cmd = new SqliteCommand(
                    "DELETE FROM EngineCapacity WHERE Id=@id", con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            LoadEngineCapacity();
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

                    if (selectedEngineId == 0)
                    {

                        string query = @"INSERT INTO EngineCapacity 
                                (Name, Description, Capacity)
                                VALUES (@name, @description, @capacity)";

                        SqliteCommand cmd = new SqliteCommand(query, con);

                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@description", txtDescription.Text);
                        cmd.Parameters.AddWithValue("@capacity", txtCapacity.Text);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        string query = @"UPDATE EngineCapacity 
                                 SET Name=@name, Description=@desc, Capacity=@cap
                                 WHERE Id=@id";

                        SqliteCommand cmd = new SqliteCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", selectedEngineId);
                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@desc", txtDescription.Text);
                        cmd.Parameters.AddWithValue("@cap", txtCapacity.Text);
                        cmd.ExecuteNonQuery();

                        selectedEngineId = 0;
                        button1.Text = "Add";
                    }

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
