using Model;
using UnityEngine;
using View;

namespace Controller
{
    public class CardController : MonoBehaviour
    {
        private CardView _view;
        
        private void Awake()
        {
            _view = GetComponent<CardView>();
        }

        public void Init(CardModel model)
        {
            _view.Show(model);
        }
    }
}