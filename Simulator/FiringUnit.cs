namespace Simulator;
public interface IFiringUnit : IModel
{
    void FireMissile(object? sender, EventArgs e);
    public event EventHandler<MissileFiredEventArgs>? OnMissileFired;
}
public class FiringUnit : IFiringUnit
{
    private readonly Random rnd = new();
    private float killProbability;
    public float KillProbability
    {
        get => killProbability;
        set => killProbability = Math.Clamp(value, 0, 1);
    }

    public event EventHandler<MissileFiredEventArgs>? OnMissileFired;
    public FiringUnit(float killProbability) => KillProbability = killProbability;

    public void FireMissile(object? sender, EventArgs e)
    {
        MissileFiredEventArgs eventArgs = new(rnd.NextSingle()<= KillProbability);
        OnMissileFired?.Invoke(this, eventArgs);
    }
    public void Update() { }
}

public class MissileFiredEventArgs(bool targetNeutralized) : EventArgs
{
    public bool TargetNeutralised { get; } = targetNeutralized;
}