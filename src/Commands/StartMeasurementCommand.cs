using System.Collections.Generic;
using ThesisGame.Controllers;

namespace ThesisGame.Commands;

public class StartMeasurementCommand : Command
{
    public override void Execute()
    {
        Game.PerformanceBotController.StartCollection();
        Result = CommandResult.Succeed;
    }
}
