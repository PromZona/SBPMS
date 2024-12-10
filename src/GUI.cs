using Godot;

namespace ThesisGame;

public partial class GUI : GridContainer
{
    private Label _fpsLabel;
    private Label _memoryUsageLabel;
    private Label _frameTimeLabel;
    private Label _unitsCountLabel;
    
    public override void _Ready()
    {
        base._Ready();

        _fpsLabel = GetNode<Label>("FPSValue");
        _memoryUsageLabel = GetNode<Label>("MemoryUsageValue");
        _frameTimeLabel = GetNode<Label>("FrameTimeValue");
        _unitsCountLabel = GetNode<Label>("UnitsCountValue");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        
        _fpsLabel.Text = Engine.Singleton.GetFramesPerSecond().ToString();
        _memoryUsageLabel.Text =
            (Performance.Singleton.GetMonitor(Performance.Monitor.MemoryStatic) / (1024 * 1024)).ToString("F2");
        _frameTimeLabel.Text = Performance.Singleton.GetMonitor(Performance.Monitor.TimeProcess).ToString();
        _unitsCountLabel.Text = Game.Units.Count.ToString();
    }
}