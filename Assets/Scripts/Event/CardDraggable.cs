using UnityEngine;
using UnityEngine.EventSystems;

namespace Event
{
    public class CardDraggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] public Transform fieldPanel;

        private Transform _prevParent;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _prevParent = transform.parent;
            transform.SetParent(_prevParent.parent, false);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var nextParent = _prevParent;
            if (eventData.pointerEnter != null && eventData.pointerEnter.transform == fieldPanel)
            {
                nextParent = fieldPanel;
            }

            var x = transform.position.x;
            var count = 0;
            for (var i = 0; i < nextParent.childCount; i++)
            {
                var child = nextParent.GetChild(i);
                if (child.position.x < x)
                {
                    count++;
                }
            }

            transform.SetParent(nextParent, false);
            transform.SetSiblingIndex(count); 
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}