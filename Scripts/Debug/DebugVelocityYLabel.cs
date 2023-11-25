using Godot;
using System;

public class DebugVelocityYLabel : Label
{
 // Called every frame. 'delta' is the elapsed time since the previous frame.
 public override void _PhysicsProcess(float delta)
 {
     Text = "Velocity Y: " + Player.velocity.y;
 }
}
