using Godot;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

public partial class UciEngine : Node
{
	[Signal]
	public delegate void NewUciTextEventHandler(string line);

	[Export] public string UciEnginePath = "assets/stockfish/stockfish-windows-x86-64-avx2.exe";
	private Process uciProcess;
	private StreamWriter uciInput;
	private StreamReader uciOutput;
	private Thread readThread;
	private string outputText = "";
	private const int BufferSize = 1024; // Read larger chunks of data
	private bool stopThread = false; // Flag to signal the thread to stop
	private CancellationTokenSource cancellationTokenSource;
	private CancellationToken cancellationToken;

	public override void _Ready()
	{
		StartUciEngine();
		Write("uci");
		Write("isready");
	}

	private void StartUciEngine()
	{
		uciProcess = new Process();
		uciProcess.StartInfo.FileName = UciEnginePath;
		uciProcess.StartInfo.UseShellExecute = false;
		uciProcess.StartInfo.RedirectStandardInput = true;
		uciProcess.StartInfo.RedirectStandardOutput = true;
		uciProcess.StartInfo.CreateNoWindow = true;
		uciProcess.Start();

		uciInput = uciProcess.StandardInput;
		uciOutput = uciProcess.StandardOutput;

		readThread = new Thread(ReadOutput);
		readThread.Start();
	}

	public void Write(string newTextLine)
	{
		outputText = "";
		string cmd = newTextLine + "\n";
		EmitSignal(nameof(NewUciText), ">>> " + newTextLine);
		uciInput.WriteLine(cmd);
		uciInput.Flush();
	}

	public string Read()
	{
		return outputText;
	}

	private void ReadOutput()
	{
		char[] buffer = new char[BufferSize];
		while (!stopThread)
		{
			int read = uciOutput.Read(buffer, 0, BufferSize);
			if (read > 0)
			{
				string text = new string(buffer, 0, read);
				outputText += text;
				// Process complete lines
				string[] lines = outputText.Split('\n');
				for (int i = 0; i < lines.Length - 1; i++)
				{
					ProcessLine(lines[i]);
				}
				outputText = lines[lines.Length - 1]; // Keep the last partial line
			}
			else
			{
				Thread.Sleep(10); // Sleep for a short time to avoid busy-waiting
			}
		}
	}

	private void ProcessLine(string line)
	{
		GD.Print(line);
		// Use the intermediary method to emit the signal on the main thread
		CallDeferred(nameof(EmitNewUciText), line);
	}

	private void EmitNewUciText(string line)
	{
		EmitSignal(nameof(NewUciText), line);
	}

	public override void _ExitTree()
	{
		CleanUp();
	}

	private void CleanUp()
	{
		cancellationTokenSource.Cancel(); // Signal the thread to stop
		if (readThread != null && readThread.IsAlive)
		{
			readThread.Join(); // Wait for the thread to finish
		}
		if (uciProcess != null && !uciProcess.HasExited)
		{
			uciProcess.Kill();
		}
	}
}
