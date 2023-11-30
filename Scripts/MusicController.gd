extends Node

var music = load("res://Audio/A_Game Off Music.wav")

func _ready():
	pass
	
func playMusic():
	$music.stream = music
	$music.play()
