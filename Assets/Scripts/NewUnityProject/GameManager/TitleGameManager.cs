using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NewUnityProject.GameManager
{
    public class TitleGameManager : MonoBehaviour
    {
        [SerializeField] public Transform playerNameDialog;
        [SerializeField] public InputField playerNameInputField;

        public void Start()
        {
            playerNameDialog.gameObject.SetActive(false);
        }
        
        public void Update()
        {
            if (playerNameDialog.gameObject.activeInHierarchy)
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
                if(string.IsNullOrEmpty(playerName))
                {
                    playerNameDialog.gameObject.SetActive(true);
                    return;
                }
				
                SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
            }
        }

        public void ConfirmPlayerName()
        {
            PlayerPrefs.SetString("playerName", playerNameInputField.text.Trim());
            playerNameDialog.gameObject.SetActive(false);
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