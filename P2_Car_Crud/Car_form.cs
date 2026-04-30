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
    public partial class Car_form : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        public Car_form()
        {
            InitializeComponent();
        }
        private void Car_form_Load(object sender, EventArgs e)
        {
            cmbTransmission.DataSource = null;
            cmbFueltype.DataSource = null;

            cmbTransmission.Items.Clear();
            cmbFueltype.Items.Clear();

            cmbTransmission.Items.Add("Manual");
            cmbTransmission.Items.Add("Automatic");

            cmbFueltype.Items.Add("Petrol");
            cmbFueltype.Items.Add("Diesel");
            cmbFueltype.Items.Add("Electric");


            LoadCars();
            LoadManufacturers();
            LoadEngineCapacity();

            ClearForm();

            cmbTransmission.SelectedIndex = 0;
            cmbFueltype.SelectedIndex = 0;
        }
        void LoadCars()
        {
            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                con.Open();

                string query = @"SELECT 
                    Cars.Id,
                    Cars.Name,
                    Manufacturer.Name AS Manufacturer,
                    EngineCapacity.Capacity,
                    Cars.Price,
                    Cars.Year
                    FROM Cars
                    LEFT JOIN Manufacturer ON Cars.ManufacturerId = Manufacturer.Id
                    LEFT JOIN EngineCapacity ON Cars.EngineCapacityId = EngineCapacity.Id
                   ";

                DataTable dt = new DataTable();

                using (var cmd = new SqliteCommand(query, con))
                using (var reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }

                dataGridView1.DataSource = dt;
            }
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

                cmbManufacturer.DataSource = null;
                cmbManufacturer.DataSource = dt;
                cmbManufacturer.DisplayMember = "Name";
                cmbManufacturer.ValueMember = "Id";
            }
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

                cmbEngine.DataSource = null;
                cmbEngine.DisplayMember = "Capacity";
                cmbEngine.ValueMember = "Id";
                cmbEngine.DataSource = dt;
            }
        }
        void ClearForm()
        {
            txtName.Clear();
            txtPrice.Clear();
            txtYear.Clear();

            cmbManufacturer.SelectedIndex = 0;
            cmbEngine.SelectedIndex = 0;
            cmbTransmission.SelectedIndex = 0;
            cmbFueltype.SelectedIndex = 0;
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
                using (SqliteConnection con = new SqliteConnection(connectionString))
                {
                    con.Open();

                    string query = @"INSERT INTO Cars 
                                (Name, ManufacturerId, EngineCapacityId, Transmission, FuelType, Price, Year)
                                VALUES (@name, @manu, @engine, @transmission, @fueltype, @price, @year)";

                    SqliteCommand cmd = new SqliteCommand(query, con);

                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@manu", cmbManufacturer.SelectedValue);
                    cmd.Parameters.AddWithValue("@engine", cmbEngine.SelectedValue);
                    cmd.Parameters.AddWithValue("@transmission", cmbTransmission.Text);
                    cmd.Parameters.AddWithValue("@fueltype", cmbFueltype.Text);
                    cmd.Parameters.AddWithValue("@price", txtPrice.Text);
                    cmd.Parameters.AddWithValue("@year", txtYear.Text);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Car added successfully");
                LoadCars();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
