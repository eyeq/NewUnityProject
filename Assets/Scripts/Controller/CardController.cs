using Model;
using UnityEngine;
using UnityEngine.EventSystems;
using View;

namespace Controller
{
    public class CardController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] public Transform handPanel;
        
        [SerializeField] public Transform fieldPanel;

        private CardView _view;
        
        private Transform _prevParent;

        private void Awake()
        {
            _view = GetComponent<CardView>();
        }

        public void Init(CardModel model, Transform hand, Transform field)
        {
            handPanel = hand;
            fieldPanel = field;
            _view.Show(model);
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _prevParent = transform.parent;
            if (_prevParent != handPanel)
            {
                return;
            }
            
            transform.SetParent(_prevParent.parent, false);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_prevParent != handPanel)
            {
                return;
            }
            
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_prevParent != handPanel)
            {
                return;
            }
            
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