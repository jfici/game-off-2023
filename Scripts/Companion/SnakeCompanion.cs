using Godot;
using System;

public class SnakeCompanion : KinematicBody2D
{
	/// Companion follow variables
	[Export] public float jumpDelay = 0.15f;
	public static float timeToJump = 0f;
	public static bool companionJump;
	
	// Companion movement variables
	public Vector2 direction;
	public static Vector2 velocity;
	public static float runSpeed = 350;
	public static float jumpSpeed = -1.25f * 750;
	public float gravity = 1600;
	public float terminalVelocity = 1000;
	public static bool isJumping;
	public static bool wasOnFloor;
	public static Timer coyoteTimer;
	public static Timer jumpBuffer;
	
	// Animation and state machine variables
	public AnimationTree animationTree;
	public CharacterStateMachine stateMachine;
	public static bool dying;
	public static bool dead;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Assign variables to their respective child nodes
		animationTree = GetNode<AnimationTree>("AnimationTree");
		animationTree.Active = true;
		
		coyoteTimer = GetNode<Timer>("CoyoteTimer");
		jumpBuffer = GetNode<Timer>("JumpBuffer");
		
		stateMachine = GetNode<CharacterStateMachine>("CharacterStateMachine");

		GetNode<AudioStreamPlayer>("Audio/A_CrateOpen").Play();
	}

	public override void _PhysicsProcess(float delta)
	{              
		#region Movement
		direction.x = Input.GetAxis("move_left", "move_right");

		if(direction.x != 0 && stateMachine.CheckIfCanMove())
		{
			velocity.x = runSpeed * direction.x;
		}
		else velocity = velocity.MoveToward(new Vector2(0, velocity.y), runSpeed);
		
		// Vertical Movement
		// Jumping
		if(velocity.y >= 0) isJumping = false;
		
		
		// Countdown until companion jumps
		if (timeToJump > 0)
		{
			timeToJump -= delta;
		}
		else if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			timeToJump = jumpDelay;
		}
		
		if(timeToJump < 0f)
		{
			companionJump = true;
			timeToJump = 0f;
		}
		
		// Gravity
		if(!IsOnFloor() && coyoteTimer.IsStopped())
		{
			// Make the player fall faster in the air
			velocity.y += gravity * delta;	
			if(velocity.y > terminalVelocity) velocity.y = terminalVelocity;
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
		
		#region Animations
		// Set blend position to play run and climb animations        
		animationTree.Set("parameters/GroundMove/blend_position", direction.x);
		
		// Flip sprite based on direction of movement
		if(direction.x > 0) GetNode<Sprite>("Sprite").FlipH = false;
		else if(direction.x < 0) GetNode<Sprite>("Sprite").FlipH = true;
		#endregion
	}
}
