namespace Assets.Scripts.UI
{
    public class PauseMenuUI : MenuUI
    {
        public void OnUnpause()
        {
            GameHelper.Unpause();
        }

        public void OnAbort()
        {
            GameHelper.Abort();
        }
    }
}