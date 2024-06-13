namespace Simulator;
/// <summary> Represents a system for identifying friend or foe from radar scans. </summary>
public interface IFriendFoeIdentifier : IModel
{
    /// <summary> Identifies targets in a radar scan as either friendly or hostile. </summary>
    /// <param name="e">A RadarScanEventArgs object containing the radar scan data.</param>
    void IdentifyRadarScan(object? sender, RadarScanEventArgs e);
    /// <summary> Occurs when a hostile target is detected. </summary>
    public event EventHandler<EventArgs>? OnHostileDetection;
}

public class OddFoeIdentifier : IFriendFoeIdentifier
{
    public event EventHandler<EventArgs>? OnHostileDetection;
    public void IdentifyRadarScan(object? sender, RadarScanEventArgs e)
    {
        if(IsHostile(e.ScanResults))
            OnHostileDetection?.Invoke(this, EventArgs.Empty);
    }

    /// <summary> Determines if a target is hostile based on the number of odd numbers in <paramref name="scanResults"/>.</summary>
    /// <param name="scanResults">A list of integers containing the radar scan data.</param>
    /// <returns>True if the majority of scan results are odd numbers, otherwise false.</returns>
    private static bool IsHostile(List<int> scanResults)
    {
        int oddCount = scanResults.Sum(x=>x%2);
        return oddCount > scanResults.Count / 2;
    }

    public void Update() { }
}