using Godot;
using System;

public class ChameleonRayCast : RayCast2D
{
    public RayCast2D rayLeft;
    public RayCast2D rayRight;
    public RayCast2D rayUpL;
    public RayCast2D rayUpR;

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
                if (rayLeft.IsColliding() || rayRight.IsColliding())
                {
                    ChameleonCompanion.onWall = true;

                    if (ChameleonCompanion.timeToJump < 0 || Input.IsActionJustPressed("move_right") || Input.IsActionJustPressed("move_left"))
                    {
                        ChameleonCompanion.onWall = false;
                    }
                }
                else ChameleonCompanion.onWall = false;

        if (rayUpL.IsColliding() || rayUpR.IsColliding())
        {
            // Stop the companion from ascending if they bonk on collision above them
            ChameleonCompanion.velocity.y += 50;
        }
    }
}