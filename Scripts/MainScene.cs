using Godot;
using System;

public partial class MainScene : Control
{
	public override void _Ready()
	{
		GD.Print("MainScene _Ready called");

		// Defer any additional initialization if needed
		CallDeferred(nameof(InitializeAdditionalComponents));
	}

	private void InitializeAdditionalComponents()
	{
		GD.Print("Initializing additional components");

		// Add any additional initialization here if needed

		// Run tests if needed
		// RunTests();
	}

	public override void _ExitTree()
	{
		GD.Print("MainScene _ExitTree called");

		// Clean up resources if needed
		// Ensure global scripts are properly cleaned up
	}
}