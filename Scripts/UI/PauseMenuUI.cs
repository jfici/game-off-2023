using Godot;
using System;

public class PauseMenuUI : Control
{
    PackedScene optionsMenu;
    
    private bool canUnpause;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        optionsMenu = GD.Load<PackedScene>("res://UI/OptionsMenuUI.tscn");
        
        canUnpause = false;
    }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("ui_cancel"))
        {
            if(Player.isPaused)
            {
                if(GetTree().Root.HasNode("JoeyWorkspace/OptionsMenuUI"))
                {
                    GetParent().GetNode<Control>("OptionsMenuUI").QueueFree();
                    GetParent().GetNode<Control>("PauseMenuUI").Show();
                }
                else if(!canUnpause) canUnpause = true;
                else if(canUnpause) _on_ResumeButton_pressed();
            }
        }
    }
    
    private void _on_ResumeButton_pressed()
    {
        // Unpause game
        Player.isPaused = false;
        GetTree().Paused = false;
        QueueFree();
    }
    
    private void _on_OptionsButton_pressed()
    {
        this.Hide();
        Control instance = (Control)optionsMenu.Instance();
        GetParent<Node2D>().AddChild(instance);
        instance.SetPosition(new Vector2(500, 250));
    }
    
    private void _on_QuitMenuButton_pressed()
    {
        GetTree().ChangeScene("Scenes/Main Menu.tscn");
        GetTree().Paused = false;
        Player.isPaused = false;
    }
}
