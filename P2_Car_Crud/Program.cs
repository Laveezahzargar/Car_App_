using Microsoft.Data.Sqlite;
using Serilog;
using Serilog.Events;
using System.Configuration;

namespace P2_Car_Crud
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .Enrich.WithThreadId()
               .WriteTo.Console()

               .WriteTo.File("logs/info-.txt",
                   rollingInterval: RollingInterval.Day,
                   restrictedToMinimumLevel: LogEventLevel.Information,
                    outputTemplate:
                "{Timestamp:HH:mm:ss} | {Level:u3} | [Thread:{ThreadId}] | {SourceContext} | {Message:lj}{NewLine}{Exception}{NewLine}------------------------{NewLine}"
                   )

               .WriteTo.File(
                "logs/error-.txt",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Error,
                outputTemplate:
                "{Timestamp:HH:mm:ss} | {Level:u3} | [Thread:{ThreadId}] | {SourceContext} | {Message:lj}{NewLine}{Exception}{NewLine}------------------------{NewLine}"
    )

               .CreateLogger();
            try
            {
                Application.ThreadException += (sender, args) =>
                {
                    Log.Error(args.Exception, "Unhandled UI exception");
                    MessageBox.Show("Unexpected error occurred.");
                };

                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    Log.Fatal((Exception)args.ExceptionObject, "Fatal error");
                };


                CreateTables();

                ApplicationConfiguration.Initialize();
                Application.Run(new Main_form());
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application crashed on startup");
            }
            finally
            {
                Log.CloseAndFlush();
            }
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