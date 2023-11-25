using Godot;
using System;

public class PlayerWallState : State
{
    [Export] public string jumpAnimationName = "Jump Animation";
    [Export] public string landingAnimationName = "Landing Animation";
    public float wallJumpSpeed = 1000;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        airState = this.GetParent<Node>().GetNode<State>("AirState");
        landingState = this.GetParent<Node>().GetNode<State>("LandingState");
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
    }
    
    public override void StateInput(InputEvent @event)
    {
        if(@event.IsActionPressed("jump"))
        {
            // Wall jump a certain direction based on where the sprite is facing
            if (!character.GetNode<Sprite>("Sprite").FlipH)
            {
                wallJumpSpeed = -wallJumpSpeed;
            }
            else if (character.GetNode<Sprite>("Sprite").FlipH)
            {
                wallJumpSpeed = Math.Abs(wallJumpSpeed);
            }
            
            // Wall jump
            Player.velocity = new Vector2(wallJumpSpeed, -Player.jumpSpeed);
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
