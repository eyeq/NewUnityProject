using UnityEngine;
using UnityEngine.EventSystems;

namespace Event
{
    public class PlayerDraggable : MonoBehaviour, IDragHandler
    {
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }
    }
}