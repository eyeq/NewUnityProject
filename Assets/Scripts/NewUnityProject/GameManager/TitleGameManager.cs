using UnityEngine;
using UnityEngine.SceneManagement;

namespace NewUnityProject.GameManager
{
    public class TitleGameManager : MonoBehaviour
    {
        public void Update()
        {
            if (AnyInput())
            {
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