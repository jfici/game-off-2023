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
            else if(ChameleonCompanion.dead || SnakeCompanion.dead)
            {
                deathLabel.Text = "A friend died :(";
            }
        }
    }
    
    private void _on_RetryButton_pressed()
    {
        GetTree().ReloadCurrentScene();
        ExitGameOver();
    }
    
    private void _on_QuitMenuButton_pressed()
    {
        GetTree().ChangeScene("Scenes/Main Menu.tscn");
        ExitGameOver();
    }
    
    private void ExitGameOver()
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
        ChameleonCompanion.companionJump = true;
        ChameleonCompanion.timeToJump = 0f;
        SnakeCompanion.companionJump = true;
        SnakeCompanion.timeToJump = 0f;
    }
}
