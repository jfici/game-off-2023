using Godot;
using System;

public class OptionsMenuUI : Control
{
	public Control mainMenu;
	public Control pauseMenu;    
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(GetTree().CurrentScene.Name == "Main Menu")
		{
			mainMenu = GetParent().GetNode<Control>("MainMenuUI");
		}
		else
		{
			pauseMenu = GetParent().GetNode<Control>("PauseMenuUI");
		}
	}
	
	private void _on_Volume_value_changed(float value)
	{
		double newValue = 10 * Math.Log10(value);
		Godot.AudioServer.SetBusVolumeDb(Godot.AudioServer.GetBusIndex("Master"), Convert.ToSingle(newValue));
	}
	
	private void _on_BackButton_pressed()
	{
		Hide();
		
		// Go back to the main menu if the game hasn't started
		if(GetTree().CurrentScene.Name == "Main Menu")
		{
			mainMenu.Show();
		}
		// Go back to the pause menu if you're in-game
		else if(MenuManager.isPaused)
		{
			pauseMenu.Show();
			MenuManager.canUnpause = true;
		}
	}
}
