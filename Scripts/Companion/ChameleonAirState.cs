using Godot;
using System;

public class ChameleonAirState : State
{
    [Export] public string landingAnimationName = "Landing Animation";
    [Export] public string wallAnimationName = "Wall Animation";
    [Export] public string jumpAnimationName = "Jump Animation";
    [Export] public string deathAnimationName = "Death Animation";
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        landingState = GetParent<Node>().GetNode<State>("LandingState");
        wallState = GetParent<Node>().GetNode<State>("WallState");
        deathState = GetParent<Node>().GetNode<State>("DeathState");
    }

    public override void StateProcess(float delta)
    {
        // Jump Buffer
        if(character.IsOnFloor() && !ChameleonCompanion.jumpBuffer.IsStopped())
        {
            // Jump again when touching the ground if jump buffer timer is running
            ChameleonCompanion.jumpBuffer.Stop();
            ChameleonCompanion.velocity.y = ChameleonCompanion.jumpSpeed;
            ChameleonCompanion.isJumping = true;
            playback.Start(jumpAnimationName);
            ChameleonCompanion.coyoteTimer.Stop();
        }
        else if(character.IsOnFloor())
        {
            nextState = landingState;
        }
        else if(ChameleonCompanion.onWall)
        {
            nextState = wallState;
        }
        
        // Kill the companion if they collided with an enemy/trap
        if(ChameleonCompanion.dying)
        {
            nextState = deathState;
            playback.Travel(deathAnimationName);
        }
    }
    
    public override void StateInput(InputEvent @event)
    {
        // Start timer for jump buffer if trying to jump while in the air
        if(@event.IsActionPressed("jump"))
        {
            ChameleonCompanion.jumpBuffer.Start();
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
