namespace Simulator;
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
