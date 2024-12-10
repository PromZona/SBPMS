using Godot;
using ThesisGame.Controllers;

namespace ThesisGame.Commands;

public class CloseGameCommand : Command
{
    public override void Execute()
    {
        GD.Print("Closing game...");
        Game.Instance.GetTree().Quit();
    }
}