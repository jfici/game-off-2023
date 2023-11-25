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
        if(!character.IsOnFloor() && !character.IsOnWall() && Player.coyoteTimer.IsStopped())
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
        // Jump if input is pressed while player is on the floor or coyote timer is running
        if(@event.IsActionPressed("jump"))
        {
            // if(!Player.coyoteTimer.IsStopped()) GD.Print("Jumped while coyote timer was running!");
            
            if(character.IsOnFloor() || !Player.coyoteTimer.IsStopped())
            {
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
