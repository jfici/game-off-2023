using Godot;
using System;

public class MainMenuUI : Control
{
   public Control optionsMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Initialize the options menu variable and hide the UI
        optionsMenu = GetParent().GetNode<Control>("OptionsMenuUI");
        optionsMenu.Hide();
	}
	
	private void _on_StartButton_pressed()
	{
		// Load the level scene
		GetTree().ChangeScene("res://Scenes/Climb Dungeon.tscn");
	}
	
	private void _on_OptionsButton_pressed()
	{
		Hide();
		optionsMenu.Show();
	}
	
	private void _on_QuitButton_pressed()
	{
		GetTree().Quit();
	}
	
	// Placeholder methods to load prototype levels
	private void _on_JoeyButton_pressed()
	{
		// Load Joey's prototype scene
		GetTree().ChangeScene("res://Scenes/JoeyWorkspace.tscn");
	}
	private void _on_BrandonButton_pressed()
	{
		// Load Brandon's prototype scene
		GetTree().ChangeScene("res://Scenes/Brandon/Brandon.tscn");
	}
}
