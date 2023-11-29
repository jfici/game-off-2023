using Godot;
using System;
using System.Linq;

public class CompanionCrate : Area2D
{
    [Export] PackedScene chameleon;
    [Export] PackedScene snake;
    public TileMap gateTile;
    public TileMap valveTile;
    public Node follower;
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gateTile = GetParent().GetNode<TileMap>("GateTileMap");
        valveTile = GetParent().GetNode<TileMap>("ValveTileMap");
    }

    public void _on_ChameleonCrate_body_entered(KinematicBody2D body)
    {
        if(body.CollisionLayer == 2)
        {
            // Delete the container
            QueueFree();
            
            // Initialize the companion that's in the crate and unlock its power
            follower = chameleon.Instance();
            Player.unlockClimb = true;
            
            GateValve();
            SpawnCompanion();
        }
    }
    
    public void _on_SnakeCrate_body_entered(KinematicBody2D body)
    {
        if(body.CollisionLayer == 2)
        {
            // Delete the container
            QueueFree();
            
            // Initialize the companion that's in the crate and unlock its power
            follower = snake.Instance();
            Player.unlockHighJump = true;
            
            GateValve();
            SpawnCompanion();
        }
    }
    
    
    private void GateValve()
    {
        // Stop the player from backtracking and open the exit
            gateTile.Hide();
            gateTile.CollisionLayer = 0;
            gateTile.CollisionMask = 0;
            valveTile.Show();
            valveTile.CollisionLayer = 1;
            valveTile.CollisionMask = 70;
    }
    
    private void SpawnCompanion()
    {
        // Spawn one companion
        var followerPos = new Vector2(Position.x - 100, Position.y);
        GetParent().CallDeferred("add_child", follower);
        follower.Set("position", followerPos);

        // Or spawn three companions
        // for (int i = 1; i < 4; i++)
        // {
        //     var followerPos = new Vector2(Position.x - (100 * i), Position.y);
        //     GetParent().CallDeferred("add_child", follower);
        //     follower.Set("position", followerPos);
        // }
    }
}
