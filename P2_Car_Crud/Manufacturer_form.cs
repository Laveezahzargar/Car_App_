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
        int selectedManufacturerId = 0;
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

                dataGridViewManufacturer.DataSource = dt;

                if (!dataGridViewManufacturer.Columns.Contains("ViewCars"))
                {
                    DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                    btn.Name = "ViewCars";
                    btn.HeaderText = "Cars";
                    btn.Text = "View Cars";
                    btn.UseColumnTextForButtonValue = true;

                    dataGridViewManufacturer.Columns.Add(btn);
                }
                if (!dataGridViewManufacturer.Columns.Contains("Edit"))
                {
                    DataGridViewButtonColumn editBtn = new DataGridViewButtonColumn();
                    editBtn.Name = "Edit";
                    editBtn.HeaderText = "Edit";
                    editBtn.Text = "Edit";
                    editBtn.UseColumnTextForButtonValue = true;
                    dataGridViewManufacturer.Columns.Add(editBtn);
                }

                if (!dataGridViewManufacturer.Columns.Contains("Delete"))
                {
                    DataGridViewButtonColumn deleteBtn = new DataGridViewButtonColumn();
                    deleteBtn.Name = "Delete";
                    deleteBtn.HeaderText = "Delete";
                    deleteBtn.Text = "Delete";
                    deleteBtn.UseColumnTextForButtonValue = true;
                    dataGridViewManufacturer.Columns.Add(deleteBtn);
                }
            }
        }
        private void dataGridViewManufacturer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int id = Convert.ToInt32(dataGridViewManufacturer.Rows[e.RowIndex].Cells["Id"].Value);

            string columnName = dataGridViewManufacturer.Columns[e.ColumnIndex].Name;

            if (columnName == "ViewCars")
            {
                Car_form carForm = new Car_form(id, null);
                carForm.Show();
            }

            else if (columnName == "Edit")
            {
                txtName.Text = dataGridViewManufacturer.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                selectedManufacturerId = id;
                button1.Text = "Update";
            }

            else if (columnName == "Delete")
            {
                DeleteManufacturer(id);
            }
        }
       
        void DeleteManufacturer(int id)
        {
            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                con.Open();

                SqliteCommand cmd = new SqliteCommand(
                    "DELETE FROM Manufacturer WHERE Id=@id", con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            LoadManufacturers();
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
                    if (selectedManufacturerId == 0)
                    {
                        string query = @"INSERT INTO Manufacturer
                                (Name, Description)
                                VALUES (@name, @Description)";

                        SqliteCommand cmd = new SqliteCommand(query, con);

                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        string query = @"UPDATE Manufacturer 
                                 SET Name=@name, Description=@desc
                                 WHERE Id=@id";

                        SqliteCommand cmd = new SqliteCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", selectedManufacturerId);
                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@desc", txtDescription.Text);
                        cmd.ExecuteNonQuery();

                        selectedManufacturerId = 0;
                        button1.Text = "Add";

                    }
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
