using ThesisGame.Controllers;

namespace ThesisGame.Commands;

public class SaveMeasurementsCommand : Command
{
    public override void Execute()
    {
        Game.PerformanceBotController.SaveMeasurements();
        Result = CommandResult.Succeed;
    }
}