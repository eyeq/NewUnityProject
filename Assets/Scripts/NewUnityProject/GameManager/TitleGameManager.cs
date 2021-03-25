using NewUnityProject.Controller;
using NewUnityProject.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NewUnityProject.GameManager
{
    public class TitleGameManager : MonoBehaviour
    {
        [SerializeField] public InputPopupController inputPopup;

        public void Start()
        {
            inputPopup.gameObject.SetActive(false);
            inputPopup.SetClickListener(() =>
            {
                inputPopup.gameObject.SetActive(false);
                PlayerPrefs.SetString("playerName", inputPopup.ViewModel.Input.Trim());
            });
        }

        public void Update()
        {
            if (inputPopup.gameObject.activeInHierarchy)
            {
                return;
            }

            if (AnyInput())
            {
                var playerName = "";
                if (PlayerPrefs.HasKey("playerName"))
                {
                    playerName = PlayerPrefs.GetString("playerName").Trim();
                }

                if (string.IsNullOrEmpty(playerName))
                {
                    inputPopup.Init(new InputPopupModel()
                    {
                        Message = "ユーザー名を入力してください。",
                        Placeholder = "Enter your Nick.",
                        Input = playerName,
                    });
                    inputPopup.gameObject.SetActive(true);
                    return;
                }

                SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
            }
        }

        private static bool AnyInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                return true;
            }

            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                return true;
            }

            return false;
        }
    }
}