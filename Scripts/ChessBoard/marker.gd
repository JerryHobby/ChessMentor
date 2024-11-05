extends Control

@onready var shape: TextureRect = $Shape

enum COLOR {WHITE, BLACK, RED, GREEN, YELLOW, BLUE}
enum SHAPE {CIRCLE, SQUARE, CROSS, DOT}
var square_size = GameManager.piece_size

func _ready() -> void:
	shape.size = Vector2(square_size,square_size)
	shape.hide()

func show_shape(shape_texture:SHAPE, shape_color:COLOR):
	var new_texture_path = "res://Assets/Shapes/"
	match shape_texture:
		SHAPE.SQUARE:
			new_texture_path += "Square"
		SHAPE.CIRCLE:
			new_texture_path += "Circle"
		SHAPE.CROSS:
			new_texture_path += "Cross"
		SHAPE.DOT:
			new_texture_path += "Dot"

	match shape_color:
		COLOR.WHITE:
			new_texture_path += "White.svg"
		COLOR.BLACK:
			new_texture_path += "Black.svg"
		COLOR.RED:
			new_texture_path += "Red.svg"
		COLOR.BLUE:
			new_texture_path += "Blue.svg"
		COLOR.GREEN:
			new_texture_path += "Green.svg"
		COLOR.YELLOW:
			new_texture_path += "Yellow.svg"

	var new_texture = load(new_texture_path)
	if new_texture:
		shape.texture = new_texture
		shape.show()
	else:
		print("Failed to load texture: " + new_texture_path)
		return

	shape.texture = new_texture
	shape.show()


func hide_shape():
	shape.hide()
