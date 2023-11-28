using Godot;
using System;

public class GameOverUI : Control
{
    public Label deathLabel;
    
    public override void _Ready()
	{
        // Get label to set game over reason text when someone dies
        deathLabel = GetNode<Label>("Panel/GameOverMenuContainer/YouDiedLabel");
	}

	public override void _PhysicsProcess(float delta)
    {
        if(Visible)
        {
            if(Player.dead)
            {
                deathLabel.Text = "You died :(";
            }
            else if(ChameleonCompanion.dead)
            {
                deathLabel.Text = "A friend died :(";
            }
        }
    }
    
    private void _on_QuitMenuButton_pressed()
    {
        GetTree().ChangeScene("Scenes/Main Menu.tscn");
        GetTree().Paused = false;
        MenuManager.isPaused = false;
        MenuManager.canUnpause = false;
        GetTree().Paused = false;
        Player.dying = false;
        Player.dead = false;
        ChameleonCompanion.dying = false;
        ChameleonCompanion.dead = false;
    }
}
