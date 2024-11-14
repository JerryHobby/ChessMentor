using Godot;
using System;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }

    static public int piece_size = 100; // Example value

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
            GD.Print("GameManager initialized.");
        }
        else
        {
            QueueFree();
        }
    }
}