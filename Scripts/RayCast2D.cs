using Godot;
using System;

public class RayCast2D : Godot.RayCast2D
{
    public RayCast2D rayRight;
    public RayCast2D rayLeft;
    public RayCast2D rayUp;
    public RayCast2D rayUp2; 
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        rayRight = GetParent().GetNode<RayCast2D>("RayCast2D_Right");
        rayLeft = GetParent().GetNode<RayCast2D>("RayCast2D_Left");
        rayUp = GetParent().GetNode<RayCast2D>("RayCast2D_Up");
        rayUp2 = GetParent().GetNode<RayCast2D>("RayCast2D_Up2");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {                      
        if(PlayerPrototypeJoey.powerClimb)
        {
            if(rayRight.IsColliding() || rayLeft.IsColliding())
            {
                PlayerPrototypeJoey.onWall = true;

                if(Input.IsActionJustPressed("jump") || Input.IsActionJustPressed("move_right") || Input.IsActionJustPressed("move_left"))
                {
                    PlayerPrototypeJoey.onWall = false;
                }
            }
            else PlayerPrototypeJoey.onWall = false;
        }
        
        if(rayUp.IsColliding() || rayUp2.IsColliding())
        {
            // Stop the player from ascending if they bonk on collision above them
            PlayerPrototypeJoey.velocity.y += 50;
        }
    }
}
