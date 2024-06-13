using System.Diagnostics;
using System.Timers;

namespace Simulator;
internal class Program
{
    static void Main(string[] args)
    {
        string csvPath = (args.Length > 0) ? args[0] : @"C:\Users\pjro\source\repos\Simulator\Simulator\radar-output.csv";
        PatriotModel patriot = new(new CSVFileRadar(csvPath), new OddFoeIdentifier(), new FiringUnit(0.8f));

        Console.WriteLine("Initiated Patriod Model");
        System.Timers.Timer timer = new()
        {
            Interval = 1000
        };
        string StepIndex="";
        int StepCount = 0;

        timer.Elapsed += ( (object? sender, ElapsedEventArgs e)=>{
            StepCount++;
            StepIndex = $"{StepCount,2} | ";
            patriot.Update();
        });

        //patriot.Radar.OnScan += ( (object? sender, RadarScanEventArgs e) =>
        //{
        //    Console.Write(StepIndex + "New Scanresults:");
        //    foreach(var i in e.ScanResults) Console.Write($"{i,8}|");
        //    Console.WriteLine();
        //}
        //);

        patriot.FriendFoeIdentifier.OnHostileDetection += ( (object? sender, EventArgs e) => Console.WriteLine(StepIndex + "Hostile Detected") );

        patriot.FiringUnit.OnMissileFired += ( (object? sender, MissileFiredEventArgs e) => Console.WriteLine(StepIndex + (e.TargetNeutralised ? "Target Neutralized" : "Target still Operational")) );

        timer.Start();

        Console.ReadLine();
    }
}

public class PatriotModel : IModel
{
    public IRadar Radar { get; }
    public IFriendFoeIdentifier FriendFoeIdentifier { get; }
    public IFiringUnit FiringUnit { get; }
    public PatriotModel(IRadar radar, IFriendFoeIdentifier friendFoeIdentifier, IFiringUnit firingUnit)
    {
        Radar = radar;
        FriendFoeIdentifier = friendFoeIdentifier;
        FiringUnit = firingUnit;

        Radar.OnScan += FriendFoeIdentifier.IdentifyRadarScan;
        FriendFoeIdentifier.OnHostileDetection += firingUnit.FireMissile;
    }

    public void Update()
    {
        Radar.Update();
        FriendFoeIdentifier.Update();
        FiringUnit.Update();
    }
}

public interface IModel
{
    public void Update();
}