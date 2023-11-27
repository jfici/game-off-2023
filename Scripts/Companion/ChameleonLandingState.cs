using Godot;
using System;

public class ChameleonLandingState : State
{
    [Export] public string landingAnimationName = "Landing Animation";
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        groundState = this.GetParent<Node>().GetNode<State>("GroundState");
    }
    
    public void On_Animation_Tree_Animation_Finished(string anim_name)
    {
        nextState = groundState;
    }
}
