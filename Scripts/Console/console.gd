extends VBoxContainer

var console_text = ""
var puzzleDb
@onready var consoleNode: TextEdit = $Console
@onready var commandNode: LineEdit = $"HBoxContainer/UCI Command"


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	UciEngine.write("help")
	#get_tree().get_root().size_changed.connect(resize)

#func resize():
	#print("Screen resized")



func _process(_delta: float) -> void:
	#console_text = console_text + UciEngine.read()
	consoleNode.text = UciEngine.output_text
	consoleNode.scroll_vertical = consoleNode.get_v_scroll_bar().max_value


func _on_move_pressed() -> void:
	UciEngine.write("go movetime 1000")





func _on_submit_button_pressed() -> void:
	UciEngine.write(commandNode.text)
	print("Sending:", commandNode.text)
	commandNode.text = ""


func _on_uci_command_text_submitted(new_text: String) -> void:
	UciEngine.write(new_text)
	print("Sending:", new_text)
	commandNode.text = ""
