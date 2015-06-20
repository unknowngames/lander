using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RadialSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public float Radius = 200.0f;
    public float Extent = 50.0f;

    public RectTransform Handle;

    [SerializeField]
    private float MinValue = 0.0f;

    [SerializeField]
    private float Value = 0.5f;

    [SerializeField]
    private float MaxValue = 1.0f;

    bool handleDown = false;
    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        //Debug.Log("Rot: " + Handle.eulerAngles );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UpdateHandle(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateHandle(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateHandle(eventData.position);
    }

    void UpdateHandle(Vector2 position)
    {
        Vector2 mainPos = rectTransform.anchoredPosition;
        Vector2 direction = position - mainPos;

        float magn = direction.magnitude;
        if(magn >= Radius + Extent || magn <= Radius - Extent)
        {
            return;
        }

        direction.Normalize();

        Handle.position = mainPos + direction * Radius;
        float angle = Vector2.Angle(Vector2.up, direction);
        Handle.rotation = Quaternion.Euler(0, 0, angle);

        Debug.Log("Angle: " + angle);
        //Debug.Log("Direction: " + direction.x + " " + direction.y);

        //Debug.Log("Anchor: " + rectTransform.anchoredPosition + " HandlePos " + Handle.anchoredPosition);
    }
}
