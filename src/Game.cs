using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using ThesisGame.Commands;
using ThesisGame.Controllers;

namespace ThesisGame;

public partial class Game : Node
{
    private readonly List<IController> _controllers = new();

    public static readonly LevelLoaderController LevelLoaderController = new();
    public static readonly MovementController MovementController = new();
    public static readonly SpawnController SpawnController = new();
    public static readonly GameCommandController GameCommandController = new();
    public static readonly PerformanceBotController PerformanceBotController = new();

    public static List<Unit> Units { get; private set; } = new();
    
    private static Game _instance;

    public static Game Instance
    {
        get => _instance;
        private set => _instance = value;
    }

    public override void _Ready()
    {
        Instance = this;
        RegisterControllers();

        if (!OS.Singleton.HasFeature("editor"))
        {
            string[] args = OS.Singleton.GetCmdlineUserArgs();
            string scenarioPath = args
                .Where(x => x.Contains("scenario"))
                .Select(x => x.Split('=')[1])
                .FirstOrDefault();
            GD.Print($"Found scenario path: {scenarioPath}");
            PerformanceBotController.SetScenario(scenarioPath);
        }
        else
        {
            PerformanceBotController.SetScenario("PerfBot/Scenario.json");
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("LoadLevel1"))
        {
            GameCommandController.AddCommand(new LoadLevelCommand("res://levels/Level1.tscn"));
        }

        if (Input.IsActionJustPressed("LoadLevel2"))
        {
            GameCommandController.AddCommand(new LoadLevelCommand("res://levels/Level2.tscn"));
        }

        if (Input.IsActionJustPressed("LoadLevel3"))
        {
            GameCommandController.AddCommand(new LoadLevelCommand("res://levels/Level3.tscn"));
        }


        foreach (var controller in _controllers)
        {
            controller.Update(delta);
        }
    }

    private void RegisterControllers()
    {
        _controllers.Add(LevelLoaderController);
        _controllers.Add(SpawnController);
        _controllers.Add(MovementController);
        _controllers.Add(GameCommandController);
        _controllers.Add(PerformanceBotController);
    }

    public void AddUnit(Unit unit)
    {
        Units.Add(unit);
    }

    public void ClearUnits()
    {
        Units.Clear();
    }
}