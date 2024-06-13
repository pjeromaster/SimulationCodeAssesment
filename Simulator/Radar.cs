namespace Simulator;

public interface IRadar : IModel
{
    public event EventHandler<RadarScanEventArgs>? OnScan;
    public bool IsActive();
}

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