using Godot;
using System;

public class PlayerWallState : State
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
        if(!character.IsOnFloor() && !Player.onWall)
        {
            nextState = airState;
            playback.Travel(jumpAnimationName);
        }
        else if(character.IsOnFloor() && !Player.onWall)
        {
            nextState = landingState;
        }
        
        // Kill the player if they collided with an enemy/trap
        if(Player.dying)
        {
            nextState = deathState;
            playback.Travel(deathAnimationName);
        }
    }
    
    public override void StateInput(InputEvent @event)
    {
        if(@event.IsActionPressed("jump"))
        {
            // Wall jump a certain direction based on which side is on the wall
            if(character.GetNode<RayCast2D>("RayCasts/RayCast2D_Right").IsColliding())
            {
                jumpOffSpeed = -Player.wallJumpSpeed;
            }
            //else if (character.GetNode<Sprite>("Sprite").FlipH)
            if(character.GetNode<RayCast2D>("RayCasts/RayCast2D_Left").IsColliding())
            {
                jumpOffSpeed = Player.wallJumpSpeed;
            }
            
            // Wall jump
            Player.velocity = new Vector2(jumpOffSpeed, Player.jumpSpeed);
            Player.isJumping = true;
            Player.onWall = false;
            nextState = airState;
            playback.Travel(jumpAnimationName);
        }
        
        if(@event.IsActionPressed("move_right") || @event.IsActionPressed("move_left"))
        {
            Player.onWall = false;
            nextState = airState;
            playback.Travel(jumpAnimationName);
        }
    }
    
    public override void OnEnter()
    {
        Player.velocity.y = 0;
    }
}
