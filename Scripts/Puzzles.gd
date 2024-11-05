extends Node
class_name Puzzles

func _ready() -> void:
	pass # Replace with function body.

func get_random_fen():
	var sqlquery = "SELECT * FROM puzzles ORDER BY RANDOM() LIMIT 1;"
	return SqlController.db_query(sqlquery)
