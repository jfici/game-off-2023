using Godot;
using System;

public class PlayerDeathState : State
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Load game over UI
        //gameOverMenu = GD.Load<PackedScene>("res://UI/GameOverMenuUI.tscn");
    }

    public void On_Animation_Tree_Animation_Finished(string anim_name)
    {
        Player.dead = true;
        GD.Print("You died - game over!");
    }
}
