using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controller
{
    public class PlayerController : MonoBehaviour, IPunInstantiateMagicCallback, IDragHandler
    {
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }
    }
}