namespace Assets.Scripts.Interfaces
{
    public interface IGameSession
    {
        ISpaceshipState Spaceship { get; }
        IGameScore Score { get; }
    }
}