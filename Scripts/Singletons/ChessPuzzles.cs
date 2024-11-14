using Godot;
using System;
using System.Data;
using System.Collections.Generic;

public partial class ChessPuzzles : Node
{
    private SqlController sqlController;
    private Stack<string> puzzleStack = new Stack<string>();

    public override void _Ready()
    {
        GD.Print("ChessPuzzles _Ready called");
        CallDeferred(nameof(Initialize));
    }

    private void Initialize()
    {
        GD.Print("Initializing ChessPuzzles");
        sqlController = GetTree().Root.GetNode<SqlController>("SqlController");
        if (sqlController != null)
        {
            GD.Print("SqlController found in ChessPuzzles");
            LoadPuzzles();
        }
        else
        {
            GD.Print("SqlController is null in ChessPuzzles");
            // Print the list of nodes under /root to help debug
            PrintRootNodes();
        }
    }

    private void PrintRootNodes()
    {
        GD.Print("Listing nodes under /root:");
        foreach (Node node in GetTree().Root.GetChildren())
        {
            GD.Print("Node: ", node.Name);
        }
    }

    private void LoadPuzzles()
    {
        GD.Print("LoadPuzzles called");
        if (sqlController == null)
        {
            GD.Print("SqlController is null in LoadPuzzles");
            return;
        }

        string sqlQuery = "SELECT fen FROM puzzles ORDER BY RANDOM() LIMIT 100;";
        DataTable result = sqlController.DbQuery(sqlQuery);

        if (result == null)
        {
            GD.Print("DbQuery returned null");
            return;
        }

        puzzleStack.Clear();
        foreach (DataRow row in result.Rows)
        {
            puzzleStack.Push(row["fen"].ToString());
        }
        GD.Print("Loaded ", puzzleStack.Count, " puzzles");
    }

    public string GetRandomFen()
    {
        if (puzzleStack.Count == 0)
        {
            LoadPuzzles();
        }
        if (puzzleStack.Count > 0)
        {
            return puzzleStack.Pop();
        }
        else
        {
            GD.Print("No puzzles available");
            return string.Empty;
        }
    }
}