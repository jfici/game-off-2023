using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

public class CharacterStateMachine : Node
{    
    public State[] states;
    KinematicBody2D character;
    public State currentState;
    
    [Export] public AnimationTree animationTree;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        states = new State[this.GetChildCount()];
        
        // Get the character parent node
        character = GetParent<KinematicBody2D>();
        
        // Establish starting state
        if(character.IsOnFloor())
        {
            currentState = GetNode<State>("GroundState");
        }
        else
        {
            currentState = GetNode<State>("AirState");
        }
        
        // Assign the animation tree node
        animationTree = GetParent().GetNode<AnimationTree>("AnimationTree");
        
        // Change states
        foreach(State child in GetChildren())
        {
            if(child is State)
            {
                states.Append(child);
                
                // Assign the parent character for all the children states to use
                child.character = character;
                child.playback = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
            }
            else GD.Print("Child " + child.Name + " is not a State for CharacterStateMachine");
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        // Change state when next one is queued up
        if(currentState.nextState != null)
        {
            SwitchStates(currentState.nextState);
        }
        
        // Method that executes code based on the state you're in
        currentState.StateProcess(delta);
    }

    public bool CheckIfCanMove()
    {
        return currentState.canMove;
    }
    
    private void SwitchStates(State newState)
    {
        if(currentState != null)
        {
            currentState.OnExit();
            currentState.nextState = null;
        }
        
        currentState = newState;
        currentState.OnEnter();
    }
    
    public override void _Input(InputEvent @event)
    {
        currentState.StateInput(@event);
    }
}
