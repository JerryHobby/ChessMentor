# MenuBar.gd
extends MenuButton

func _ready():
	var menu = get_popup()
	menu.add_item("Play New Game", 0)
	menu.add_item("Load Previous Game", 1)
	menu.add_item("Upload PGN", 2)
	menu.add_item("Load Position (FEN)", 3)
	menu.add_item("Edit Profile", 4)
	menu.add_item("Preferences", 5)
	menu.add_item("Login / Logoff", 6)
	#menu.connect("id_pressed", self, "_on_menu_item_pressed")

func _on_menu_item_pressed(id):
	match id:
		0:
			# Handle Play New Game
			pass
		1:
			# Handle Load Previous Game
			pass
		2:
			# Handle Upload PGN
			pass
		3:
			# Handle Load Position (FEN)
			pass
		4:
			# Handle Edit Profile
			pass
		5:
			# Handle Preferences
			pass
		6:
			# Handle Login / Logoff
			pass
