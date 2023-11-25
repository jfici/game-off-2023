using Godot;
using System;

public class PlayerAirState : State
{
    [Export] public string landingAnimationName = "Landing Animation";
    [Export] public string wallAnimationName = "Wall Animation";
    [Export] public string jumpAnimationName = "Jump Animation";
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        landingState = this.GetParent<Node>().GetNode<State>("LandingState");
        wallState = this.GetParent<Node>().GetNode<State>("WallState");
    }

    public override void StateProcess(float delta)
    {
        // Jump Buffer
        if(character.IsOnFloor() && !Player.jumpBuffer.IsStopped())
        {
            Player.jumpBuffer.Stop();
            Player.velocity = new Vector2(0, -Player.jumpSpeed);
            Player.isJumping = true;
            playback.Start(jumpAnimationName);
            Player.coyoteTimer.Stop();
        }
        else if(character.IsOnFloor())
        {
            nextState = landingState;
        }
        else if(Player.onWall)
        {
            nextState = wallState;
        }
    }
    
    public override void StateInput(InputEvent @event)
    {
        // Start timer for jump buffer if trying to jump while in the air
        if(@event.IsActionPressed("jump"))
        {
            Player.jumpBuffer.Start();
        }
    }
    
    public override void OnExit()
    {
        if(nextState == landingState)
        {
            playback.Travel(landingAnimationName);
        }
        else if(nextState == wallState)
        {
            playback.Travel(wallAnimationName);
        }
    }
}
