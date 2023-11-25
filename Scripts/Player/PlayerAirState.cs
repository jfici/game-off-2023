using Godot;
using System;

public class PlayerAirState : State
{
    [Export] public string landingAnimationName = "Landing Animation";
    [Export] public string wallAnimationName = "Wall Animation";
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        landingState = this.GetParent<Node>().GetNode<State>("LandingState");
        wallState = this.GetParent<Node>().GetNode<State>("WallState");
    }

    public override void StateProcess(float delta)
    {
        if(character.IsOnFloor())
        {
            nextState = landingState;
        }
        else if(Player.onWall)
        {
            nextState = wallState;
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
