StreamReader sr = new(@"C:\Users\pjero\source\repos\Simulator\Simulator\radar-output.csv");
Random rnd = new();
System.Timers.Timer timer = new(1000);
timer.Elapsed += ( (object? sender, ElapsedEventArgs e) =>{
    string? line = sr.ReadLine();
    if(line != null && ( line[6] + line[14] + line[22] + line[30] + line[38] + line[46] + line[54] + line[62] + line[70] + line[78] + line[86] ) > 533)
        Console.WriteLine($"Foe Detected, firing Missile......{( rnd.NextDouble() > 0.8 ? "Missed" : "Target Hit" )}");
} );
timer.Start();
Console.ReadLine();