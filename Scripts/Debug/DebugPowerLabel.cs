using Godot;
using System;

public class DebugPowerLabel : Label
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(PlayerPrototypeJoey.powerClimb)
        {
            Text = "Power: Climb";
        }
        else if(PlayerPrototypeJoey.powerJump)
        {
            Text = "Power: Jump";
        }
        else if(PlayerPrototypeJoey.powerGrapple)
        {
            Text = "Power: Grapple";
        }
        else if(PlayerPrototypeJoey.powerSmash)
        {
            Text = "Power: Smash";
        }
        else Text = "Power: None";
    }
}
