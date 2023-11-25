using Godot;
using System;

public class DebugPowerLabel : Label
{
     // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Player.powerClimb)
        {
            Text = "Power: Climb";
        }
        else if(Player.powerJump)
        {
            Text = "Power: Jump";
        }
        else if(Player.powerGrapple)
        {
            Text = "Power: Grapple";
        }
        else if(Player.powerSmash)
        {
            Text = "Power: Smash";
        }
        else Text = "Power: None";
    }
}
