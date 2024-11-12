extends VBoxContainer

var console_text = ""
var puzzleDb
@onready var consoleNode: TextEdit = $Console
@onready var commandNode: LineEdit = $"HBoxContainer/UCI Command"
const MAX_CONSOLE_SIZE = 50_000

# Reference to the UciEngine instance
@onready var uciEngine: UciEngine = get_node("/root/UciEngine")

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	uciEngine.NewUciText.connect(_on_new_text)
	
	#.connect("NewTextEventHandler", _on_new_text)

	uciEngine.Write("help")

	#get_tree().get_root().size_changed.connect(resize)

#func resize():
	#print("Screen resized")


#func _process(_delta: float) -> void:
	##console_text = console_text + UciEngine.read()
	#consoleNode.text = UciEngine.output_text
	#consoleNode.scroll_vertical = consoleNode.get_v_scroll_bar().max_value


func _on_move_pressed() -> void:
	uciEngine.Write("go movetime 1000")


func _on_new_text(line: String):
	consoleNode.text += line + "\n"
	consoleNode.text = consoleNode.text.right(MAX_CONSOLE_SIZE)
	consoleNode.scroll_vertical = consoleNode.get_v_scroll_bar().max_value

func _on_submit_button_pressed() -> void:
	uciEngine.Write(commandNode.text)
	print("Sending:", commandNode.text)
	commandNode.text = ""


func _on_uci_command_text_submitted(new_text: String) -> void:
	_on_submit_button_pressed()
