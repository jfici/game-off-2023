using Godot;
using System;

public class State : Node
{
    [Export] public bool canMove;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        canMove = true;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
