using Godot;
using System;
using System.Collections.Generic;

public partial class PieceController : Node2D
{
    private static readonly Dictionary<char, PackedScene> PieceScenes = new Dictionary<char, PackedScene>
    {
        { 'P', GD.Load<PackedScene>("res://Scenes/Pieces/white_pawn.tscn") },
        { 'R', GD.Load<PackedScene>("res://Scenes/Pieces/white_rook.tscn") },
        { 'N', GD.Load<PackedScene>("res://Scenes/Pieces/white_knight.tscn") },
        { 'B', GD.Load<PackedScene>("res://Scenes/Pieces/white_bishop.tscn") },
        { 'Q', GD.Load<PackedScene>("res://Scenes/Pieces/white_queen.tscn") },
        { 'K', GD.Load<PackedScene>("res://Scenes/Pieces/white_king.tscn") },
        { 'p', GD.Load<PackedScene>("res://Scenes/Pieces/black_pawn.tscn") },
        { 'r', GD.Load<PackedScene>("res://Scenes/Pieces/black_rook.tscn") },
        { 'n', GD.Load<PackedScene>("res://Scenes/Pieces/black_knight.tscn") },
        { 'b', GD.Load<PackedScene>("res://Scenes/Pieces/black_bishop.tscn") },
        { 'q', GD.Load<PackedScene>("res://Scenes/Pieces/black_queen.tscn") },
        { 'k', GD.Load<PackedScene>("res://Scenes/Pieces/black_king.tscn") }
    };

    [Export] private NodePath square1Path;
    private TextureRect square1;
    private ChessManager chessManager;
    private List<Node> board = new List<Node>();
    private Vector2 pieceOffset;
    private Vector2 originalPosition;

    public override void _Ready()
    {
        square1 = GetNode<TextureRect>(square1Path);
        chessManager = GetNode<ChessManager>("/root/ChessManager");
        pieceOffset = square1.Size / 3;

        // Example FEN string for initial position
        string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        SetupBoardFromFen(fen);
    }

    public void SetupBoardFromFen(string fen)
    {
        ClearBoard();
        string[] rows = fen.Split(' ')[0].Split('/');
        for (int rowIndex = 0; rowIndex < 8; rowIndex++)
        {
            string row = rows[rowIndex];
            int colIndex = 0;
            foreach (char ch in row)
            {
                if (char.IsDigit(ch))
                {
                    colIndex += (int)char.GetNumericValue(ch);
                }
                else
                {
                    PlacePiece(ch, rowIndex, colIndex);
                    colIndex++;
                }
            }
        }

        // Notify ChessManager of the new FEN
        chessManager.UpdateBoardFromFen(fen);
    }

    private void ClearBoard()
    {
        foreach (Node piece in board)
        {
            piece.QueueFree();
        }
        board.Clear();
    }

    private void PlacePiece(char pieceChar, int row, int col)
    {
        if (PieceScenes.TryGetValue(pieceChar, out PackedScene pieceScene))
        {
            Control pieceInstance = pieceScene.Instantiate<Control>();
            TextureRect textureRect = pieceInstance.GetNode<TextureRect>("TextureRect");

            textureRect.Size = new Vector2(GameManager.piece_size, GameManager.piece_size);
            textureRect.Scale = new Vector2(0.8f, 0.8f);
            textureRect.MouseDefaultCursorShape = Control.CursorShape.PointingHand;

            pieceInstance.GuiInput += (InputEvent @event) => OnPieceInputEvent(pieceInstance, @event);

            AddChild(pieceInstance);
            pieceInstance.Position = pieceOffset + new Vector2(col * textureRect.Size.X, row * textureRect.Size.Y);
            board.Add(pieceInstance);
        }
    }

    private void OnPieceInputEvent(Control pieceInstance, InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.Pressed)
            {
                // Store the original position in board coordinates when the piece is picked up
                originalPosition = pieceInstance.Position / GameManager.piece_size;
            }
            else
            {
                PieceDropped(pieceInstance, pieceInstance.Position);
            }
        }
        else if (@event is InputEventMouseMotion mouseMotionEvent)
        {
            if ((mouseMotionEvent.ButtonMask & MouseButtonMask.Left) != 0)
            {
                pieceInstance.Position += mouseMotionEvent.Relative;
            }
        }
    }

    public void MovePiece(Control pieceInstance, int newRow, int newCol)
    {
        pieceInstance.Position = pieceOffset + new Vector2(newCol * GameManager.piece_size, newRow * GameManager.piece_size);
    }

    public void RemovePiece(Node pieceInstance)
    {
        pieceInstance.QueueFree();
    }

    public void PieceDropped(Control pieceInstance, Vector2 newPosition)
    {
        Vector2 adjustedPosition = newPosition / GameManager.piece_size;

        // Calculate the new row and column based on the adjusted position
        int newRow = (int)Math.Floor(adjustedPosition.Y);
        int newCol = (int)Math.Floor(adjustedPosition.X);

        // Convert the positions to chess notation
        string fromSquare = ConvertToChessSquare(originalPosition);
        string toSquare = ConvertToChessSquare(new Vector2(newCol, newRow));
        string move = fromSquare + toSquare;

        // Validate the move with ChessManager
        if (chessManager.IsValidMove(move))
        {
            // Remove any piece at the target square
            RemovePieceAtPosition(newRow, newCol, pieceInstance);

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

    private void RemovePieceAtPosition(int row, int col, Control movingPiece)
    {
        foreach (Node piece in board)
        {
            Control pieceControl = (Control)piece;
            if (pieceControl == movingPiece)
            {
                continue; // Skip the piece being moved
            }

            Vector2 piecePosition = pieceControl.Position / GameManager.piece_size;
            int pieceRow = (int)Math.Floor(piecePosition.Y);
            int pieceCol = (int)Math.Floor(piecePosition.X);

            if (pieceRow == row && pieceCol == col)
            {
                piece.QueueFree();
                board.Remove(piece);
                break;
            }
        }
    }

    private string ConvertToChessSquare(Vector2 boardPosition)
    {
        int fileIndex = (int)Math.Floor(boardPosition.X);
        int rankIndex = (int)Math.Floor(boardPosition.Y);
        char file = (char)('a' + fileIndex);
        char rank = (char)('8' - rankIndex);
        return file.ToString() + rank.ToString();
    }

    private void ReturnPieceToOriginalPosition(Control pieceInstance)
    {
        pieceInstance.Position = originalPosition * GameManager.piece_size;
    }
}