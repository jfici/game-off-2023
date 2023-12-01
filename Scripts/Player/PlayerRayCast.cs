using Godot;
using System;

public class PlayerRayCast : Godot.RayCast2D
{
	public RayCast2D rayLeft;
	public RayCast2D rayRight;
	public RayCast2D rayUpL;
	public RayCast2D rayUpR;
	bool playWallGrab = true;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rayLeft = GetParent().GetNode<RayCast2D>("RayCast2D_Left");
		rayRight = GetParent().GetNode<RayCast2D>("RayCast2D_Right");
		rayUpL = GetParent().GetNode<RayCast2D>("RayCast2D_UpL");
		rayUpR = GetParent().GetNode<RayCast2D>("RayCast2D_UpR");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
		if (Player.powerClimb)
		{
			if (rayLeft.IsColliding() || rayRight.IsColliding())
			{
				Player.onWall = true;
				if (playWallGrab == true)
				{
					GetNode<AudioStreamPlayer>("../../Audio/Player_WallGrab").Play();
					playWallGrab = false;
				}

				if (Input.IsActionJustPressed("jump") || Input.IsActionJustPressed("move_right") || Input.IsActionJustPressed("move_left"))
				{
					playWallGrab = true;
					Player.onWall = false;
				}
			}
			else
			{
				Player.onWall = false;
				playWallGrab = true;
			}
		}

		if (rayUpL.IsColliding() || rayUpR.IsColliding())
		{
			// Stop the player from ascending if they bonk on collision above them
			Player.velocity.y += 50;
		}
	}
}
