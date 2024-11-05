extends MenuBar


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass


func _on_game_id_pressed(id: int) -> void:
	print("Game Menu item: ", id)


func _on_account_id_pressed(id: int) -> void:
	print("Account Menu item: ", id)


func _on_help_id_pressed(id: int) -> void:
	print("Help Menu item: ", id)
