using Timer = System.Timers.Timer;

namespace Simulator;
internal static class Program
{
    private static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.White; Console.CursorVisible = false;

        Timer timer = new(interval:1000);
        int StepCount = 0;

        string csvPath = (args.Length > 0) ? args[0] : FindCSVPath();
        PatriotModel patriot = new(new CSVFileRadar(csvPath), new OddFoeIdentifier(), new ProbabilityFiringUnit(0.8f));
        patriot.FriendFoeIdentifier.OnHostileDetection += OnHostileDetection;
        patriot.FiringUnit.OnMissileFired += OnMissileFired;

        timer.Elapsed += (object? sender, ElapsedEventArgs e) =>
        {
            StepCount++;
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorLeft = 0; Console.Write($"\n{StepCount,3} | ");
            Console.CursorLeft = 35; Console.Write('|');
            Console.CursorLeft = 56; Console.Write('|');
            patriot.Update();
        };

        timer.Start();

        while(patriot.Radar.IsActive())
            Thread.Sleep(1000);
    }

    private static void OnMissileFired(object? sender, MissileFiredEventArgs e)
    {
        Console.CursorLeft = 37;
        if(e.TargetNeutralised)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Target Neutralized");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Target Operational");
        }
    }

    private static void OnHostileDetection(object? sender, EventArgs e)
    {
        Console.CursorLeft = 7;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Hostile Entity Identified");
    }

    /// <summary> Attempts to locate the CSV file relative to the working directory. </summary>
    /// <returns> The relative path to the CSV file </returns>
    /// <exception cref="System.IO.FileNotFoundException">Could not find radar-output.csv</exception>
    private static string FindCSVPath() => File.Exists("radar-output.csv")
            ? "radar-output.csv"
            : File.Exists("..\\..\\..\\radar-output.csv")
            ? "..\\..\\..\\radar-output.csv"
            : throw new FileNotFoundException("Could not find radar-output.csv");
}