using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject firstLevel;

    void Start()
    {
        LevelProgress.UnlockLevel(firstLevel.GetComponent<LevelClick>().levelName);
    }
}
