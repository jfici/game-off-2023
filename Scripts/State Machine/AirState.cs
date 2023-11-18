using Godot;
using System;

public class AirState : State
{
    [Export] public String landingAnimationName = "Landing Animation";
    [Export] public String wallAnimationName = "Wall Animation";
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        groundState = this.GetParent<Node>().GetNode<State>("GroundState");
        landingState = this.GetParent<Node>().GetNode<State>("LandingState");
        wallState = this.GetParent<Node>().GetNode<State>("WallState");
    }

    public override void StateProcess(float delta)
    {
        if(character.IsOnFloor())
        {
            nextState = landingState;
        }
        else if(PlayerPrototypeJoey.onWall)
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
