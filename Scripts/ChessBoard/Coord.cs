using Godot;
using System;

public partial class Coord : MarginContainer
{
    [Export] public string LabelText { get; set; }

    public override void _Ready()
    {
        Label label = GetNode<Label>("Label");
        label.Text = LabelText;
    }
}