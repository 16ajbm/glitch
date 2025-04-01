using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject firstLevel;

    void Start()
    {
        LevelManager.UnlockLevel(firstLevel.GetComponent<LevelClick>().levelName);
    }

        public static void UnlockLevel(string levelName)
    {
        PlayerPrefs.SetInt(levelName, 1);
        PlayerPrefs.Save();
    }

    public static bool IsLevelUnlocked(string levelName)
    {
        return PlayerPrefs.GetInt(levelName, 0) == 1;
    }
}
