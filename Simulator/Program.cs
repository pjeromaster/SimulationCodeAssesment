using System.Timers;

namespace Simulator;
internal class Program
{
    static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.CursorVisible = false;

        string csvPath = (args.Length > 0) ? args[0] : FindCSVPath();

        PatriotModel patriot = new(new CSVFileRadar(csvPath), new OddFoeIdentifier(), new FiringUnit(0.8f));

        Console.WriteLine("Initiated Patriot Model");
        System.Timers.Timer timer = new(interval:1000);
        int StepCount = 0;

        timer.Elapsed += ( (object? sender, ElapsedEventArgs e) =>
        {
            StepCount++;
            Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"\n{StepCount,3} | ");
            Console.CursorLeft = 35;
            Console.Write('|');
            Console.CursorLeft = 56;
            Console.Write('|');
            patriot.Update();
        } );

        patriot.FriendFoeIdentifier.OnHostileDetection += ( (object? sender, EventArgs e) =>
        {
            Console.CursorLeft = 7;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Hostile Entity Identified");
        });
        patriot.FiringUnit.OnMissileFired += ( (object? sender, MissileFiredEventArgs e) =>
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
        });

        timer.Start();

        while(patriot.Radar.IsActive())
            Thread.Sleep(1000);
        Console.WriteLine("\nSimulation is finished");
    }

    static string FindCSVPath()
    {
        if(File.Exists("radar-output.csv")) return "radar-output.csv";
        if(File.Exists("..\\..\\..\\radar-output.csv")) return "..\\..\\..\\radar-output.csv";
        throw new FileNotFoundException("Could not find radar-output.csv");
    }
}