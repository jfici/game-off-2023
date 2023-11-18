using Godot;
using System;

public class LandingState : State
{
    [Export] public String landingAnimationName = "Landing Animation";
    
    public string animStarted;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        groundState = this.GetParent<Node>().GetNode<State>("GroundState");
    }
    
    public void On_Animation_Tree_Animation_Finished(String anim_name)
    {
        nextState = groundState;
    }
}
