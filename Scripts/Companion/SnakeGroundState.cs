using Godot;
using System;

public class SnakeGroundState : State
{ 
    [Export] public string jumpAnimationName = "Jump Animation";
    [Export] public string deathAnimationName = "Death Animation";
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        airState = GetParent<Node>().GetNode<State>("AirState");
        deathState = GetParent<Node>().GetNode<State>("DeathState");
    }
    
    public override void StateProcess(float delta)
    {
        // Companion jumps after a delay from when the player jumped
        if(SnakeCompanion.companionJump)
        {
            if(character.IsOnFloor())
			{
                SnakeCompanion.velocity.y = SnakeCompanion.jumpSpeed;
                SnakeCompanion.isJumping = true;
                SnakeCompanion.companionJump = false;
                nextState = airState;
                playback.Travel(jumpAnimationName);
                SnakeCompanion.coyoteTimer.Stop();
			}
        }
        
        if(!character.IsOnFloor() && SnakeCompanion.coyoteTimer.IsStopped())
        {
            nextState = airState;
        }
        
        // Kill the companion if they collided with an enemy/trap
        if(SnakeCompanion.dying)
        {
            nextState = deathState;
            playback.Travel(deathAnimationName);
        }
    }
}
