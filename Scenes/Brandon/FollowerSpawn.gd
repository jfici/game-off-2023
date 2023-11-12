extends Area2D

export var follower_scene: PackedScene

func _on_Area2D_area_shape_entered(area_rid, area, area_shape_index, local_shape_index):
	if get_rid() == area_rid:
		var follower = follower_scene.instance()
		follower.set_position(Vector2(0, 0))
		get_parent().add_child(follower)
		#queue_free()
