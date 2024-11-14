using Godot;
using System;
using System.Data;
using System.Data.SQLite;

public partial class SqlController : Node
{
    private SQLiteConnection database;
    private string dbPath = "res://Assets/DB/lichess_db.db";

    public override void _Ready()
    {
        GD.Print("SqlController _Ready called");
        DbConnect(dbPath);
    }

    private void DbConnect(string databasePath)
    {
        try
        {
            string absolutePath = ProjectSettings.GlobalizePath(databasePath);
            string connectionString = $"Data Source={absolutePath};Version=3;";
            database = new SQLiteConnection(connectionString);
            database.Open();
            GD.Print("Database connection opened successfully");
        }
        catch (Exception ex)
        {
            GD.PrintErr("Failed to open database connection: ", ex.Message);
        }
    }

    public DataTable DbQuery(string queryString)
    {
        try
        {
            GD.Print("Executing query: ", queryString);
            using (var command = new SQLiteCommand(queryString, database))
            {
                using (var adapter = new SQLiteDataAdapter(command))
                {
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    GD.Print("DbQuery executed successfully, rows returned: ", dataTable.Rows.Count);
                    return dataTable;
                }
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr("DbQuery failed: ", ex.Message);
            return null;
        }
    }

    public override void _ExitTree()
    {
        GD.Print("SqlController _ExitTree called");
        if (database != null && database.State == ConnectionState.Open)
        {
            GD.Print("Closing database connection");
            database.Close();
            GD.Print("Database connection closed");
        }
        else
        {
            GD.Print("Database connection was already closed or null");
        }
    }
}