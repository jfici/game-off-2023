using Godot;
using System;

public class MenuManager : CanvasLayer
{
    // Menu UI variables
    public Control pauseMenu;
    public Control optionsMenu;
    public Control gameOverMenu;
    
    // Pause game bools
    public static bool isPaused;
    public static bool canUnpause;
    
    // Player power indicator UI variables
    public ColorRect chameleonIcon;
    public ColorRect snakeIcon;
    
    // Checkpoint UI variables
    public Label checkpointLabel;
    public Timer checkpointTextTimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Initializing UI variables
        pauseMenu = GetNode<Control>("PauseMenuUI");
        optionsMenu = GetNode<Control>("OptionsMenuUI");
        gameOverMenu = GetNode<Control>("GameOverUI");
        chameleonIcon = GetNode<ColorRect>("ChameleonRect");
        snakeIcon = GetNode<ColorRect>("SnakeRect");
        checkpointLabel = GetNode<Label>("CheckpointLabel");
        checkpointTextTimer = GetNode<Timer>("CheckpointLabel/CheckpointTextTimer");
        
        // Hiding UI when game scene first loads
        pauseMenu.Hide();
        optionsMenu.Hide();
        gameOverMenu.Hide();
        chameleonIcon.Hide();
        snakeIcon.Hide();
        checkpointLabel.Hide();
        
        // Make sure the game can be paused when game scene first loads
        isPaused = false;
        canUnpause = false;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        #region Pause Logic
        // Pause the game if unpaused when the player hits Esc
        if(Input.IsActionJustPressed("ui_cancel")) 
        {
            // Pause the game if it isn't already
            if(!isPaused)
            {
                PauseGame();
            }
            else if(isPaused)
            {
                // Unpause the game if you're in the pause menu
                if(canUnpause)
                {
                    pauseMenu.Hide();
                    GetTree().Paused = false;
                    isPaused = false;
                    canUnpause = false;
                }
                // Back out to the pause menu if you're in the options menu
                else if(!canUnpause) 
                {
                    optionsMenu.Hide();
                    pauseMenu.Show();
                    canUnpause = true;
                }
            }
        }
        #endregion
        
        // Show animal icons when player has their power enabled
        if(Player.powerClimb)
        {
            chameleonIcon.Show();
            snakeIcon.Hide();
        }
        else if(Player.powerHighJump)
        {
            chameleonIcon.Hide();
            snakeIcon.Show();
        }
        
        // Show checkpoint text for a short period after player reaches a checkpoint
        if(Player.checkpointSet)
        {
            checkpointLabel.Show();
            checkpointTextTimer.Start();
            Player.checkpointSet = false;    
        }
        
        // Game Over
        if(Player.dead || ChameleonCompanion.dead || SnakeCompanion.dead)
        {
            gameOverMenu.Show();
            GetTree().Paused = true;
        }
    }
    
    private void PauseGame()
    {
        pauseMenu.Show();
        GetTree().Paused = true;
        isPaused = true;
        canUnpause = true;
    }
    
    private void _on_CheckpointTextTimer_timeout()
    {
        checkpointLabel.Hide();
    }
}
