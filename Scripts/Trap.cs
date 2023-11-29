using Godot;
using System;

public class Trap : Area2D
{
    public void _on_Sawblade_body_entered(KinematicBody2D body)
    {
        if(body.CollisionLayer == 2)
        {
            Player.dying = true;
        }
        else if(body.CollisionLayer == 64 && body.Name == "ChameleonCompanion")
        {
            ChameleonCompanion.dying = true;
        }
        else if(body.CollisionLayer == 64 && body.Name == "SnakeCompanion")
        {
            SnakeCompanion.dying = true;
        }
    }
    
    public void _on_Spikes_body_entered(KinematicBody2D body)
    {
        if(body.CollisionLayer == 2)
        {
            Player.dying = true;
        }
        else if(body.CollisionLayer == 64 && body.Name == "ChameleonCompanion")
        {
            ChameleonCompanion.dying = true;
        }
        else if(body.CollisionLayer == 64 && body.Name == "SnakeCompanion")
        {
            SnakeCompanion.dying = true;
        }
    }
}
