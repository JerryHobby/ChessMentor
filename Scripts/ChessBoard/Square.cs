using Godot;
using System;

public partial class Square : TextureRect
{
    private static readonly Texture2D LightSquare = GD.Load<Texture2D>("res://Assets/chessThemes/board/set0/lightSquare.png");
    private static readonly Texture2D DarkSquare = GD.Load<Texture2D>("res://Assets/chessThemes/board/set0/darkSquare.png");

    private Control marker;
    private bool dark = true;

    public override void _Ready()
    {
        marker = GetNode<Control>("Marker");
        Size = new Vector2(GameManager.piece_size, GameManager.piece_size);
        Texture = LightSquare;
        MouseFilter = MouseFilterEnum.Stop;
    }

    public void SetDark()
    {
        dark = true;
        Texture = DarkSquare;
    }

    public void SetLight()
    {
        dark = false;
        Texture = LightSquare;
    }

    private void OnMouseEntered()
    {
        // marker.ShowShape(Marker.Shape.Circle, Marker.Color.White);
    }

    private void OnMouseExited()
    {
        marker.Hide();
    }
}