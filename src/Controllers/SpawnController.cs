using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace ThesisGame.Controllers;

public class SpawnController : IController
{
    private List<Marker3D> _spawnPoints = new();
    private int _maxUnitsToSpawnCount = 0;
    private float _spawnInterval = 1.0f;
    private int _amountUnitsToSpawn = 0;
    private PackedScene _unitToSpawn = null;

    private readonly RandomNumberGenerator _random = new();
    
    
    private int _spawnedUnitsCount = 0;
    private ulong _lastTimeSpawned = 0;
    
    public void Update(double delta)
    {
        if (_spawnedUnitsCount >= _maxUnitsToSpawnCount)
            return;
        
        ulong currentTime = Time.Singleton.GetTicksMsec();
        if ((currentTime - _lastTimeSpawned) / 1000.0f < _spawnInterval)
            return;
        _lastTimeSpawned = currentTime;
        
        for (int i = 0; i < _amountUnitsToSpawn; i++)
        {
            if (_spawnedUnitsCount >= _maxUnitsToSpawnCount)
                break;

            int positionIndex = _random.RandiRange(0, _spawnPoints.Count - 1);
            Marker3D positionMarker = _spawnPoints[positionIndex];
            Node parentNode = Game.Instance.GetTree().CurrentScene.GetNode("Units");
            Unit unit = _unitToSpawn.Instantiate() as Unit;
            
            parentNode.AddChild(unit);
            unit!.GlobalPosition = positionMarker.GlobalPosition;
            
            Game.Instance.AddUnit(unit);
            _spawnedUnitsCount++;
        }
    }

    public void Initialize(LevelData level)
    {
        _spawnPoints.Clear();
        _spawnedUnitsCount = 0;
        _lastTimeSpawned = 0;

        _spawnPoints = level.GetSpawnPoints().ToList();
        _maxUnitsToSpawnCount = level.MaxUnitsToSpawn;
        _spawnInterval = level.SpawnInterval;
        _amountUnitsToSpawn = level.AmountUnitsToSpawn;
        _unitToSpawn = level.UnitToSpawn;
    }
}