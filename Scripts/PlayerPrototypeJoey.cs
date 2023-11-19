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
	public static float speedX = 350;
	public static float speedY;
    public static float climbSpeed = 300;
    public static bool onWall;
	public float gravity = 30;
	public float terminalVelocity = 1000;
	public Vector2 direction;
    
    // Power-up variables
    // Change these from Export to static once we have something in-game to tie the unlocks to!!!
    [Export] public bool unlockClimb;
    [Export] public bool unlockJump;
    [Export] public bool unlockGrapple;
    [Export] public bool unlockSmash;
    public bool[] powers = new bool[4];
    public bool canClimb;
    public bool canJump;
    public bool canGrapple;
    public bool canSmash;
    public bool[] canUsePowers = new bool[4];
    public static bool powerClimb;
    
    public static bool powerJump;
    
    public static bool powerGrapple;
    
    public static bool powerSmash;
    public int powerIndex;
    
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

        UpdatePowers();
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
        if(powerJump) speedY = 1200;
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
        // Switch to a power when it becomes available for the first time
        #region Power Unlocks        
        if(unlockClimb)
        {
            powerClimb = true;
            powerJump = false;
            powerGrapple = false;
            powerSmash = false;
            powerIndex = 0;
            canClimb = true;
            UpdatePowerArrays();
            unlockClimb = false;
        }
        else if(unlockJump)
        {
            powerClimb = false;
            powerJump = true;
            powerGrapple = false;
            powerSmash = false;
            powerIndex = 1;
            canJump = true;
            UpdatePowerArrays();
            unlockJump = false;
        }
        else if(unlockGrapple)
        {
            powerClimb = false;
            powerJump = false;
            powerGrapple = true;
            powerSmash = false;
            powerIndex = 2;
            canGrapple = true;
            UpdatePowerArrays();
            unlockGrapple = false;
        }
        else if(unlockSmash)
        {
            powerClimb = false;
            powerJump = false;
            powerGrapple = false;
            powerSmash = true;
            powerIndex = 3;
            canSmash = true;
            UpdatePowerArrays();
            unlockSmash = false;
        }
        #endregion

        // 'Q' key to cycle through available powers
        // Powers cycle through climb > jump > grapple > smash
        if (Input.IsActionJustPressed("power_toggle"))
        {
            // Look for the next power that is available
            for (int i = 1; i < powers.GetLength(0); i++)
            {
                // Go back to start of the powers array before reaching the end
                if ((powerIndex + i) > 3)
                {
                    i -= 5;
                }
                else
                {
                    // Set power to the next one available
                    if (canUsePowers[powerIndex + i])
                    {
                        powers[powerIndex] = false;
                        powers[powerIndex + i] = true;
                        powerIndex += i;
                        UpdatePowers();
                        break;
                    }
                }
            }
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
    
    private void UpdatePowers()
    {
        // Set power bools equal to what's in the array
        powerClimb = powers[0];
        powerJump = powers[1];
        powerGrapple = powers[2];
        powerSmash = powers[3];
    }
    
    private void UpdatePowerArrays()
    {
        // Set power array elements equal to individual bools
        powers[0] = powerClimb;
        powers[1] = powerJump;
        powers[2] = powerGrapple;
        powers[3] = powerSmash;
        
        canUsePowers[0] = canClimb;
        canUsePowers[1] = canJump;
        canUsePowers[2] = canGrapple;
        canUsePowers[3] = canSmash;
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
