namespace Assets.Scripts
{
    public interface IGameSession
    {
        ISpaceshipState Spaceship { get; }
        IGameScore Score { get; }
    }
}