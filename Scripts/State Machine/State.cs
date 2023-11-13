using Godot;
using System;

public class State : Node
{
    [Export] public bool canMove = true;
    public KinematicBody2D character;
    public AnimationNodeStateMachinePlayback playback;
    public State nextState;
    
    public State airState;
    public State groundState;
    public State landingState;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public virtual void StateProcess(float delta)
    {

    }
    
    public virtual void StateInput(InputEvent @event)
    {
        
    }
    
    public virtual void OnEnter()
    {
        
    }
    
    public virtual void OnExit()
    {
        
    }
}
