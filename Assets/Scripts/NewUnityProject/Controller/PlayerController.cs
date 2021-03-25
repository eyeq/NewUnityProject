using NewUnityProject.Extensions;
using NewUnityProject.Model;
using NewUnityProject.View;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NewUnityProject.Controller
{
    public class PlayerController : MonoBehaviour, IPunInstantiateMagicCallback, IDragHandler
    {
        public PlayerModel Model { get; private set; }

        private PlayerView _view;

        public void Init(PlayerModel model)
        {
            Model = model;
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
            if (photonView.IsMine)
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
            if (photonView.IsMine)
            {
                transform.position = eventData.position;
            }
        }
    }
}