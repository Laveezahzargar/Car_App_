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
    public partial class Car_form : Form
    {
        int selectedCarId = 0;
        int filterManufacturerId = 0;
        int filterEngineId = 0;
        int _manuId;
        int _engineId;

        string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        public Car_form(int? manuId = null, int? engineId = null)
        {
            InitializeComponent();

            Color bg = Color.White;
            Color panel = Color.FromArgb(245, 245, 245);
            Color accent = Color.FromArgb(0, 120, 215);
            Color text = Color.Black;

            StyleButton(button1);

            _manuId = manuId ?? 0;
            _engineId = engineId ?? 0;

            if (manuId != null)
                filterManufacturerId = manuId.Value;

            if (engineId != null)
                filterEngineId = engineId.Value;
        }
        private void Car_form_Load(object sender, EventArgs e)
        {
            dataGridViewCar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            cmbTransmission.DataSource = null;
            cmbFueltype.DataSource = null;

            cmbTransmission.Items.Clear();
            cmbFueltype.Items.Clear();

            cmbTransmission.Items.Add("Manual");
            cmbTransmission.Items.Add("Automatic");

            cmbFueltype.Items.Add("Petrol");
            cmbFueltype.Items.Add("Diesel");
            cmbFueltype.Items.Add("Electric");

            ApplyTheme();

            LoadCars();
            LoadManufacturers();
            LoadEngineCapacity();

            if (_manuId != 0)
            {
                cmbManufacturer.SelectedValue = _manuId;
                cmbManufacturer.Enabled = false; 
            }
            if (_engineId != 0)
            {
                cmbEngine.SelectedValue = _engineId;
                cmbEngine.Enabled = false;
            }

            ClearForm();

            cmbTransmission.SelectedIndex = 0;
            cmbFueltype.SelectedIndex = 0;
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
        public void SetManufacturerFilter(int id)
        {
            filterManufacturerId = id;
            filterEngineId = 0;
        }

        public void SetEngineFilter(int id)
        {
            filterEngineId = id;
            filterManufacturerId = 0;
        }
        void LoadCars()
        {
            try
            {
                using (SqliteConnection con = new SqliteConnection(connectionString))
                {
                    con.Open();

                    string query = @"SELECT 
                    Cars.Id,
                    Cars.Name,
                    Manufacturer.Name AS Manufacturer, 
                    EngineCapacity.Capacity,
                    Cars.Transmission,
                    Cars.FuelType,
                    Cars.Price,
                    Cars.Year
                    FROM Cars
                    LEFT JOIN Manufacturer ON Cars.ManufacturerId = Manufacturer.Id
                    LEFT JOIN EngineCapacity ON Cars.EngineCapacityId = EngineCapacity.Id
                    WHERE 1=1
                   ";

                    if (filterManufacturerId != 0)
                    {
                        query += " AND Cars.ManufacturerId = @manuId";
                    }

                    if (filterEngineId != 0)
                    {
                        query += " AND Cars.EngineCapacityId = @engineId";
                    }

                    DataTable dt = new DataTable();

                    SqliteCommand cmd = new SqliteCommand(query, con);

                    if (filterManufacturerId != 0)
                    {
                        cmd.Parameters.AddWithValue("@manuId", filterManufacturerId);
                    }

                    if (filterEngineId != 0)
                    {
                        cmd.Parameters.AddWithValue("@engineId", filterEngineId);
                    }


                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }

                    dataGridViewCar.DataSource = dt;

                    if (!dataGridViewCar.Columns.Contains("Edit"))
                    {
                        DataGridViewButtonColumn editBtn = new DataGridViewButtonColumn();
                        editBtn.Name = "Edit";
                        editBtn.HeaderText = "Edit";
                        editBtn.Text = "Edit";
                        editBtn.UseColumnTextForButtonValue = true;
                        dataGridViewCar.Columns.Add(editBtn);
                    }

                    if (!dataGridViewCar.Columns.Contains("Delete"))
                    {
                        DataGridViewButtonColumn deleteBtn = new DataGridViewButtonColumn();
                        deleteBtn.Name = "Delete";
                        deleteBtn.HeaderText = "Delete";
                        deleteBtn.Text = "Delete";
                        deleteBtn.UseColumnTextForButtonValue = true;
                        dataGridViewCar.Columns.Add(deleteBtn);
                    }

                    dataGridViewCar.Columns["Edit"].DefaultCellStyle.BackColor = Color.FromArgb(40, 167, 69);
                    dataGridViewCar.Columns["Edit"].DefaultCellStyle.ForeColor = Color.White;
                    dataGridViewCar.Columns["Delete"].DefaultCellStyle.BackColor = Color.FromArgb(220, 53, 69);
                    dataGridViewCar.Columns["Delete"].DefaultCellStyle.ForeColor = Color.White;
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Failed to load cars");
                MessageBox.Show("Operation failed.");
            }
        }
        void LoadCarToForm(int rowIndex)
        {
            var row = dataGridViewCar.Rows[rowIndex];

            selectedCarId = Convert.ToInt32(row.Cells["Id"].Value);

            txtName.Text = row.Cells["Name"].Value.ToString();
            txtPrice.Text = row.Cells["Price"].Value.ToString();
            txtYear.Text = row.Cells["Year"].Value.ToString();

            cmbManufacturer.Text = row.Cells["Manufacturer"].Value.ToString();
            cmbEngine.Text = row.Cells["Capacity"].Value.ToString();
            cmbTransmission.Text = row.Cells["Transmission"].Value.ToString();
            cmbFueltype.Text = row.Cells["FuelType"].Value.ToString();

            button1.Text = "Update";
        }
        void DeleteCar(int id)
        {
            try
            {
                var confirm = MessageBox.Show("Delete this car?", "Confirm", MessageBoxButtons.YesNo);

                if (confirm == DialogResult.Yes)
                {
                    using (SqliteConnection con = new SqliteConnection(connectionString))
                    {
                        con.Open();

                        string query = "DELETE FROM Cars WHERE Id=@id";
                        SqliteCommand cmd = new SqliteCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();

                        Log.Warning("Car deleted: {Id}", id);
                    }

                    LoadCars();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to delete car.");
                MessageBox.Show("Operation failed.");
            }
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int carId = Convert.ToInt32(dataGridViewCar.Rows[e.RowIndex].Cells["Id"].Value);

            if (dataGridViewCar.Columns[e.ColumnIndex].Name == "Edit")
            {
                LoadCarToForm(e.RowIndex);
            }

            if (dataGridViewCar.Columns[e.ColumnIndex].Name == "Delete")
            {
                DeleteCar(carId);
            }
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

                    cmbManufacturer.DataSource = null;
                    cmbManufacturer.DataSource = dt;
                    cmbManufacturer.DisplayMember = "Name";
                    cmbManufacturer.ValueMember = "Id";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load manufacturers.");
                MessageBox.Show("Operation failed.");
            }
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
                        "SELECT Id, Capacity FROM EngineCapacity", con))
                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }

                    cmbEngine.DataSource = null;
                    cmbEngine.DataSource = dt;
                    cmbEngine.DisplayMember = "Capacity";
                    cmbEngine.ValueMember = "Id";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load engine capacities.");
                MessageBox.Show("Operation failed.");
            }
        }
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            filterManufacturerId = 0;
            filterEngineId = 0;

            LoadCars();
        }
        void ClearForm()
        {
            txtName.Clear();
            txtPrice.Clear();
            txtYear.Clear();

            cmbTransmission.SelectedIndex = 0;
            cmbFueltype.SelectedIndex = 0;

            if (_manuId != 0)
            {
                cmbManufacturer.SelectedValue = _manuId;
            }
            else if (cmbManufacturer.Items.Count > 0)
            {
                cmbManufacturer.SelectedIndex = 0;
            }

            if (_engineId != 0)
            {
                cmbEngine.SelectedValue = _engineId;
            }
            else if (cmbEngine.Items.Count > 0)
            {
                cmbEngine.SelectedIndex = 0;

                selectedCarId = 0;
                button1.Text = "Add";
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text == "" || txtPrice.Text == "" || txtYear.Text == "" ||
                        cmbManufacturer.SelectedIndex == -1 || cmbEngine.SelectedIndex == -1 ||
                        cmbTransmission.SelectedIndex == -1 ||
                        cmbFueltype.SelectedIndex == -1)
                {
                    MessageBox.Show("Please fill all fields");
                    return;
                }
                var manufacturerId = _manuId != 0 ? _manuId : cmbManufacturer.SelectedValue;
                var engineId = _engineId != 0 ? _engineId : cmbEngine.SelectedValue;
                using (SqliteConnection con = new SqliteConnection(connectionString))
                {
                    con.Open();

                    if (selectedCarId == 0)
                    {
                        string query = @"INSERT INTO Cars 
                                (Name, ManufacturerId, EngineCapacityId, Transmission, FuelType, Price, Year)
                                VALUES (@name, @manu, @engine, @transmission, @fueltype, @price, @year)";

                        SqliteCommand cmd = new SqliteCommand(query, con);

                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@manu", manufacturerId);
                        cmd.Parameters.AddWithValue("@engine", engineId);
                        cmd.Parameters.AddWithValue("@transmission", cmbTransmission.Text);
                        cmd.Parameters.AddWithValue("@fueltype", cmbFueltype.Text);
                        cmd.Parameters.AddWithValue("@price", txtPrice.Text);
                        cmd.Parameters.AddWithValue("@year", txtYear.Text);
                        cmd.ExecuteNonQuery();

                        Log.Information("User added new car: {CarName}", txtName.Text);

                        LoadCars();
                        ClearForm();
                    }
                    else
                    {
                        string query = @"UPDATE Cars SET 
                                        Name=@name,
                                        ManufacturerId=@manu,
                                        EngineCapacityId=@engine,
                                        Transmission=@transmission,
                                        FuelType=@fueltype,
                                        Price=@price,
                                        Year=@year
                                        WHERE Id=@id";

                        SqliteCommand cmd = new SqliteCommand(query, con);

                        cmd.Parameters.AddWithValue("@id", selectedCarId);
                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@manu", manufacturerId);
                        cmd.Parameters.AddWithValue("@engine", engineId);
                        cmd.Parameters.AddWithValue("@transmission", cmbTransmission.Text);
                        cmd.Parameters.AddWithValue("@fueltype", cmbFueltype.Text);
                        cmd.Parameters.AddWithValue("@price", txtPrice.Text);
                        cmd.Parameters.AddWithValue("@year", txtYear.Text);

                        cmd.ExecuteNonQuery();

                        Log.Information("User updated car: {CarId}, Name: {CarName}", selectedCarId, txtName.Text);

                        selectedCarId = 0;
                        button1.Text = "Add";
                        ClearForm();
                        LoadCars();
                    }
                   
                }
                
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to add/update car.");
                MessageBox.Show("Operation failed.");
            }

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
