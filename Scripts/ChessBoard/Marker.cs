using Godot;
using System;

public partial class Marker : Control
{
    private TextureRect shape;
    private int squareSize;

    public enum Color
    {
        White,
        Black,
        Red,
        Green,
        Yellow,
        Blue
    }

    public enum Shape
    {
        Circle,
        Square,
        Cross,
        Dot
    }

    public override void _Ready()
    {
        shape = GetNode<TextureRect>("Shape");
        squareSize = GameManager.piece_size;
        shape.Size = new Vector2(squareSize, squareSize);
        shape.Hide();
    }

    public void ShowShape(Shape shapeTexture, Color shapeColor)
    {
        string newTexturePath = "res://Assets/Shapes/";

        switch (shapeTexture)
        {
            case Shape.Square:
                newTexturePath += "Square";
                break;
            case Shape.Circle:
                newTexturePath += "Circle";
                break;
            case Shape.Cross:
                newTexturePath += "Cross";
                break;
            case Shape.Dot:
                newTexturePath += "Dot";
                break;
        }

        switch (shapeColor)
        {
            case Color.White:
                newTexturePath += "White.svg";
                break;
            case Color.Black:
                newTexturePath += "Black.svg";
                break;
            case Color.Red:
                newTexturePath += "Red.svg";
                break;
            case Color.Blue:
                newTexturePath += "Blue.svg";
                break;
            case Color.Green:
                newTexturePath += "Green.svg";
                break;
            case Color.Yellow:
                newTexturePath += "Yellow.svg";
                break;
        }

        shape.Texture = GD.Load<Texture2D>(newTexturePath);
        shape.Show();
    }
}