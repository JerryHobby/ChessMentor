# PlayerProfile.tscn
[gd_scene load_steps=2 format=2]

[node name="PlayerProfile" type="VBoxContainer"]
rect_min_size = Vector2(512, 768)

[node name="Name" type="Label" parent="PlayerProfile"]
text = "Name:"

[node name="Email" type="Label" parent="PlayerProfile"]
text = "Email:"

[node name="Rating" type="Label" parent="PlayerProfile"]
text = "Rating:"

[node name="PuzzlesPlayed" type="Label" parent="PlayerProfile"]
text = "Puzzles Played:"

[node name="GamesPlayed" type="Label" parent="PlayerProfile"]
text = "Games Played:"
