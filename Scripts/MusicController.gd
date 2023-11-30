extends Node

var music = load("res://Audio/A_Game Off Music.wav")

func _ready():
	pass

func play_music():
	$Music.stream = music
	$Music.play()
