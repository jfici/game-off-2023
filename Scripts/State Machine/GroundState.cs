using Godot;
using System;

public class GroundState : State
{ 
    [Export] public String jumpAnimationName = "Jump Animation";
    [Export] public String wallAnimationName = "Wall Animation";
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        airState = this.GetParent<Node>().GetNode<State>("AirState");
        wallState = this.GetParent<Node>().GetNode<State>("WallState");
    }
    
    public override void StateProcess(float delta)
    {
        if(!character.IsOnFloor() && !character.IsOnWall())
        {
            nextState = airState;
        }
        
        if(PlayerPrototypeJoey.onWall)
        {
            nextState = wallState;
            playback.Travel(wallAnimationName);
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
        PlayerPrototypeJoey.onWall = false;
        nextState = airState;
        playback.Travel(jumpAnimationName);
    }
}
