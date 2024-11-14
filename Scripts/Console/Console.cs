using Godot;
using System;

public partial class Console : VBoxContainer
{
    private string consoleText = "";
    private const int MaxConsoleSize = 50000;

    [Export] private NodePath consoleNodePath;
    [Export] private NodePath commandNodePath;
    private TextEdit consoleNode;
    private LineEdit commandNode;
    private UciEngine uciEngine;

    public override void _Ready()
    {
        consoleNode = GetNode<TextEdit>(consoleNodePath);
        commandNode = GetNode<LineEdit>(commandNodePath);
        uciEngine = GetNode<UciEngine>("/root/UciEngine");

        if (uciEngine != null)
        {
            uciEngine.NewUciText += OnNewText;
            uciEngine.Write("help");
        }
        else
        {
            GD.PrintErr("UciEngine node not found");
        }
    }

    private void OnMovePressed()
    {
        uciEngine.Write("go movetime 1000");
    }

    private void OnNewText(string line)
    {
        consoleNode.Text += line + "\n";
        if (consoleNode.Text.Length > MaxConsoleSize)
        {
            consoleNode.Text = consoleNode.Text.Substring(consoleNode.Text.Length - MaxConsoleSize);
        }
        consoleNode.ScrollVertical = consoleNode.GetVScrollBar().MaxValue;
    }
}