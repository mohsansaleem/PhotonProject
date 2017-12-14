using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Controllers.MainMenu
{
    public class StartSceneController : MonoBehaviour
    {
        [SerializeField]
        RectTransform waitingScreen;

        public void OnStartClicked()
        {
            waitingScreen.gameObject.SetActive(true);

            StartCoroutine(LoadLobbyAsync());
        }

        IEnumerator LoadLobbyAsync()
        {
            Debug.LogError("Going To Lobby...");
            // The Application loads the Scene in the background at the same time as the current Scene.
            //This is particularly good for creating loading screens. You could also load the Scene by build //number.
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LobbyScene");

            //Wait until the last operation fully loads to return anything
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}