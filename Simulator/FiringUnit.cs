namespace Simulator;

/// <summary> Represents a firing unit in the system which can fire missiles and report on the result. </summary>
/// <seealso cref="Simulator.IModel" />
public interface IFiringUnit : IModel
{
    /// <summary> Prompts the Firing unit to fire a missile </summary>
    void FireMissile(object? sender, EventArgs e);
    /// <summary> Occurs when a missile has been fired and contains whether the target has been neutralised.</summary>
    public event EventHandler<MissileFiredEventArgs>? OnMissileFired;
}

public class ProbabilityFiringUnit : IFiringUnit
{
    private readonly Random rnd = new();
    private float killProbability;
    /// <summary> Gets or sets the probability of successfully hitting the target. The value is clamped between 0 and 1. </summary>
    public float KillProbability
    {
        get => killProbability;
        set => killProbability = Math.Clamp(value, 0, 1);
    }
    /// <summary> Occurs when a missile has been fired and contains information about whether the target has been neutralized.</summary>
    public event EventHandler<MissileFiredEventArgs>? OnMissileFired;

    /// <summary> Initializes a new instance of the <see cref="ProbabilityFiringUnit"/> class with a specified kill probability. </summary>
    /// <param name="killProbability">The probability of successfully hitting the target.</param>
    public ProbabilityFiringUnit(float killProbability) => KillProbability = killProbability;

    /// <summary> Prompts the firing unit to fire a missile. Determines if the target is hit based on the kill probability and raises the OnMissileFired event. </summary>
    public void FireMissile(object? sender, EventArgs e)
    {
        MissileFiredEventArgs eventArgs = new(rnd.NextSingle()<= KillProbability);
        OnMissileFired?.Invoke(this, eventArgs);
    }

    /// <summary> Updates the state of the firing unit.</summary>
    public void Update() { }
}


public class MissileFiredEventArgs(bool targetNeutralized) : EventArgs
{
    public bool TargetNeutralised { get; } = targetNeutralized;
}