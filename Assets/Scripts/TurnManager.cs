using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
        CurrentCharacter = characters[currentCharacterIndex];
        OnTurnStart?.Invoke(CurrentCharacter);
    }

    public void EndCurrentTurn()
    {
        OnTurnEnd?.Invoke(CurrentCharacter);
        Invoke(nameof(BeginNextTurn), nextTurnDelay);
    }

    void OnCharacterDie(Character character)
    {
        if (character.isPlayer)
            Debug.Log("You lost!");
        else
            Debug.Log("You win!");
    }

}
