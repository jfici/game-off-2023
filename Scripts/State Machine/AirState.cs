using Godot;
using System;

public class AirState : State
{
    [Export] public String landingAnimationName = "Landing Animation";
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        groundState = this.GetParent<Node>().GetNode<State>("GroundState");
        landingState = this.GetParent<Node>().GetNode<State>("LandingState");
    }

    public override void StateProcess(float delta)
    {
        if(character.IsOnFloor())
        {
            nextState = landingState;
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
