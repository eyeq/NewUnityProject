using UnityEngine;
using UnityEngine.EventSystems;

public class CardDraggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    public Transform fieldPanel;
    
    private Transform _parent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _parent = transform.parent;
        transform.SetParent(_parent.parent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter.transform == fieldPanel)
        {
            transform.SetParent(fieldPanel, false);
        }
        else
        {
            transform.SetParent(_parent, false);
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
