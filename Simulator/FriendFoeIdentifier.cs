namespace Simulator;
public interface IFriendFoeIdentifier : IModel
{
    void IdentifyRadarScan(object? sender, RadarScanEventArgs e);
    public event EventHandler<EventArgs>? OnHostileDetection;
}

public class OddFoeIdentifier : IFriendFoeIdentifier
{
    public event EventHandler<EventArgs>? OnHostileDetection;
    public void IdentifyRadarScan(object? sender, RadarScanEventArgs e)
    {
        if(IsHostile(e.ScanResults)) OnHostileDetection?.Invoke(this, EventArgs.Empty);
    }

    private static bool IsHostile(List<int> scanResults)
    {
        //int acc = 0;
        //foreach(int i in scanResults)
        //    acc += i % 2;
        //int oddCount = scanResults.Aggregate(0,(acc,x) => acc + x % 2 );
        int oddCount = scanResults.Sum(x=>x%2);
        return oddCount > scanResults.Count / 2;
    }

    public void Update() { }
}