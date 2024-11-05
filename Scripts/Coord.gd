extends MarginContainer

@export var labelText:String
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	$Label.text = labelText
