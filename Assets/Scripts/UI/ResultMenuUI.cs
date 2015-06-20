namespace Assets.Scripts.UI
{
    public class ResultMenuUI : MenuUI
    {
        public void OnStart()
        {      
            Game.Begin ();
        }

        public void OnAbort()
        {
            Game.Abort ();
        }
    }
}