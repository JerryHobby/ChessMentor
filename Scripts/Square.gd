extends TextureRect

const LIGHT_SQUARE = preload("res://Assets/chessThemes/board/set0/lightSquare.png")
const DARK_SQUARE = preload("res://Assets/chessThemes/board/set0/darkSquare.png")
@onready var shapes: Control = $Shapes

var dark = true;
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	texture = LIGHT_SQUARE
	
func set_dark():
	dark = true;
	texture = DARK_SQUARE

func set_light():
	texture = LIGHT_SQUARE


func _on_mouse_entered() -> void:
	shapes.show_shape(shapes.SHAPE.SQUARE, shapes.COLOR.WHITE)

func _on_mouse_exited() -> void:
	shapes.hide_shape()
