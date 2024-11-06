extends TextureRect

const LIGHT_SQUARE = preload("res://Assets/chessThemes/board/set0/lightSquare.png")
const DARK_SQUARE = preload("res://Assets/chessThemes/board/set0/darkSquare.png")
@onready var marker: Control = $Marker

var dark = true;
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	size = Vector2(GameManager.piece_size, GameManager.piece_size)
	texture = LIGHT_SQUARE
	
func set_dark():
	dark = true;
	texture = DARK_SQUARE

func set_light():
	texture = LIGHT_SQUARE


func _on_mouse_entered() -> void:
	#marker.show_shape(marker.SHAPE.CIRCLE, marker.COLOR.WHITE)
	pass

func _on_mouse_exited() -> void:
	marker.hide_shape()
