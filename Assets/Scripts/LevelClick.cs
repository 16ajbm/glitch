using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelClick : MonoBehaviour, IPointerClickHandler
{
    public string levelName;
    public GameObject lockedSprite;

    void Start()
    {
        if (LevelManager.IsLevelUnlocked(levelName))
        {
            lockedSprite.SetActive(false);
        } else
        {
            lockedSprite.SetActive(true);
        }
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene(levelName);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (LevelManager.IsLevelUnlocked(levelName))
        {
            Debug.Log($"Level: {levelName} selected.");
            LoadLevel();
        } else
        {
            Debug.Log($"Level: {levelName} is locked.");
        }
    }
}
