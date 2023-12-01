using Godot;
using System;

public class PlayerGroundState : State
{ 
	[Export] public string jumpAnimationName = "Jump Animation";
	[Export] public string wallAnimationName = "Wall Animation";
	[Export] public string deathAnimationName = "Death Animation";
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		airState = this.GetParent<Node>().GetNode<State>("AirState");
		wallState = this.GetParent<Node>().GetNode<State>("WallState");
		deathState = this.GetParent<Node>().GetNode<State>("DeathState");
	}
	
	public override void StateProcess(float delta)
	{
		if(!character.IsOnFloor() && !character.IsOnWall() && Player.coyoteTimer.IsStopped())
		{
			nextState = airState;
		}
		
		if(Player.onWall)
		{
			nextState = wallState;
			playback.Travel(wallAnimationName);
		}
		
		// Kill the player if they collided with an enemy/trap
		if(Player.dying)
		{
			nextState = deathState;
			playback.Travel(deathAnimationName);
		}
	}
	
	public override void StateInput(InputEvent @event)
	{
		// Jump if input is pressed while player is on the floor or coyote timer is running
		if(@event.IsActionPressed("jump"))
		{
			if(character.IsOnFloor() || !Player.coyoteTimer.IsStopped())
			{
				GetNode<AudioStreamPlayer>("../../Audio/Player_Jump").Play();
				Player.velocity.y = Player.jumpSpeed;
				Player.isJumping = true;
				Player.onWall = false;
				nextState = airState;
				playback.Travel(jumpAnimationName);
				Player.coyoteTimer.Stop();
			}
			else
			{
				GD.Print("Can't jump off ground because IsOnFloor() is " + character.IsOnFloor() + " at this point of the current frame!");
			}
		}
	}
}
