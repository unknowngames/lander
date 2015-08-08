using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class BackgroundFolower : MonoBehaviour
{
    public Transform Target;

    [SerializeField]
    private float xOffset = 750;
    [SerializeField]
    private float yOffset = 50;
                

    public void Update()
    {
        if (GameHelper.PlayerSpaceship != null)
        {
            Target = GameHelper.PlayerSpaceship.transform;
        }

        if (Target == null)
        {
            return;
        }

        Vector3 position = transform.position;

        position.x = Target.position.x + xOffset;
        position.y = Target.position.y + yOffset;

        transform.position = position;
    }
}
