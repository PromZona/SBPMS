using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;
using ThesisGame.Commands;
using ThesisGame.Controllers;
using ThesisGame.Model;

namespace ThesisGame.Controllers;

public class PerformanceBotController : IController
{
    private bool _isActivated = false;

    private Queue<Command> _commands = new();
    private Command _currentCommand = null;
    private SampleData _data = new();
    private bool _isCollecting = false;
    private const int WaitingAfterLoad = 3; // seconds
    private OutputMeasurements _output = null;

    struct SampleData
    {
        public List<double> FPS = new();
        public List<double> MemoryUsage = new();

        public SampleData()
        {
        }

        public void Clear()
        {
            FPS.Clear();
            MemoryUsage.Clear();
        }
    }

    public PerformanceBotController()
    {
    }

    public void Update(double delta)
    {
        if (!_isActivated)
            return;

        if (_commands.Count == 0)
            return;

        if (_isCollecting)
            CollectFramePerformance();

        _currentCommand ??= _commands.Dequeue();

        _currentCommand.Execute();
        if (_currentCommand.Result == Command.CommandResult.Succeed)
        {
            _currentCommand = null;
        }
    }

    public void StartCollection()
    {
        GD.Print("Performance Collection Starts");

        _isCollecting = true;
        _data.Clear();
    }

    public void StopCollection()
    {
        GD.Print("Performance Collection Ends");
        _isCollecting = false;

        Sample sample = new Sample()
        {
            LevelName = Game.LevelLoaderController.CurrentlyLoadedScene.Name,
            Position = "(0, 0, 0)",
            Measurements = new()
        };
        
        sample.Measurements.Add("FPS", _data.FPS.Average());
        sample.Measurements.Add("MemoryUsage", _data.MemoryUsage.Average());
        _output.Samples.Add(sample);
        
        GD.Print(_data.FPS.Average().ToString("F2"));
        GD.Print(_data.MemoryUsage.Average().ToString("F2"));
    }

    private void CollectFramePerformance()
    {
        double fps = Engine.Singleton.GetFramesPerSecond();
        double memoryUsage = Performance.Singleton.GetMonitor(Performance.Monitor.MemoryStatic) / (1024 * 1024);

        _data.FPS.Add(fps);
        _data.MemoryUsage.Add(memoryUsage);
    }

    public void SetScenario(string pathToScenario)
    {
        string scenarioText = File.ReadAllText(pathToScenario);
        InputScenario scenario = JsonSerializer.Deserialize<InputScenario>(scenarioText);
       
        GD.Print("Scenario read successfully");
        _commands.Clear();

        foreach (var scene in scenario.Scenarios)
        {
            _commands.Enqueue(new LoadLevelCommand($"res://levels/{scene.LevelName}.tscn"));
            _commands.Enqueue(new WaitCommand(WaitingAfterLoad));
            _commands.Enqueue(new StartMeasurementCommand());
            _commands.Enqueue(new WaitCommand(scene.CollectingTime));
            _commands.Enqueue(new StopMeasurementCommand());
        }
        
        _commands.Enqueue(new SaveMeasurementsCommand());
        _commands.Enqueue(new CloseGameCommand());
        GD.Print("Measurement Bot Commands Created");
        
        _isActivated = true;
        _output = new OutputMeasurements()
        {
            Build = "Game_v1.0",
            Platform = OS.GetName(),
            Timestamp = Time.GetDatetimeStringFromSystem(),
            Samples = new List<Sample>()
        };
    }

    public void SaveMeasurements()
    {
        string outputJson = JsonSerializer.Serialize<OutputMeasurements>(_output);
        File.WriteAllText($"measurements_{Time.GetUnixTimeFromSystem()}.json", outputJson);
    }
}