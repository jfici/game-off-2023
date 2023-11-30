using Godot;
using System;

public class SnakeAirState : State
{
    [Export] public string landingAnimationName = "Landing Animation";
    [Export] public string jumpAnimationName = "Jump Animation";
    [Export] public string deathAnimationName = "Death Animation";
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        landingState = GetParent<Node>().GetNode<State>("LandingState");
        deathState = GetParent<Node>().GetNode<State>("DeathState");
    }

    public override void StateProcess(float delta)
    {        
        // Jump Buffer
        if(character.IsOnFloor() && !SnakeCompanion.jumpBuffer.IsStopped())
        {
            // Jump again when touching the ground if jump buffer timer is running
            SnakeCompanion.jumpBuffer.Stop();
            SnakeCompanion.velocity.y = SnakeCompanion.jumpSpeed;
            SnakeCompanion.isJumping = true;
            playback.Start(jumpAnimationName);
            SnakeCompanion.coyoteTimer.Stop();
        }
        else if(character.IsOnFloor())
        {
            nextState = landingState;
        }
        
        // Kill the companion if they collided with an enemy/trap
        if(SnakeCompanion.dying)
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
            SnakeCompanion.jumpBuffer.Start();
        }
    }
    
    public override void OnExit()
    {
        if(nextState == landingState)
        {
            playback.Travel(landingAnimationName);
        }
    }
}
