namespace Assets.Scripts.UI
{
    public class PauseMenuUI : MenuUI
    {
        public void OnUnpause()
        {
            Game.Unpause();
        }

        public void OnAbort()
        {
            Game.Abort();
        }
    }
}