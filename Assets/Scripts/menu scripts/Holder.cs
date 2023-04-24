using UnityEngine;
using UnityEngine.EventSystems;

public class Holder : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private bool hold;
    private Vector2 origin; // ���������� �������
    private Vector2 direction; // ����������� 

    private void Awake()
    {
        direction = Vector2.zero;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        hold = true;
        origin = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)// ����������� �� ������
    {
        Vector2 currentPosition = eventData.position;
        direction = currentPosition - origin;//.normalized;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        hold = false;
        direction = Vector2.zero;
    }

    public bool IsTapped()
    {
        return hold;
    }

    public Vector2 GetDirection()
    {
        return direction;
    }

    public Vector2 GetPosition()
    {
        return origin;
    }
}
