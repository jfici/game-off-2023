using Godot;
using System;

public class OptionsMenuUI : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    
    private void _on_BackButton_pressed()
    {
        QueueFree();
        if(PlayerPrototypeJoey.isPaused) GetParent().GetNode<Control>("PauseMenuUI").Show();
        else GetParent().GetNode<Control>("MainMenuUI").Show();
    }
}
