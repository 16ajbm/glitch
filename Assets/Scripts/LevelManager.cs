using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject firstLevel;

    void Start()
    {
        LevelManager.UnlockLevel(firstLevel.GetComponent<LevelClick>().levelName);
        // Need to set the following when user presses new game on main menu: PlayerPrefs.SetInt("Tutorial1", 0);
    }

    public static void UnlockLevel(string levelName)
    {
        PlayerPrefs.SetInt(levelName, 1);
        PlayerPrefs.Save();
    }

    public static void LockLevel(string levelName)
    {
        PlayerPrefs.SetInt(levelName, 0);
        PlayerPrefs.Save();
    }

    public static bool IsLevelUnlocked(string levelName)
    {
        return PlayerPrefs.GetInt(levelName, 0) == 1;
    }
}
