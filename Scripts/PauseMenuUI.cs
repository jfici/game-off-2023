using Godot;
using System;

public class PauseMenu : Control
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
    
    private void _on_ResumeButton_pressed()
    {
        // Unpause game
        SetProcess(IsProcessing());
        QueueFree();
    }
    
    private void _on_OptionsButton_pressed()
    {
        GetNode<VBoxContainer>("PauseMenuContainer").Hide();
        Control instance = (Control)optionsMenu.Instance();
        GetParent<Node2D>().AddChild(instance);
        instance.SetPosition(new Vector2(500, 250));
    }
    
    private void _on_QuitMenuButton_pressed()
    {
        GetTree().ChangeScene("Main Menu.tscn");
    }
}
