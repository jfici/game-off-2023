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
        landingState = this.GetParent<Node>().GetNode<State>("LandingState");
        wallState = this.GetParent<Node>().GetNode<State>("WallState");
        deathState = this.GetParent<Node>().GetNode<State>("DeathState");
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
        
        // Variable jump height
        if(@event.IsActionReleased("jump") && ChameleonCompanion.velocity.y < (0.5f * ChameleonCompanion.jumpSpeed))
        {
            // The companion jumps half as high if the player release the jump button early
            ChameleonCompanion.velocity.y = 0.5f * ChameleonCompanion.jumpSpeed;
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
