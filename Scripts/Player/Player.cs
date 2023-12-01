using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

public class Player : KinematicBody2D
{
	// Player movement variables
	public Vector2 direction;
	public static Vector2 velocity;
	public static float runSpeed = 350;
	public static float jumpSpeed;
	[Export] public float jumpForce = 750;
	public static float wallJumpSpeed = 1000;
	public static float climbSpeed = 300;
	public float gravity = 1600;
	public float terminalVelocity = 1000;
    public static bool isJumping;
    public static bool onWall;
    public static bool wasOnFloor;
    public static Timer coyoteTimer;
    public static Timer jumpBuffer;
    
    // Power-up variables
    public static bool unlockClimb;
    public static bool unlockHighJump;
    [Export] public bool unlockGrapple; //Change to static if something in-game can unlock them
    [Export] public bool unlockSmash; //Change to static if something in-game can unlock them
    public bool[] powers = new bool[4];
    public bool canClimb;
    public bool canHighJump;
    public bool canGrapple;
    public bool canSmash;
    public bool[] canUsePowers = new bool[4];
    public static bool powerClimb;
    public static bool powerHighJump;
    public static bool powerGrapple;
    public static bool powerSmash;
    public int powerIndex;
    
    // Animation and state machine variables
    public AnimationTree animationTree;
    public CharacterStateMachine stateMachine;
    public static bool dying;
    public static bool dead;
    
    // Save data and checkpoint variables
    public static Vector2 checkpointPos = new Vector2(32, -32);
    public static bool checkpointSet;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Assign variables to their respective child nodes
		animationTree = GetNode<AnimationTree>("AnimationTree");
		animationTree.Active = true;
		
		coyoteTimer = GetNode<Timer>("CoyoteTimer");
		jumpBuffer = GetNode<Timer>("JumpBuffer");
		
		stateMachine = GetNode<CharacterStateMachine>("CharacterStateMachine");
		
		// Spawn the player at the checkpoint
		Position = checkpointPos;
		GetNode<AudioStreamPlayer>("Audio/Player_Jump").Stream = (AudioStream)ResourceLoader.Load("res://Audio/A_Jump.wav");
	}

	public override void _PhysicsProcess(float delta)
	{              
		#region Movement
		direction.x = Input.GetAxis("move_left", "move_right");
		direction.y = Input.GetAxis("move_up", "move_down");
		
		// Horizontal Movement
		if(direction.x != 0 && stateMachine.CheckIfCanMove())
		{
			velocity.x = runSpeed * direction.x;
			onWall = false;
		}
		else velocity = velocity.MoveToward(new Vector2(0, velocity.y), runSpeed);
		
		// Vertical Movement
		// Jumping
		if(powerHighJump) jumpSpeed = -1.25f * jumpForce;
		else jumpSpeed = -jumpForce;
		
		if(velocity.y >= 0) isJumping = false;
		
		// Gravity
		if(!IsOnFloor() && !onWall && coyoteTimer.IsStopped())
		{
			// Make the player fall faster in the air
			velocity.y += gravity * delta;	
			if(velocity.y > terminalVelocity) velocity.y = terminalVelocity;
		}
		
		// Wall Climbing
		if(direction.y != 0 && onWall && stateMachine.CheckIfCanMove())
		{
			velocity.y = climbSpeed * direction.y;
		}
		else if(direction.y == 0 && onWall)
		{
			velocity = velocity.MoveToward(new Vector2(velocity.x, 0), climbSpeed);
		}
		
		// Putting MoveAndSlide() so it's after all movement but before coyote time logic
		wasOnFloor = IsOnFloor();
		MoveAndSlide(velocity, new Vector2(0, -1));
		
		// Coyote time
		if(!IsOnFloor() && wasOnFloor && !isJumping)
		{
			velocity.y = 0;
			coyoteTimer.Start();
		}
		#endregion

		#region Power-ups
		// Switch to a power when it becomes available for the first time
		#region Power Unlocks        
		if(unlockClimb)
		{
			powerClimb = true;
			powerHighJump = false;
			powerGrapple = false;
			powerSmash = false;
			powerIndex = 0;
			canClimb = true;
			UpdatePowerArrays();
			unlockClimb = false;
		}
		else if(unlockHighJump)
		{
			powerClimb = false;
			powerHighJump = true;
			powerGrapple = false;
			powerSmash = false;
			powerIndex = 1;
			canHighJump = true;
			UpdatePowerArrays();
			unlockHighJump = false;
			
			GetNode<AudioStreamPlayer>("Audio/Player_Jump").Stream = (AudioStream)ResourceLoader.Load("res://Audio/A_SuperJump.wav");
		}
		else if(unlockGrapple)
		{
			powerClimb = false;
			powerHighJump = false;
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
			powerHighJump = false;
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
	}
	
	private void UpdatePowers()
	{
		// Set power bools equal to what's in the array
		powerClimb = powers[0];
		powerHighJump = powers[1];
		powerGrapple = powers[2];
		powerSmash = powers[3];
	}
	
	private void UpdatePowerArrays()
	{
		// Set power array elements equal to individual bools
		powers[0] = powerClimb;
		powers[1] = powerHighJump;
		powers[2] = powerGrapple;
		powers[3] = powerSmash;
		
		canUsePowers[0] = canClimb;
		canUsePowers[1] = canHighJump;
		canUsePowers[2] = canGrapple;
		canUsePowers[3] = canSmash;
	}
}
