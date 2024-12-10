using System.Collections.Generic;

namespace ThesisGame.Controllers;

public class GameCommandController : IController
{
    private Queue<Command> _commands = new Queue<Command>();
    
    public void Update(double delta)
    {
        if (_commands.Count == 0)
            return;

        while (_commands.Count > 0)
        {
            var command = _commands.Dequeue();
            command.Execute();
        }
    }

    public void AddCommand(Command command)
    {
       _commands.Enqueue(command); 
    }

    public void ClearCommands()
    {
        _commands.Clear();
    }
}