using Godot;
using System;

public class ChameleonGroundState : State
{ 
    [Export] public string jumpAnimationName = "Jump Animation";
    [Export] public string wallAnimationName = "Wall Animation";
    [Export] public string deathAnimationName = "Death Animation";
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        airState = this.GetParent<Node>().GetNode<State>("AirState");
        wallState = this.GetParent<Node>().GetNode<State>("WallState");
        deathState = this.GetParent<Node>().GetNode<State>("DeathState");
    }
    
    public override void StateProcess(float delta)
    {
        // Companion jumps after a delay from when the player jumped
        if(ChameleonCompanion.timeToJump < 0)
        {
            if(character.IsOnFloor())
			{
                ChameleonCompanion.velocity.y = ChameleonCompanion.jumpSpeed;
                ChameleonCompanion.isJumping = true;
                ChameleonCompanion.onWall = false;
                nextState = airState;
                playback.Travel(jumpAnimationName);
                ChameleonCompanion.coyoteTimer.Stop();
			}
        }
        
        
        if(!character.IsOnFloor() && !character.IsOnWall() && ChameleonCompanion.coyoteTimer.IsStopped())
        {
            nextState = airState;
        }
        
        if(ChameleonCompanion.onWall)
        {
            nextState = wallState;
            playback.Travel(wallAnimationName);
        }
        
        // Kill the companion if they collided with an enemy/trap
        if(ChameleonCompanion.dying)
        {
            nextState = deathState;
            playback.Travel(deathAnimationName);
        }
    }
}
