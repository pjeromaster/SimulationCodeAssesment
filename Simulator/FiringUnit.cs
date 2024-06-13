using System.Diagnostics;

namespace Simulator;

public class FiringUnit : IFiringUnit
{
    private readonly Random rnd;
    private float killProbability;
    public float KillProbability
    {
        get => killProbability;
        set => killProbability = Math.Clamp(value, 0, 1);
    }

    public event EventHandler<MissileFiredEventArgs>? OnMissileFired;
    public FiringUnit(float killProbability) : this(killProbability, new Random()) { }
    public FiringUnit(float killProbability, int seed) : this(killProbability, new Random(seed)) { }
    public FiringUnit(float killProbability, Random rnd)
    {
        KillProbability = killProbability;
        this.rnd = rnd;
    }

    public void FireMissile(object? sender, EventArgs e)
    {
        float r = rnd.NextSingle();
        Debug.WriteLine(r);

        MissileFiredEventArgs eventArgs = new(r<= KillProbability);
        OnMissileFired?.Invoke(this, eventArgs);
    }
    public void Update() { }
}
public interface IFiringUnit : IModel
{
    void FireMissile(object? sender, EventArgs e);
    public event EventHandler<MissileFiredEventArgs>? OnMissileFired;
}

public class MissileFiredEventArgs(bool targetNeutralized) : EventArgs
{
    public bool TargetNeutralised { get; } = targetNeutralized;
}