using Godot;
using System;

public class PlayerGroundState : State
{ 
    [Export] public string jumpAnimationName = "Jump Animation";
    [Export] public string wallAnimationName = "Wall Animation";
    
    public KinematicBody2D player;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        airState = this.GetParent<Node>().GetNode<State>("AirState");
        wallState = this.GetParent<Node>().GetNode<State>("WallState");
        
        player = GetParent().GetParent<KinematicBody2D>();
    }
    
    public override void StateProcess(float delta)
    {
        if(!character.IsOnFloor() && !character.IsOnWall())
        {
            nextState = airState;
        }
        if(Player.onWall)
        {
            nextState = wallState;
            playback.Travel(wallAnimationName);
        }
    }
    
    public override void StateInput(InputEvent @event)
    {
        if(@event.IsActionPressed("jump"))
        {
            // Jump
            Player.velocity = new Vector2(0, -Player.jumpSpeed);
            Player.onWall = false;
            nextState = airState;
            playback.Travel(jumpAnimationName);
        }
    }
}
