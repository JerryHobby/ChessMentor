extends Node

var uci_engine = "assets/stockfish/stockfish-windows-x86-64-avx2.exe"
#var uci_engine = "C:/Users/jerry/AppData/Local/Programs/Python/Python312/python.exe"
var uci_stderr
var uci_pid
var pipe
var thread
var info
var output_text = ""


func _ready() -> void:
	if OS.get_name() == "Windows":
		info = OS.execute_with_pipe(uci_engine, [])
	else:
		pass
	pipe = info["stdio"]
	uci_pid = info["pid"]
	#print(info)
	thread = Thread.new()
	thread.start(_thread_func)
	get_window().close_requested.connect(clean_func)
	write("uci")
	write("isready")


func write(new_text: String):
	output_text = ""
	var cmd = new_text + "\n"
	var buffer = cmd.to_utf8_buffer()
	pipe.store_buffer(buffer)


func read() -> String:
	return output_text


func _exit_tree() -> void:
	print("Exiting")
	OS.kill(uci_pid)
	print("Running:", OS.is_process_running(uci_pid))


func _add_char(c):
	output_text += c


func _thread_func():
	# read stdin and add to TextEdit.
	while pipe.is_open() and pipe.get_error() == OK:
		#_add_char(char(pipe.get_8()))
		_add_char.call_deferred(char(pipe.get_8()))


func clean_func():
	# close pipe and cleanup.
	pipe.close()
	thread.wait_to_finish()
