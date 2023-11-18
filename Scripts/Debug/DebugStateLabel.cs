using Godot;
using System;

public class DebugStateLabel : Label
{
    public CharacterStateMachine stateMachine;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        stateMachine = GetParent().GetNode<CharacterStateMachine>("CharacterStateMachine");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Text = "State: " + stateMachine.currentState.Name;
    }
}
