extends GridContainer

@onready var square_1: TextureRect = $Square1

var square_size = GameManager.piece_size

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	# Add 64 TextureRect nodes for the squares
	for i in range(1,65):
		var node_name = "Square" + str(i)
		var node = get_node(node_name)
		node.custom_minimum_size = Vector2(square_size,square_size)
		
		var row = int((i - 1) / 8)
		var col = (i - 1) % 8
		
		# Set the color based on the row and column
		if (row % 2 == 0 and col % 2 == 0) or (row % 2 == 1 and col % 2 == 1):
			node.set_dark()
		else:
			node.set_light()
