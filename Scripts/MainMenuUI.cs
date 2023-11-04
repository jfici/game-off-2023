using Godot;
using System;

public class MainMenuUI : Control
{
   PackedScene optionsMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		optionsMenu = GD.Load<PackedScene>("res://UI/OptionsMenuUI.tscn");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	
	private void _on_StartButton_pressed()
	{
		// Load Level 1 Scene
		//GetTree().ChangeScene("[string of Level 1 path]");
	}
	
	private void _on_OptionsButton_pressed()
	{
		this.Hide();
		Control instance = (Control)optionsMenu.Instance();
		GetParent<Node2D>().AddChild(instance);
		instance.SetPosition(new Vector2(500, 250));
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
		//GetTree().ChangeScene("[insert path of scene here]");
	}
}
