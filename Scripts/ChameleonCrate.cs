using Godot;
using System;
using System.Linq;

public class ChameleonCrate : Area2D
{
    [Export] PackedScene chameleon;
    public TileMap gateTile;
    public TileMap valveTile;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gateTile = GetParent().GetNode<TileMap>("GateTileMap");
        valveTile = GetParent().GetNode<TileMap>("ValveTileMap");
        GD.Print("Gate has collision layer: " + gateTile.CollisionLayer);
        GD.Print("Valve has collision layer: " + valveTile.CollisionMask);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }

    public void _on_ChameleonCrate_body_entered(KinematicBody2D body)
    {
        if(body.CollisionLayer == 2)
        {
            // Delete the container
            QueueFree();
            
            // Stop the player from backtracking and open the exit
            gateTile.Hide();
            gateTile.CollisionLayer = 0;
            gateTile.CollisionMask = 0;
            valveTile.Show();
            valveTile.CollisionLayer = 1;
            valveTile.CollisionMask = 70;
            GD.Print("Gate has collision layer: " + gateTile.CollisionLayer);
            GD.Print("Valve has collision layer: " + valveTile.CollisionMask);

            // Unlock the power tied to the animal in the container
            Player.unlockClimb = true;

            // Spawn a chameleon companion
            var follower = chameleon.Instance();
            var followerPos = new Vector2(Position.x - 100, Position.y);
            GetParent().CallDeferred("add_child", follower);
            follower.Set("position", followerPos);

            // Or spawn three chameleon companions
            // for (int i = 1; i < 4; i++)
            // {
            //     var follower = chameleon.Instance();
            //     var followerPos = new Vector2(Position.x - (100 * i), Position.y);
            //     GetParent().CallDeferred("add_child", follower);
            //     follower.Set("position", followerPos);
            // }
        }
    }
}
