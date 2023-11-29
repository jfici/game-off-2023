using Godot;
using System;

public class SnakeDeathState : State
{
    public void On_Animation_Tree_Animation_Finished(string anim_name)
    {
        SnakeCompanion.dead = true;
    }
}
