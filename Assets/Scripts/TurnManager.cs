using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    // Should be private, but need it for test example (I don't know)
    // how to set characters (maybe a constructor?)
    [SerializeField] public List<Character> characters;

    [SerializeField] private float nextTurnDelay = 1f;

    // This should be zero, I am off in a modulo calculation somewhere
    private int currentCharacterIndex = -1;

    public Character CurrentCharacter;

    public UnityEvent<Character> OnTurnStart;
    public UnityEvent<Character> OnTurnEnd;

    public static TurnManager Instance;

    public GameObject victoryScreen;
    public GameObject defeatScreen;
    private bool gameOver = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BeginNextTurn();

    }


    void OnEnable()
    {
        // Subscribe
        Character.OnDie += OnCharacterDie;
    }

    void OnDisable()
    {
        // Unsubscribe
        Character.OnDie -= OnCharacterDie;
    }

    public void BeginNextTurn()
    {
        if (!gameOver)
        {
            currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
            CurrentCharacter = characters[currentCharacterIndex];
            Debug.Log($"BeginNextTurn: {CurrentCharacter.name}");
            OnTurnStart?.Invoke(CurrentCharacter);
        }
    }

    public void EndCurrentTurn()
    {
        Debug.Log($"EndCurrentTurn: {CurrentCharacter.name}");
        OnTurnEnd?.Invoke(CurrentCharacter);
        Invoke(nameof(BeginNextTurn), nextTurnDelay);
    }

    void OnCharacterDie(Character character)
    {
        if (character.isPlayer)
        {
            Debug.Log("You lost!");
            defeatScreen.SetActive(true);
        }
        else
        {
            Debug.Log("You win!");
            string currentLevel = SceneManager.GetActiveScene().name;
            switch (currentLevel)
            {   
                case "Tutorial":
                    PlayerPrefs.SetInt("Tutorial", 1);
                    break;
                case "Level1":
                    LevelManager.UnlockLevel("Level2");
                    break;
                case "Level2":
                    LevelManager.UnlockLevel("Level3");
                    break;
                case "Level3":
                    LevelManager.UnlockLevel("Level4");
                    break;
                case "Level4":
                    LevelManager.UnlockLevel("Level5");
                    break;
            }
            victoryScreen.SetActive(true);
        }
        gameOver = true;
        StartCoroutine(WaitForEndScreen());
        //SceneManager.LoadScene("LevelSelect");
    }

    IEnumerator WaitForEndScreen()
    {
        yield return new WaitForSeconds(5);
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            SceneManager.LoadScene("Level1");
        }
        else
        {
            SceneManager.LoadScene("LevelSelect");
        }
    }

}
