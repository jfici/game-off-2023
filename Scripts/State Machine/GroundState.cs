using Godot;
using System;

public class GroundState : State
{ 
    [Export] public String jumpAnimationName = "Jump Animation";
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        airState = this.GetParent<Node>().GetNode<State>("AirState");
    }
    
    public override void StateProcess(float delta)
    {
        if(!character.IsOnFloor())
        {
            nextState = airState;
        }
    }
    
    public override void StateInput(InputEvent @event)
    {
        if(@event.IsActionPressed("jump"))
        {
            Jump();
        }
    }
    
    public void Jump()
    {
        PlayerPrototypeJoey.velocity.y = -PlayerPrototypeJoey.speedY;
        nextState = airState;
        playback.Travel(jumpAnimationName);
    }
}
