namespace Assets.Scripts.UI
{
    public class ResultMenuUI : MenuUI
    {
        public void OnStart()
        {      
            GameHelper.Begin ();
        }

        public void OnAbort()
        {
            GameHelper.Abort();
        }
    }
}