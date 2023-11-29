using Godot;
using System;

public class SnakeLandingState : State
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        groundState = GetParent<Node>().GetNode<State>("GroundState");
    }
    
    public void On_Animation_Tree_Animation_Finished(string anim_name)
    {
        nextState = groundState;
    }
}
