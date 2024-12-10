using System.Collections.Generic;

namespace ThesisGame.Controllers;

public abstract class Command
{
    public enum CommandResult
    {
        Succeed,
        Running,
        Failed
    }

    public CommandResult Result { get; protected set; } = CommandResult.Running;
    public abstract void Execute();
}