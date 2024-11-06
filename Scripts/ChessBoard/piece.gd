extends TextureRect

var dragging = false
var offset = Vector2()

func _ready():
	size = Vector2(GameManager.piece_size, GameManager.piece_size)
	set_process_input(true)

func _input(event):
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_LEFT:
			if event.pressed:
				if get_global_rect().has_point(event.position):
					dragging = true
					offset = event.position - global_position
					#get_tree().set_input_as_handled()
			else:
				if dragging:
					dragging = false
					# Snap to the nearest square
					var new_position = (global_position / size).floor() * size
					position = new_position
					# Notify the controller about the move
					get_parent().piece_dropped(self, new_position)
	elif event is InputEventMouseMotion and dragging:
		global_position = event.position - offset
