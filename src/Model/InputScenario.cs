using System.Collections.Generic;

namespace ThesisGame.Model;

public class InputScenario
{
    public List<Scenario> Scenarios { get; set; }
}

public class Scenario
{
    public string LevelName { get; set; }
    public List<string> Measurements { get; set; }
    public ulong CollectingTime { get; set; }
}