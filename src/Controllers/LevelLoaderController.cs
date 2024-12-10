using System;
using Godot;

namespace ThesisGame.Controllers;

public partial class LevelLoaderController : Node, IController
{
    public LoadingStage LoadingStatus { get; private set; } = LoadingStage.NotLoading;

    public LevelData CurrentlyLoadedScene = null;
    public PackedScene RequestedScene = null;

    public enum LoadingStage
    {
        NotLoading,
        LoadRequested,
        WaitingEngine,
    }
    public void LoadLevel(PackedScene level)
    {
        RequestedScene = level;
        LoadingStatus = LoadingStage.LoadRequested;
    }

    public void Update(double delta)
    {
        if (LoadingStatus == LoadingStage.NotLoading)
            return;

        if (LoadingStatus == LoadingStage.LoadRequested)
        {
            RequestLoading();
            return;
        }

        if (LoadingStatus == LoadingStage.WaitingEngine)
        {
            SceneTree sceneTree = Game.Instance.GetTree();
            
            if (sceneTree?.CurrentScene == null)
                return;
            
            SetupScene(sceneTree);
            LoadingStatus = LoadingStage.NotLoading;
        }
    }

    private void RequestLoading()
    {
        SceneTree sceneTree = Game.Instance.GetTree();
        
        Error error = sceneTree.ChangeSceneToPacked(RequestedScene);

        if (error == Error.Ok)
        {
            GD.Print("Changing Scene...");
            LoadingStatus = LoadingStage.WaitingEngine;
        }
        else
        {
            GD.Print("Failed to change scene: Invalid Data");
            LoadingStatus = LoadingStage.NotLoading;
        }
    }

    private void SetupScene(SceneTree sceneTree)
    {
        Game.Instance.ClearUnits();
        
        LevelData levelData = sceneTree.CurrentScene as LevelData;
        CurrentlyLoadedScene = levelData;
        Game.SpawnController.Initialize(levelData);
        Game.MovementController.Initialize(levelData);
    }
}