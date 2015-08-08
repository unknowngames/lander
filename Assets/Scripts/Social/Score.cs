using System.Threading.Tasks;
using Parse;
using UnityEngine;

namespace Assets.Scripts.Social
{
    public class Score : MonoBehaviour
    {
        public void Start ()
        {
            ParseObject testObject = new ParseObject("TestObject");
            testObject["foo"] = "bar";
            testObject.SaveAsync();

            Task<ParseUser> task = ParseUser.LogInAsync("login", "password");
            task.Wait();
            ParseUser parseUser = task.Result;
            Debug.Log(parseUser);
        }
    }
}
