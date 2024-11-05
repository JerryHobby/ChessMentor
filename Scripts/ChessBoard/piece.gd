extends TextureRect

enum PIECE {PAWN, ROOK, BISHOP, KNIGHT, QUEEN, KING}
enum COLOR {WHITE, BLACK}

var piece_size = GameManager.piece_size * 0.75

func _ready() -> void:
	setup(COLOR.WHITE, PIECE.PAWN)

func setup(pieceColor:COLOR, pieceShape:PIECE):
	var new_texture_path = "res://Assets/chessThemes/pieces/set3/"
	
	size = Vector2(piece_size, piece_size)
	
	match pieceColor:
		COLOR.WHITE:
			new_texture_path += "White"
		COLOR.BLACK:
			new_texture_path += "Black"

	match pieceShape:
		PIECE.PAWN:
			new_texture_path += "Pawn.png"
		PIECE.ROOK:
			new_texture_path += "Rook.png"
		PIECE.BISHOP:
			new_texture_path += "Bishop.png"
		PIECE.KNIGHT:
			new_texture_path += "Knight.png"
		PIECE.QUEEN:
			new_texture_path += "Queen.png"
		PIECE.KING:
			new_texture_path += "King.png"

	var new_texture = load(new_texture_path)
	if new_texture:
		texture = new_texture
	else:
		print("Failed to load piece texture: " + new_texture_path)
		return
