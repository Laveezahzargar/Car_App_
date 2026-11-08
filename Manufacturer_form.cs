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
using Serilog;

namespace P2_Car_Crud
{
    public partial class Manufacturer_form : Form
    {
        int selectedManufacturerId = 0;
        string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        public Manufacturer_form()
        {
            InitializeComponent();

            Color bg = Color.White;
            Color panel = Color.FromArgb(245, 245, 245);
            Color accent = Color.FromArgb(0, 120, 215);
            Color text = Color.Black;

            StyleButton(button1);


            dataGridViewManufacturer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void Manufacturer_form_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            LoadManufacturers();
        }
        private void StyleButton(Button btn)
        {
            btn.BackColor = Color.FromArgb(0, 120, 215);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
        }
        private void ApplyTheme()
        {
            this.BackColor = Color.White;
        }
        void LoadManufacturers()
        {
            try
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

                    dataGridViewManufacturer.EnableHeadersVisualStyles = false;
                    dataGridViewManufacturer.DefaultCellStyle.SelectionBackColor = Color.White;

                    dataGridViewManufacturer.Columns["ViewCars"].DefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215);
                    dataGridViewManufacturer.Columns["ViewCars"].DefaultCellStyle.ForeColor = Color.White;

                    dataGridViewManufacturer.Columns["Edit"].DefaultCellStyle.BackColor = Color.FromArgb(40, 167, 69);
                    dataGridViewManufacturer.Columns["Edit"].DefaultCellStyle.ForeColor = Color.White;

                    dataGridViewManufacturer.Columns["Delete"].DefaultCellStyle.BackColor = Color.FromArgb(220, 53, 69);
                    dataGridViewManufacturer.Columns["Delete"].DefaultCellStyle.ForeColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load manufacturers");
                MessageBox.Show("Operation failed.");
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
                carForm.ShowDialog();
            }

            else if (columnName == "Edit")
            {
                txtName.Text = dataGridViewManufacturer.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                txtDescription.Text = dataGridViewManufacturer.Rows[e.RowIndex].Cells["Description"].Value.ToString();

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
            try
            {
                using (SqliteConnection con = new SqliteConnection(connectionString))
                {
                    con.Open();

                    SqliteCommand checkCmd = new SqliteCommand(
                    "SELECT COUNT(*) FROM Cars WHERE ManufacturerId = @id", con);

                    checkCmd.Parameters.AddWithValue("@id", id);

                    long count = (long)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show($"Cannot delete.{count} car(s) are linked to this manufacturer.");
                        return;
                    }

                    SqliteCommand cmd = new SqliteCommand(
                        "DELETE FROM Manufacturer WHERE Id=@id", con);

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    Log.Warning("Manufacturer deleted: {Id}", id);
                }

                LoadManufacturers();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to delete manufacturer.");
                MessageBox.Show("Operation failed.");
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
                    if (selectedManufacturerId == 0)
                    {
                        string query = @"INSERT INTO Manufacturer
                                (Name, Description)
                                VALUES (@name, @Description)";

                        SqliteCommand cmd = new SqliteCommand(query, con);

                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                        cmd.ExecuteNonQuery();

                        Log.Information("User added new manufacturer: {ManufacturerName}", txtName.Text);
                    }
                    else
                    {
                        string query = @"UPDATE Manufacturer 
                                 SET Name=@name, Description=@description
                                 WHERE Id=@id"; 

                        SqliteCommand cmd = new SqliteCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", selectedManufacturerId);
                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@description", txtDescription.Text);
                        cmd.ExecuteNonQuery();

                        Log.Information("User updated manufacturer: {ManufacturerId}, Name: {ManufacturerName}", selectedManufacturerId, txtName.Text);

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
                Log.Error(ex, "Failed to add/update manufacturer.");
                MessageBox.Show("Operation failed.");
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
