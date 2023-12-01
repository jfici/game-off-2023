using Godot;
using System;

public class WinUI : Control
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

    private void _on_QuitMenuButton_pressed()
    {
        GetTree().ChangeScene("Scenes/Main Menu.tscn");
        ExitGame();
        
        // Reset progress variables
        Player.checkpointPos = new Vector2(0, 0);
        Player.powerClimb = false;
        Player.powerHighJump = false;
    }
    
    private void ExitGame()
    {
        MenuManager.isPaused = false;
        MenuManager.canUnpause = false;
        GetTree().Paused = false;
        Player.dying = false;
        Player.dead = false;
        ChameleonCompanion.dying = false;
        ChameleonCompanion.dead = false;
        SnakeCompanion.dying = false;
        SnakeCompanion.dead = false;
    }
}
