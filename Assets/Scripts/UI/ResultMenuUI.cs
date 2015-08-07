
using System;
using UnityEngine;
using Random = UnityEngine.Random;

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