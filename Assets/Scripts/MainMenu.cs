using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetInt("Tutorial", 0);
        LevelManager.UnlockLevel("Level1");
        LevelManager.LockLevel("Level2");
        LevelManager.LockLevel("Level3");
        LevelManager.LockLevel("Level4");
        LevelManager.LockLevel("Level5");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

}
