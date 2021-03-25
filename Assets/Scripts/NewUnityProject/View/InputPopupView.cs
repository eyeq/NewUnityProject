using NewUnityProject.Model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NewUnityProject.View
{
    public class InputPopupView : MonoBehaviour
    {
        [SerializeField] public Text message;

        [SerializeField] public Text placeholder;

        [SerializeField] public Text input;

        [SerializeField] public Button button;

        private UnityAction _call;
        
        public InputPopupModel Model =>
            new InputPopupModel()
            {
                Message = message.text,
                Placeholder = placeholder.text,
                Input = input.text,
            };

        public void Show(InputPopupModel model)
        {
            message.text = model.Message;
            placeholder.text = model.Placeholder;
            input.text = model.Input;
        }

        public void SetClickListener(UnityAction call)
        {
            _call = call;
            
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(call);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                _call();
            }
        }
    }
}