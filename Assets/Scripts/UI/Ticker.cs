using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Ticker : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private float velocity;

    private RectTransform RectTransform
    {
        get { return (RectTransform)transform; }
    }

    public float Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }

    public string Text
    {
        get { return text.text; }
        set { text.text = value; }
    }

    private float preferredWidth;
    private float xPosition;

    public void Update()
    {
        if (!Mathf.Approximately(preferredWidth,text.preferredWidth))
        {
            preferredWidth = text.preferredWidth;
            xPosition = RectTransform.rect.width;
        }

        xPosition -= velocity*Time.deltaTime;

        Vector3 localPosition = text.transform.localPosition;
        localPosition.x = xPosition;                 
        text.transform.localPosition = localPosition;

        if (xPosition < -preferredWidth)
        {
            xPosition = RectTransform.rect.width;
        }
    }
}
