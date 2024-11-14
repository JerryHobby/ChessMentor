using Godot;
using System;

public partial class Piece : Control
{
    private bool isWhite;
    private string type;
    private Vector2 originalPosition;
    private Vector2 pieceOffset;
    private ChessManager chessManager;

    public override void _Ready()
    {
        // Initialize piece properties
        chessManager = GetNode<ChessManager>("/root/ChessManager");
    }

    public void SetPiece(bool isWhite, string type)
    {
        this.isWhite = isWhite;
        this.type = type;
    }

    public void OnPieceInputEvent(Node pieceInstance, InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.Pressed)
            {
                // Store the original position in board coordinates when the piece is picked up
                originalPosition = ((Control)pieceInstance).Position / ((Control)pieceInstance).Size;
                GD.Print("Original Position:", originalPosition);
            }
        }
    }

    public void PieceDropped(Node pieceInstance, Vector2 newPosition)
    {
        // Adjust the new position by subtracting the piece offset
        Vector2 adjustedPosition = newPosition - pieceOffset;

        // Calculate the new row and column based on the adjusted position
        int newRow = (int)(adjustedPosition.Y / ((Control)pieceInstance).Size.Y);
        int newCol = (int)(adjustedPosition.X / ((Control)pieceInstance).Size.X);

        // Convert the positions to chess notation
        string fromSquare = ConvertToChessSquare(originalPosition);
        string toSquare = ConvertToChessSquare(new Vector2(newCol, newRow));
        string move = fromSquare + toSquare;

        // Validate the move with ChessManager
        if (chessManager.IsValidMove(move))
        {
            chessManager.MakeMove(move);
            MovePiece(pieceInstance, newRow, newCol);
        }
        else
        {
            // Return piece to original position
            GD.Print("Invalid move: " + move);
            ReturnPieceToOriginalPosition(pieceInstance);
        }
    }

    private string ConvertToChessSquare(Vector2 boardPosition)
    {
        char file = (char)('a' + (int)boardPosition.X);
        char rank = (char)('8' - (int)boardPosition.Y);
        return file.ToString() + rank.ToString();
    }

    private void ReturnPieceToOriginalPosition(Node pieceInstance)
    {
        ((Control)pieceInstance).Position = originalPosition * ((Control)pieceInstance).Size + pieceOffset;
    }

    private void MovePiece(Node pieceInstance, int newRow, int newCol)
    {
        ((Control)pieceInstance).Position = pieceOffset + new Vector2(newCol * ((Control)pieceInstance).Size.X, newRow * ((Control)pieceInstance).Size.Y);
    }
}