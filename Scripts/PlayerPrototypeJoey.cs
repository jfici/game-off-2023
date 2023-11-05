using Godot;
using System;

public class PlayerPrototypeJoey : KinematicBody2D
{
	public Vector2 velocity;
	[Export] public float speedX = 300;
	[Export] public float speedY = 700;
	[Export] public float gravity = 30;
    [Export] public float drag = 100;
	private float terminalVelocity = 1000;
	private Vector2 direction;
    
    public AnimationTree animationTree;
    
    PackedScene pauseMenu;
    public static bool isPaused;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        animationTree = this.GetNode<AnimationTree>("AnimationTree");
        animationTree.Active = true;
        
        pauseMenu = GD.Load<PackedScene>("res://UI/PauseMenuUI.tscn");
        isPaused = false;
	}

//    // Called every frame. 'delta' is the elapsed time since the previous frame.
//    public override void _Process(float delta)
//    {
//
//    }

    

	public override void _PhysicsProcess(float delta)
	{
		#region Movement
        MoveAndSlide(velocity, new Vector2(0,-1));
        
        // Horizontal movement
		direction.x = Input.GetAxis("move_left", "move_right");
		velocity.x = speedX * direction.x;
		
		// Veritcal movement and jumping
		if(Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.y = -speedY;
		}
		
		if(!IsOnFloor()) 
		{
			// Make the player fall faster in the air
            velocity.y += gravity;			
			if(velocity.y > terminalVelocity) velocity.y = terminalVelocity;
            
            // Slow the player's horizontal movement in the air
            if(direction.x > 0) velocity.x -= drag;
            else if(direction.x < 0) velocity.x += drag;
		}
        #endregion
        
        #region Animations
        // Set blend position to play run animation
        animationTree.Set("parameters/Move/blend_position", direction.x);
        
        // Flip sprite based on direction of movement
        if(direction.x > 0) this.GetNode<Sprite>("Sprite").FlipH = false;
        else if(direction.x < 0) this.GetNode<Sprite>("Sprite").FlipH = true;
        
        #endregion
        
        // Pause the game if unpaused when the player hits Esc
        if(Input.IsActionJustPressed("ui_cancel") && !isPaused) PauseGame();
	}
    
    private void PauseGame()
    {
        
        isPaused = true;
        GetTree().Paused = true;
        Control instance = (Control)pauseMenu.Instance();
		GetParent<Node2D>().AddChild(instance);
		instance.SetPosition(new Vector2(500, 250));
    }
}
