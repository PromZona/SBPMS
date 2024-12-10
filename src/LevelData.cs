using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace ThesisGame;

public partial class LevelData : Node3D
{
    [Export]
    public int MaxUnitsToSpawn = 10;
    [Export]
    public float SpawnInterval = 1.0f;
    [Export]
    public int AmountUnitsToSpawn = 1;
    [Export]
    public PackedScene UnitToSpawn = GD.Load<PackedScene>("res://unit.tscn");

    public IEnumerable<Marker3D> GetSpawnPoints()
    {
        Node pointsRoot = GetNode("SpawnPoints");
        IEnumerable<Marker3D> points = pointsRoot.GetChildren().Select(x => x as Marker3D);
        return points;
    }

    public IEnumerable<Marker3D> GetFinishPoints()
    {
        Node pointsRoot = GetNode("FinishPoints");
        IEnumerable<Marker3D> points = pointsRoot.GetChildren().Select(x => x as Marker3D);
        return points;
    }
    
}