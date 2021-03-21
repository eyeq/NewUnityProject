using Extensions;
using Model;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using View;

namespace Controller
{
    public class PlayerController : MonoBehaviour, IPunInstantiateMagicCallback, IDragHandler
    {
        private PlayerView _view;
        
        public void Init(PlayerModel model)
        {
            _view.Show(model);
        }
        
        private void Awake()
        {
            _view = GetComponent<PlayerView>();
        }
        
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
            Init(new PlayerModel()
            {
                Name = info.Sender.GetNicknameOrDefault(),
            });
            
            var photonView = GetComponent<PhotonView>();
            if (photonView.Owner.UserId == PhotonNetwork.LocalPlayer.UserId)
            {
                transform.SetAsLastSibling();
            }
            else
            {
                transform.SetAsFirstSibling();
            }
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            var photonView = GetComponent<PhotonView>();
            if (photonView.Owner.UserId == PhotonNetwork.LocalPlayer.UserId)
            {
                transform.position = eventData.position;
            }
        }
    }
}