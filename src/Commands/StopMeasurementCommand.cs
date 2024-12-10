using System.Collections.Generic;
using ThesisGame.Controllers;

namespace ThesisGame.Commands;

public class StopMeasurementCommand : Command
{
    public override void Execute()
    {
        Game.PerformanceBotController.StopCollection();
        Result = CommandResult.Succeed;
    }
}