extends Node
var database: SQLite

var db_path : String = "res://Assets/DB/lichess_db.db"

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	database = SQLite.new()
	db_connect(db_path)

func db_connect(database_path : String):
	database.path = database_path
	database.open_db()
	
func db_query(query_string : String):
	database.query(query_string)
	return database.query_result
