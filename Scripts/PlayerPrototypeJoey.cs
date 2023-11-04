using Godot;
using System;

public class PlayerPrototypeJoey : KinematicBody2D
{
	public Vector2 velocity;
	[Export] public float speedX = 300;
	[Export] public float speedY = 700;
	[Export] public float gravity = 30;
	public float terminalVelocity = 1000;
	public float directionX;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

//    // Called every frame. 'delta' is the elapsed time since the previous frame.
//    public override void _Process(float delta)
//    {
//
//    }

	public override void _PhysicsProcess(float delta)
	{
		// Horizontal movement
		directionX = Input.GetAxis("move_left", "move_right");
		velocity.x = speedX * directionX;
		MoveAndSlide(velocity, new Vector2(0,-1));
		
		// Veritcal movement and jumping
		if(Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.y = -speedY;
		}
		
		if(!IsOnFloor()) 
		{
			velocity.y += gravity;
			
			if(velocity.y > terminalVelocity) velocity.y = terminalVelocity;
		}
	}
}
