using System.Collections.Generic;
using Godot;

namespace ThesisGame;

public partial class Unit : Node3D
{
    public float Speed = 2.0f;
    
    private List<Color> _colors = new()
    {
        Colors.BlueViolet, Colors.DarkGoldenrod, Colors.DarkSlateGray, Colors.LightCoral
    };
    public NavigationAgent3D NavigationAgent;
    
    public MovementStage CurrentMovementStage = MovementStage.Finished;
    
    public enum MovementStage
    {
        IsMoving,
        Finished
    }

    public override void _Ready()
    {
        base._Ready();
        MeshInstance3D mesh = GetNode<MeshInstance3D>("Mesh");
        NavigationAgent = GetNode<NavigationAgent3D>("NavAgent");
        OmniLight3D light = GetNode<OmniLight3D>("Light");

        var rand = new RandomNumberGenerator();
        int index = rand.RandiRange(0, _colors.Count - 1);
        var material = new StandardMaterial3D();
        material.AlbedoColor = _colors[index];
        mesh.MaterialOverride = material;

        light.LightColor = _colors[index];
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        
        if (CurrentMovementStage == MovementStage.Finished)
            return;

        if (NavigationAgent.IsNavigationFinished())
        {
            CurrentMovementStage = MovementStage.Finished;
            return;
        }
        
        Transform3D currentAgentPosition = GlobalTransform;
        Vector3 nextPathPosition = NavigationAgent.GetNextPathPosition();

        Vector3 direction = currentAgentPosition.Origin.DirectionTo(nextPathPosition);
        currentAgentPosition.Origin = GlobalTransform.Origin + (direction * Speed *  (float)delta);
        GlobalTransform = currentAgentPosition;
    }
}