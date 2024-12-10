using System.Collections.Generic;
using Godot;

namespace ThesisGame.Controllers;

public class WaitCommand : Command
{
    private ulong _waitTime;
    private ulong _currentTime;


    public WaitCommand(ulong waitTimeSeconds)
    {
        _waitTime = waitTimeSeconds * 1000;
    }

    public override void Execute()
    {
        if (_currentTime == 0)
        {
            GD.Print($"Wait for {_waitTime / 1000} seconds");
            _currentTime = Time.Singleton.GetTicksMsec();
            Result = Command.CommandResult.Running;
        }

        ulong time = Time.Singleton.GetTicksMsec();

        if (time - _currentTime >= _waitTime)
        {
            Result = Command.CommandResult.Succeed;
        }
    }
}