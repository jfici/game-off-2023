using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

public class CharacterStateMachine : Node
{    
    public State[] states;
    [Export] State currentState;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        states = new State[this.GetChildCount()];
        
        // Start in GroundState
        currentState = this.GetNode<State>("GroundState");
        
        // Change states
        foreach(Node child in GetChildren())
        {
            if(child is State)
            {
                states.Append(child);
                // Setup states here
                
            }
            //else GD.Print("Child " + child.Name + " is not a State for CharacterStateMachine");
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public bool CheckIfCanMove()
    {
        return currentState.canMove;
    }
}
