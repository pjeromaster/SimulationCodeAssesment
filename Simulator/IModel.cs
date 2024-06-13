namespace Simulator;

/// <summary> Interface for objects that may require to be updated each timestep. </summary>
public interface IModel
{
    /// <summary> Called once every timestep.</summary>
    /// <remarks> Children that implement <cref>IModel</cref> should have their <cref>Update</cref> function called from their parents <cref>Update</cref> function</remarks>
    public void Update();
}
