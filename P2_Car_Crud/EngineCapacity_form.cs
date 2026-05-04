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
    public partial class EngineCapacity_form : Form
    {
        int selectedEngineId = 0;
        string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        public EngineCapacity_form()
        {
            InitializeComponent();

            Color bg = Color.White;
            Color panel = Color.FromArgb(245, 245, 245);
            Color accent = Color.FromArgb(0, 120, 215);
            Color text = Color.Black;
        }
        private void EngineCapacity_form_Load(object sender, EventArgs e)
        {
            StyleButton(button1);
            ApplyTheme();
            LoadEngineCapacity();
            dataGridViewEngine.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
        void LoadEngineCapacity()
        {
            try
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
                    dataGridViewEngine.EnableHeadersVisualStyles = false;
                    dataGridViewEngine.DefaultCellStyle.SelectionBackColor = Color.White;

                    dataGridViewEngine.Columns["ViewCars"].DefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215);
                    dataGridViewEngine.Columns["ViewCars"].DefaultCellStyle.ForeColor = Color.White;

                    dataGridViewEngine.Columns["Edit"].DefaultCellStyle.BackColor = Color.FromArgb(40, 167, 69);
                    dataGridViewEngine.Columns["Edit"].DefaultCellStyle.ForeColor = Color.White;
                    dataGridViewEngine.Columns["Delete"].DefaultCellStyle.BackColor = Color.FromArgb(220, 53, 69);
                    dataGridViewEngine.Columns["Delete"].DefaultCellStyle.ForeColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load engine capacities.");
                MessageBox.Show("Operation failed.");
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
                carForm.ShowDialog();
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
            try
            {
                using (SqliteConnection con = new SqliteConnection(connectionString))
                {
                    con.Open();

                    SqliteCommand checkCmd = new SqliteCommand(
                    "SELECT COUNT(*) FROM Cars WHERE EngineCapacityId = @id", con);

                    checkCmd.Parameters.AddWithValue("@id", id);

                    long count = (long)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show($"Cannot delete.{count} car(s) are linked to this engine capacity.");
                        return;
                    }

                    SqliteCommand cmd = new SqliteCommand(
                        "DELETE FROM EngineCapacity WHERE Id=@id", con);

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    Log.Warning("Engine capacity deleted: {Id}", id);
                }

                LoadEngineCapacity();
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Failed to delete engine capacities.");
                MessageBox.Show("Operation failed.");
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

                        Log.Information("User added new engine capacity: {EngineCapacityName}", txtName.Text);
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

                        Log.Information("User updated engine capacity: {EngineCapacityId}, Name: {EngineCapacityName}", selectedEngineId, txtName.Text);

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
                Log.Error(ex, "Failed to add engine.");
                MessageBox.Show("Operation failed.");
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
