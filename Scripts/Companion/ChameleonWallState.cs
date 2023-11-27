using Godot;
using System;

public class ChameleonWallState : State
{
    [Export] public string jumpAnimationName = "Jump Animation";
    [Export] public string landingAnimationName = "Landing Animation";
    [Export] public string deathAnimationName = "Death Animation";
    public float jumpOffSpeed;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        airState = this.GetParent<Node>().GetNode<State>("AirState");
        landingState = this.GetParent<Node>().GetNode<State>("LandingState");
        deathState = this.GetParent<Node>().GetNode<State>("DeathState");
    }
    
    public override void StateProcess(float delta)
    {
        // Companion jumps after a delay from when the player jumped
        if(ChameleonCompanion.timeToJump < 0)
        {
            if(ChameleonCompanion.onWall)
			{
                // Wall jump a certain direction based on which side is on the wall
                if (character.GetNode<RayCast2D>("RayCasts/RayCast2D_Right").IsColliding())
                {
                    jumpOffSpeed = -ChameleonCompanion.wallJumpSpeed;
                }
                //else if (character.GetNode<Sprite>("Sprite").FlipH)
                if (character.GetNode<RayCast2D>("RayCasts/RayCast2D_Left").IsColliding())
                {
                    jumpOffSpeed = ChameleonCompanion.wallJumpSpeed;
                }

                // Wall jump
                ChameleonCompanion.velocity = new Vector2(jumpOffSpeed, ChameleonCompanion.jumpSpeed);
                ChameleonCompanion.isJumping = true;
                ChameleonCompanion.onWall = false;
                nextState = airState;
                playback.Travel(jumpAnimationName);
            }
        }
        
        if(!character.IsOnFloor() && !ChameleonCompanion.onWall)
        {
            nextState = airState;
            playback.Travel(jumpAnimationName);
        }
        else if(character.IsOnFloor() && !ChameleonCompanion.onWall)
        {
            nextState = landingState;
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
        if(@event.IsActionPressed("move_right") || @event.IsActionPressed("move_left"))
        {
            ChameleonCompanion.onWall = false;
            nextState = airState;
            playback.Travel(jumpAnimationName);
        }
    }
    
    public override void OnEnter()
    {
        ChameleonCompanion.velocity.y = 0;
    }
}
