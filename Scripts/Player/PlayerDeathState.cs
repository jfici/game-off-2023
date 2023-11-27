using Godot;
using System;

public class PlayerDeathState : State
{
    public void On_Animation_Tree_Animation_Finished(string anim_name)
    {
        Player.dead = true;
        GD.Print("You died - game over!");
    }
}
