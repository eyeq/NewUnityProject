using NewUnityProject.Model;
using UnityEngine;
using UnityEngine.UI;

namespace NewUnityProject.View
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] public Text text;
        
        public void Show(PlayerModel playerModel)
        {
            text.text = playerModel.Name;
        }
    }
}