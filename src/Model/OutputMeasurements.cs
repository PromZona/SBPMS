using System.Collections.Generic;

namespace ThesisGame.Model;

public class OutputMeasurements 
{
    public string Platform { get; set; }
    public string Build { get; set; }
    public string Timestamp { get; set; }
    public List<Sample> Samples { get; set; }
}

public class Sample
{
    public string LevelName { get; set; }
    public string Position { get; set; }
    public Dictionary<string, double> Measurements { get; set; }
}
