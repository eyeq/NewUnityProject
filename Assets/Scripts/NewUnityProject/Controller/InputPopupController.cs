using NewUnityProject.Model;
using NewUnityProject.View;
using UnityEngine;
using UnityEngine.Events;

namespace NewUnityProject.Controller
{
    public class InputPopupController : MonoBehaviour
    {
        public InputPopupModel ViewModel => _view.Model;

        public InputPopupModel Model { get; private set; }

        private InputPopupView _view;
        
        public void Init(InputPopupModel model)
        {
            Model = model;
            _view.Show(model);
        }

        public void SetClickListener(UnityAction call)
        {
            _view.SetClickListener(call);
        }

        private void Awake()
        {
            _view = GetComponent<InputPopupView>();
        }
    }
}