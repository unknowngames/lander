namespace Assets.Scripts.UI
{
    public class GameMenuUI : MenuUI
    {
        public void OnPause()
        {
            Game.Pause();
        }
    }
}