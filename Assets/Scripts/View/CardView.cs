using Model;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] public Text text;

        public void Show(CardModel cardModel)
        {
            text.text = cardModel.Num.ToString();
        }
    }
}