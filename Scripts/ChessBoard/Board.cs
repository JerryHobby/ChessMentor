using Godot;
using System;

public partial class Board : MarginContainer
{
    [Export] private NodePath gridContainerPath;
    [Export] private NodePath pieceControllerPath;
    private GridContainer gridContainer;
    private PieceController pieceController;

    public override void _Ready()
    {
        GD.Print("Board _Ready called");

        gridContainer = GetNode<GridContainer>(gridContainerPath);
        if (gridContainer != null)
        {
            GD.Print("GridContainer found: ", gridContainer.Name);
            InitializeBoard();
        }
        else
        {
            GD.Print("GridContainer is null");
        }

        pieceController = GetNode<PieceController>(pieceControllerPath);
        if (pieceController != null)
        {
            GD.Print("PieceController found: ", pieceController.Name);
        }
        else
        {
            GD.Print("PieceController is null");
        }
    }

    private void InitializeBoard()
    {
        int squareSize = GameManager.piece_size; // Use GameManager.piece_size for dimensions

        for (int i = 1; i <= 64; i++)
        {
            string nodeName = "Square" + i;
            var node = gridContainer.GetNode<TextureRect>(nodeName);
            if (node != null)
            {
                node.CustomMinimumSize = new Vector2(squareSize, squareSize);

                int row = (i - 1) / 8;
                int col = (i - 1) % 8;

                // Set the color based on the row and column
                if ((row % 2 == 0 && col % 2 == 0) || (row % 2 == 1 && col % 2 == 1))
                {
                    SetDark(node);
                }
                else
                {
                    SetLight(node);
                }
            }
            else
            {
                GD.Print("Node not found: ", nodeName);
            }
        }
    }

    private void SetDark(TextureRect node)
    {
        node.Modulate = new Color(0.5f, 0.5f, 0.5f); // Example dark color
    }

    private void SetLight(TextureRect node)
    {
        node.Modulate = new Color(1.0f, 1.0f, 1.0f); // Example light color
    }

    public void SetupBoardFromFen(string fen)
    {
        if (pieceController != null)
        {
            pieceController.SetupBoardFromFen(fen);
        }
        else
        {
            GD.Print("PieceController is not available to set up the board from FEN");
        }
    }
}