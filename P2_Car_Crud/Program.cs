using System.Configuration;
using Microsoft.Data.Sqlite;

namespace P2_Car_Crud
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            CreateTables();

            ApplicationConfiguration.Initialize();
            Application.Run(new Main_form());
        }

        static void CreateTables()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                con.Open();
                string cars = @"CREATE TABLE IF NOT EXISTS Cars (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT,
                            ManufacturerId INTEGER,
                            EngineCapacityId INTEGER,
                            FuelType TEXT,
                            Transmission TEXT,
                            Price REAL,
                            Year INTEGER,
                            FOREIGN KEY (ManufacturerId) REFERENCES Manufacturer(Id),
                            FOREIGN KEY (EngineCapacityId) REFERENCES EngineCapacity(Id)
                        );";
                string manufacturer = @"CREATE TABLE IF NOT EXISTS Manufacturer (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT,
                            Description TEXT
                        );";
                string engine = @"CREATE TABLE IF NOT EXISTS EngineCapacity (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT,
                            Capacity TEXT,
                            Description TEXT
                        );";
                new SqliteCommand(cars, con).ExecuteNonQuery();
                new SqliteCommand(manufacturer, con).ExecuteNonQuery();
                new SqliteCommand(engine, con).ExecuteNonQuery();
            }
        }
    }
}