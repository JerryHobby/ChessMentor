extends TextureRect

var dragging = false
var offset = Vector2()
#@onready var marker: TextureRect = $Marker

func _ready():
	size = Vector2(GameManager.piece_size, GameManager.piece_size)
	#marker.size = size
	set_process_input(true)
	mouse_filter = Control.MOUSE_FILTER_PASS # Allow mouse events to pass through


func _input(event):
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_LEFT:
			if event.pressed:
				if get_global_rect().has_point(event.position):
					dragging = true
					offset = event.position - global_position
					accept_event()
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
