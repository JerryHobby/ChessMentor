extends Node

var uci_engine = "assets/stockfish/stockfish-windows-x86-64-avx2.exe"
var uci_stderr
var uci_pid
var pipe
var thread
var info
var output_text = ""
var buffer_size = 1024 # Read larger chunks of data

signal new_text(line: String)

func _ready() -> void:
	if OS.get_name() == "Windows":
		info = OS.execute_with_pipe(uci_engine, [])
	else:
		pass
	pipe = info["stdio"]
	uci_pid = info["pid"]
	thread = Thread.new()
	thread.start(_thread_func)
	get_window().close_requested.connect(clean_func)
	#get_tree().connect("tree_exit", clean_func)
	write("uci")
	write("isready")

func write(new_text_line: String):
	output_text = ""
	var cmd = new_text_line + "\n"
	var buffer = cmd.to_utf8_buffer()
	pipe.store_buffer(buffer)

func read() -> String:
	return output_text

func _thread_func():
	while true:
		var buffer = pipe.get_buffer(buffer_size)
		if buffer.size() > 0:
			var text = buffer.get_string_from_utf8()
			output_text += text
			# Process complete lines
			var lines = output_text.split("\n")
			for i in range(lines.size() - 1):
				process_line(lines[i])
			output_text = lines[lines.size() - 1] # Keep the last partial line
		else:
			OS.delay_msec(10) # Sleep for a short time to avoid busy-waiting

func process_line(line: String):
	# Handle the line of output from the UCI engine
	print(line)
	call_deferred("emit_signal", "new_text", line)
	# Update your text node or other UI elements here


func clean_func():
	#if thread.is_alive():
		#thread.wait_to_finish()
	OS.kill(uci_pid)
