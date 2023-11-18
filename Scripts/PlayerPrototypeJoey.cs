using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

public class PlayerPrototypeJoey : KinematicBody2D
{
	// Player movement variables
    public static Vector2 velocity;
	public static float speedX = 300;
	public static float speedY;
    public static float climbSpeed = 300;
    public static bool onWall;
	public float gravity = 30;
	public float terminalVelocity = 1000;
	public Vector2 direction;
    
    // Power-up variables
    public bool[] powers = new bool[4];
    public static string currentPower;
    public static bool canClimb;
    public static bool powerClimb;
    public static bool canJump;
    public static bool powerJump;
    public static bool canGrapple;
    public static bool powerGrapple;
    public static bool canSmash;
    public static bool powerSmash;
    
    // Animation and state machine variables
    public AnimationTree animationTree;
    public CharacterStateMachine stateMachine;
    
    // UI variables
    PackedScene pauseMenu;
    public static bool isPaused;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        animationTree = this.GetNode<AnimationTree>("AnimationTree");
        animationTree.Active = true;
        
        stateMachine = this.GetNode<CharacterStateMachine>("CharacterStateMachine");
        
        pauseMenu = GD.Load<PackedScene>("res://UI/PauseMenuUI.tscn");
        isPaused = false;

        // Initializing powers array
        powers[0] = powerClimb;
        powers[1] = powerJump;
        powers[2] = powerGrapple;
        powers[3] = powerSmash;
	}

	public override void _PhysicsProcess(float delta)
	{        
        #region Movement
        MoveAndSlide(velocity, new Vector2(0,-1));

        #region  Horizontal movement
        direction.x = Input.GetAxis("move_left", "move_right");

        if(direction.x != 0 && stateMachine.CheckIfCanMove())
        {
            velocity.x = speedX * direction.x;
            onWall = false;
        }
        else velocity = velocity.MoveToward(new Vector2(0, velocity.y), speedX);
        #endregion

        #region Vertical Movement
        // Jumping
        if(powerJump) speedY = 1500;
        else speedY = 700;
        
        // Wall Climbing
        direction.y = Input.GetAxis("move_up", "move_down");

        if(direction.y != 0 && onWall && stateMachine.CheckIfCanMove())
        {
            velocity.y = climbSpeed * direction.y;
        }
        else if(direction.y == 0 && onWall)
        {
            velocity = velocity.MoveToward(new Vector2(velocity.x, 0), climbSpeed);
        }
        #endregion

        // Gravity
        if(!IsOnFloor() && !onWall) 
		{
			// Make the player fall faster in the air
            velocity.y += gravity;		
			if(velocity.y > terminalVelocity) velocity.y = terminalVelocity;
		}
        #endregion
        
        #region Power-ups
        // 'Q' key to cycle through available powers
        if(Input.IsActionJustPressed("power_toggle"))
        {
            // Temporary toggle for climb 
            if(!powerClimb)
            {
                powerClimb = true;
                currentPower = "Climb";
            }
            else if(powerClimb)
            {
                powerClimb = false;
            }
            
            // Powers cycle through climb > jump > grapple > smash
            // if(powerClimb)
            // {
                
            // }
            // if(canClimb)
        }
        
        
        #endregion
        
        #region Animations
        // Set blend position to play run and climb animations        
        if(onWall)
        {
            animationTree.Set("parameters/WallMove/blend_position", direction.y);
        }
        else if(!onWall)
        {
            animationTree.Set("parameters/GroundMove/blend_position", direction.x);
        }
        
        // Flip sprite based on direction of movement
        if(direction.x > 0) this.GetNode<Sprite>("Sprite").FlipH = false;
        else if(direction.x < 0) this.GetNode<Sprite>("Sprite").FlipH = true;
        #endregion
        
        #region UI
        // Pause the game if unpaused when the player hits Esc
        if(Input.IsActionJustPressed("ui_cancel") && !isPaused) PauseGame();
        #endregion
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
