using Godot;
using System;

public class GameOverUI : Control
{
    private void _on_QuitMenuButton_pressed()
    {
        GetTree().ChangeScene("Scenes/Main Menu.tscn");
        GetTree().Paused = false;
        MenuManager.isPaused = false;
        MenuManager.canUnpause = false;
        Player.dying = false;
        Player.dead = false;
    }
}
