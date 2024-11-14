using Godot;
using System;

public partial class MainMenuBar : MenuBar
{
    private ChessPuzzles puzzleDb;

    private enum GameMenu
    {
        NewGame,
        NewPuzzle,
        LoadFen,
        LoadPgn,
        Hint
    }

    private enum AcctMenu
    {
        Profile = 0,
        GameSettings = 2,
        SignIn = 3
    }

    private enum HelpMenu
    {
        About,
        Support
    }

    [Export] private NodePath boardPath;
    [Export] private NodePath puzzleDbPath;
    private Node board;
    private UciEngine uciEngine;

    public override void _Ready()
    {
        GD.Print("MainMenuBar _Ready called");

        board = GetNode(boardPath);
        if (board != null)
        {
            GD.Print("Board node found: ", board.Name);
        }
        else
        {
            GD.Print("Board node is null");
        }

        uciEngine = GetNode<UciEngine>("/root/UciEngine");
        if (uciEngine != null)
        {
            GD.Print("UciEngine node found: ", uciEngine.Name);
        }
        else
        {
            GD.Print("UciEngine node is null");
        }

        puzzleDb = GetNode<ChessPuzzles>(puzzleDbPath);
        if (puzzleDb != null)
        {
            GD.Print("ChessPuzzles node found: ", puzzleDb.Name);
        }
        else
        {
            GD.Print("ChessPuzzles node is null");
        }
    }

    public override void _ExitTree()
    {
        GD.Print("MainMenuBar _ExitTree called");
        // Clean up resources if needed
        if (uciEngine != null)
        {
            GD.Print("Calling uciEngine.CleanUp()");
            uciEngine.CleanUp();
        }
    }

    private void _on_game_id_pressed(int id)
    {
        GD.Print("_on_game_id_pressed called with id: ", id);
        if (Enum.IsDefined(typeof(GameMenu), id))
        {
            OnGameIdPressed(id);
        }
    }

    private void _on_account_id_pressed(int id)
    {
        GD.Print("_on_account_id_pressed called with id: ", id);
        if (Enum.IsDefined(typeof(AcctMenu), id))
        {
            OnAcctIdPressed(id);
        }
    }

    private void _on_help_id_pressed(int id)
    {
        GD.Print("_on_help_id_pressed called with id: ", id);
        if (Enum.IsDefined(typeof(HelpMenu), id))
        {
            OnHelpIdPressed(id);
        }
    }

    private void OnGameIdPressed(int id)
    {
        GD.Print("OnGameIdPressed called with id: ", id);
        switch ((GameMenu)id)
        {
            case GameMenu.NewGame:
                // Implement new game logic
                break;
            case GameMenu.NewPuzzle:
                CallDeferred(nameof(LoadPuzzle));
                break;
            case GameMenu.LoadFen:
                // Implement load FEN logic
                break;
            case GameMenu.LoadPgn:
                // Implement load PGN logic
                break;
            case GameMenu.Hint:
                Hint();
                break;
        }
    }

    private void OnAcctIdPressed(int id)
    {
        GD.Print("OnAcctIdPressed called with id: ", id);
        switch ((AcctMenu)id)
        {
            case AcctMenu.Profile:
                // Handle Profile
                break;
            case AcctMenu.GameSettings:
                // Handle Game Settings
                break;
            case AcctMenu.SignIn:
                // Handle Sign In
                break;
        }
    }

    private void OnHelpIdPressed(int id)
    {
        GD.Print("OnHelpIdPressed called with id: ", id);
        switch ((HelpMenu)id)
        {
            case HelpMenu.About:
                // Handle About
                break;
            case HelpMenu.Support:
                // Handle Support
                break;
        }
    }

    private async void LoadPuzzle()
    {
        GD.Print("LoadPuzzle called");

        // Simulate async operation
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");

        string fen = puzzleDb.GetRandomFen();
        GD.Print("FEN from puzzleDb: ", fen);

        if (string.IsNullOrEmpty(fen))
        {
            GD.Print("No FEN available");
            return;
        }

        string command = "position fen " + fen + " moves";
        GD.Print("UCI command: ", command);

        if (board.HasMethod("SetupBoardFromFen"))
        {
            GD.Print("Calling SetupBoardFromFen on board");
            board.Call("SetupBoardFromFen", fen);
        }
        else
        {
            GD.Print("Board node does not have method SetupBoardFromFen");
        }

        uciEngine.Write(command);
        uciEngine.Write("d");
    }

    private void Hint()
    {
        GD.Print("Hint called");
        uciEngine.Write("go movetime 1000");
    }
}