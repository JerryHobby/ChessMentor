using System;
using System.Collections.Generic;
using Godot;
using ChessDotNet;

public partial class ChessManager : Node
{
    private ChessGame game;
    private UciEngine uciEngine;

    public override void _Ready()
    {
        // Initialize the UCI engine
        uciEngine = GetNode<UciEngine>("/root/UciEngine");  // Adjust the path to your UciEngine node

        // Initialize the chess game
        game = new ChessGame();

        // Example FEN string for initial position
        string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        game = new ChessGame(fen);
    }

    public bool IsValidMove(string move)
    {
        GD.Print("Trying Move: " + move);
        // Convert the move string to a Move object
        Move parsedMove = new Move(move.Substring(0, 2), move.Substring(2, 2), game.WhoseTurn);
        return game.IsValidMove(parsedMove);
    }

    public void MakeMove(string move)
    {
        if (IsValidMove(move))
        {
            // Convert the move string to a Move object
            Move parsedMove = new Move(move.Substring(0, 2), move.Substring(2, 2), game.WhoseTurn);
            game.MakeMove(parsedMove, true);
            string newFen = game.GetFen();
            uciEngine.Write("position fen " + newFen);  // Call Write method on uciEngine
        }
        else
        {
            // Handle invalid move (return piece to original location)
            GD.Print("Invalid move: " + move);
            // Logic to return the piece to its original location
            // This will depend on how the board and pieces are managed in your game
        }
    }

    public void UpdateBoardFromFen(string fen)
    {
        GD.Print("UpdateBoardFromFen called with FEN: ", fen);
        game = new ChessGame(fen);
    }
}