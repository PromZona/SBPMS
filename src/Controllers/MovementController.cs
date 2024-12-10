using System.Collections.Generic;
using System.Linq;
using Godot;

namespace ThesisGame.Controllers;

public class MovementController : IController
{
    private List<Marker3D> _finsihPoints = new();

    private RandomNumberGenerator _random = new();
    
    public void Update(double delta)
    {
        List<Unit> units = Game.Units;

        foreach (var unit in units)
        {
            if (unit.CurrentMovementStage == Unit.MovementStage.Finished)
            {
                int index = _random.RandiRange(0, _finsihPoints.Count - 1);
                Marker3D finishPoint = _finsihPoints[index];
                unit.NavigationAgent.TargetPosition = finishPoint.GlobalTransform.Origin;
                unit.CurrentMovementStage = Unit.MovementStage.IsMoving;
            }
        }
    }

    public void Initialize(LevelData level)
    {
        _finsihPoints.Clear();
        _finsihPoints = level.GetFinishPoints().ToList();
    }
}