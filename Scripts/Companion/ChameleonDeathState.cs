using Godot;
using System;

public class ChameleonDeathState : State
{
    public void On_Animation_Tree_Animation_Finished(string anim_name)
    {
        ChameleonCompanion.dead = true;
    }
}
