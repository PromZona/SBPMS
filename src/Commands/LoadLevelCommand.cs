using System;
using System.Collections.Generic;
using Godot;
using ThesisGame.Controllers;

namespace ThesisGame.Commands;

public class LoadLevelCommand : Command
{
    private String _levelName;

    public LoadLevelCommand(String levelName)
    {
        _levelName = levelName;
    }
    
    public override void Execute()
    {
        Game.LevelLoaderController.LoadLevel(GD.Load<PackedScene>(_levelName));
        Result = CommandResult.Succeed;
    }
}