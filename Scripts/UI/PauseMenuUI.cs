using Godot;
using System;

public class PauseMenuUI : Control
{
    public Control optionsMenu;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        optionsMenu = GetParent().GetNode<Control>("OptionsMenuUI");
    }
    
    private void _on_ResumeButton_pressed()
    {
        // Unpause game
        Hide();
        GetTree().Paused = false;
        MenuManager.isPaused = false;
        MenuManager.canUnpause = false;
    }
    
    private void _on_OptionsButton_pressed()
    {
        Hide();
        optionsMenu.Show();
        MenuManager.canUnpause = false;
    }
    
    private void _on_QuitMenuButton_pressed()
    {
        GetTree().ChangeScene("Scenes/Main Menu.tscn");
        GetTree().Paused = false;
        MenuManager.isPaused = false;
        MenuManager.canUnpause = false;
    }
}
