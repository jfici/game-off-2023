using Godot;
using System;

public class ChameleonCompanion : KinematicBody2D
{
    // Companion follow variables
    [Export] public float followingDistance = 70;
	[Export] public float jumpDelay = 0.15f;
    public static float timeToJump = 0f;
    public static bool companionJump;
    
    // Companion movement variables
    public Vector2 direction;
    public static Vector2 velocity;
	public static float runSpeed = 350;
	public static float jumpSpeed = -750;
    public static float wallJumpSpeed = 1000;
    public static float climbSpeed = 300;
	public float gravity = 1600;
	public float terminalVelocity = 1000;
    public static bool isJumping;
    public static bool onWall;
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
	}

	public override void _PhysicsProcess(float delta)
	{              
        #region Movement
        direction.x = Input.GetAxis("move_left", "move_right");
        direction.y = Input.GetAxis("move_up", "move_down");
            
        // Horizontal Movement
        // direction.x = GetMoveDirectionX();
        // direction.y = GetMoveDirectionY();

		if(direction.x != 0 && stateMachine.CheckIfCanMove())
		{
			velocity.x = runSpeed * direction.x;
			onWall = false;
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
		else if (Input.IsActionJustPressed("jump") && (IsOnFloor() || onWall))
		{
			timeToJump = jumpDelay;
		}
		
		if(timeToJump < 0f)
		{
			companionJump = true;
            timeToJump = 0f;
        }
        
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
        if(direction.x > 0) GetNode<Sprite>("Sprite").FlipH = true;
        else if(direction.x < 0) GetNode<Sprite>("Sprite").FlipH = false;
        #endregion
	}
    
    protected float GetMoveDirectionX()
    {
		Vector2 relativePosition = GetParent().GetNode<Node2D>("Player").Position - Position;
		if (Mathf.Abs(relativePosition.x) > followingDistance)
		{
			return relativePosition.x > 0 ? 1 : -1;
		}
        return 0;
	}
    
    protected float GetMoveDirectionY()
    {
        Vector2 relativePosition = GetParent().GetNode<Node2D>("Player").Position - Position;
        if (Mathf.Abs(relativePosition.y) > followingDistance)
		{
			return relativePosition.y > 0 ? 1 : -1;
		}
        return 0;
    }
}
