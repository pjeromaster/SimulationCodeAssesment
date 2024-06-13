namespace Simulator;
/// <summary> Represents a radar system that can perform scans and detect targets </summary>
public interface IRadar : IModel
{
    /// <summary> Occurs when a radar scan is performed. </summary>
    public event EventHandler<RadarScanEventArgs>? OnScan;
    /// <summary> Checks if the radar is currently scanning. </summary>
    public bool IsActive();
}

/// <summary> Represents a radar system that reads scan data from a CSV file. </summary>
public class CSVFileRadar : IRadar
{
    public event EventHandler<RadarScanEventArgs>? OnScan;
    private readonly StreamReader reader;
    private bool Active;
    public bool IsActive() => Active;
    public CSVFileRadar(string path)
    {
        reader = new(path);
        _ = reader.ReadLine() ?? throw new IOException();
        ResetReader();
        Active = true;
    }

    /// <summary> Updates the radar system by reading the next line from the CSV file and performing a scan.
    /// If the end of the file is reached, the radar is deactivated.</summary>
    public void Update()
    {
        if(!Active) return;
        string? line = reader.ReadLine();
        if(line == null)
        {
            Active = false;
            reader.Close();
        }
        else
        {
            List<int> data = ParseRadarOutput(line);
            RadarScanEventArgs e = new(data);
            OnScan?.Invoke(this, e);
        }
    }

    private static List<int> ParseRadarOutput(string line)
    {
        List<string> split = [.. line.Split(';')];
        return split.ConvertAll(int.Parse);
    }
    /// <summary> Resets the reader to the beginning of the CSV file. </summary>
    private void ResetReader()
    {
        reader.BaseStream.Position = 0;
        reader.DiscardBufferedData();
    }
}
public class RadarScanEventArgs(List<int> scanResults) : EventArgs
{
    public List<int> ScanResults { get; } = scanResults;
}