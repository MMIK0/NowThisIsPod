using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public void ChangeScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
       Time.timeScale = 1f;
    }
    
   
    public void QuitGame()
    {
        Application.Quit();
    }
}
