extends MenuBar
var puzzleDb = Puzzles.new()

enum GAMEMENU {
	NEWGAME, 
	NEWPUZZLE, 
	LOADFEN, 
	LOADPGN, 
	HINT
}

enum ACCTMENU {
	PROFILE = 0,
	GAMESETTINGS = 2,
	SIGNIN = 3
}

enum HELPMENU {
	ABOUT,
	SUPPORT
}

@onready var board = $"../MarginContainer/HBoxContainer/Board/MarginContainer/PieceController"

func _on_game_id_pressed(id: int) -> void:
	match id:
		GAMEMENU.NEWGAME:
			pass
		GAMEMENU.NEWPUZZLE:
			call_deferred("load_puzzle")
		GAMEMENU.LOADFEN:
			pass
		GAMEMENU.LOADPGN:
			pass
		GAMEMENU.HINT:
			hint()

func _on_account_id_pressed(id: int) -> void:
	match id:
		ACCTMENU.PROFILE:
			pass
		ACCTMENU.GAMESETTINGS:
			pass
		ACCTMENU.SIGNIN:
			pass


func _on_help_id_pressed(id: int) -> void:
	match id:
		HELPMENU.ABOUT:
			pass
		HELPMENU.SUPPORT:
			pass


func load_puzzle() -> void:
	# creating the timer forces async behaviors.
	await get_tree().create_timer(0.1).timeout  # Simulate async operation
	var fen = puzzleDb.get_random_fen()[0]["FEN"]
	var command = "position fen " + fen + " moves"
	
	board.setup_board_from_fen(fen)
	# load pieces onto the board to display the puzzle in the FEN
	UciEngine.write(command)
	UciEngine.write("d")

func hint() -> void:
	UciEngine.write("go movetime 1000")
