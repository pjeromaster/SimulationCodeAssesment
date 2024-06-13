namespace Simulator;

public interface IRadar : IModel
{
    public event EventHandler<RadarScanEventArgs>? OnScan;
}

public class CSVFileRadar : IRadar
{
    public event EventHandler<RadarScanEventArgs>? OnScan;
    private readonly StreamReader reader;
    public CSVFileRadar(string path)
    {
        //try
        //{
        reader = new(path);
        _ = reader.ReadLine() ?? throw new IOException();
        ResetReader();
        //}
        //catch(IOException e)
        //{
        //    Console.WriteLine($"The file at \"{path}\" could not be read:");
        //    Console.WriteLine(e.Message);
        //}
    }
    public void Update()
    {
        string? line = reader.ReadLine();
        if(line != null)
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