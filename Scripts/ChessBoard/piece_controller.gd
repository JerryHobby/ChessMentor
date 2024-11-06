extends Node2D

const PIECE_SCENES = {
	'P': preload("res://Scenes/Pieces/white_pawn.tscn"),
	'R': preload("res://Scenes/Pieces/white_rook.tscn"),
	'N': preload("res://Scenes/Pieces/white_knight.tscn"),
	'B': preload("res://Scenes/Pieces/white_bishop.tscn"),
	'Q': preload("res://Scenes/Pieces/white_queen.tscn"),
	'K': preload("res://Scenes/Pieces/white_king.tscn"),
	'p': preload("res://Scenes/Pieces/black_pawn.tscn"),
	'r': preload("res://Scenes/Pieces/black_rook.tscn"),
	'n': preload("res://Scenes/Pieces/black_knight.tscn"),
	'b': preload("res://Scenes/Pieces/black_bishop.tscn"),
	'q': preload("res://Scenes/Pieces/black_queen.tscn"),
	'k': preload("res://Scenes/Pieces/black_king.tscn")
}

@onready var square_1: TextureRect = $"../Board/Square1"

var board = []
var piece_offset

func _ready():
	piece_offset = square_1.size / 3

	# Example FEN string for initial position
	var fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
	setup_board_from_fen(fen)

func setup_board_from_fen(fen: String):
	clear_board()
	var rows = fen.split(" ")[0].split("/")
	for row_index in range(8):
		var row = rows[row_index]
		var col_index = 0
		for ch in row:
			if ch in "12345678":
				col_index += int(ch)
			else:
				place_piece(ch, row_index, col_index)
				col_index += 1

func clear_board():
	for piece in board:
		piece.queue_free()
	board.clear()

func place_piece(piece_char: String, row: int, col: int):
	var piece_scene = PIECE_SCENES.get(piece_char)
	
	if piece_scene:
		var piece_instance = piece_scene.instantiate()
		piece_instance.size = Vector2(GameManager.piece_size, GameManager.piece_size)
		piece_instance.scale = Vector2(0.8, 0.8)
		piece_instance.mouse_default_cursor_shape = 2
		add_child(piece_instance)
		piece_instance.position = \
		piece_offset + Vector2(col * piece_instance.size.x, row * piece_instance.size.y)
		board.append(piece_instance)

func move_piece(piece_instance, new_row: int, new_col: int):
	piece_instance.position = piece_offset + Vector2(new_col * piece_instance.size.x, new_row * piece_instance.size.y)

func remove_piece(piece_instance):
	piece_instance.queue_free()

func piece_dropped(piece_instance, new_position: Vector2):
	var target_offset = piece_offset + Vector2(0, -33)
	# Adjust the new position by subtracting the piece_offset
	var adjusted_position = new_position + target_offset
	
	# Calculate the new row and column based on the adjusted position
	var new_row = int(adjusted_position.y / piece_instance.size.y)
	var new_col = int(adjusted_position.x / piece_instance.size.x)
	
	# Move the piece to the new row and column
	move_piece(piece_instance, new_row, new_col)
	
	# Additional logic to handle game rules, captures, etc.
