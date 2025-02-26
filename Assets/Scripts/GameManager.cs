using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public AudioSource audioSource;
    public bool startPlaying;
    public BeatScroller beatScroller;
    public static GameManager instance;

    public int currentScore = 0;

    public int scorePerNote = 100;

    public float currentMultiplier = 1.0f;

    public float multiplierPerNote = 0.1f;

    public float multiplierMax = 4.0f;

    public float multiplierMin = 1.0f;


    public TMP_Text scoreText;
    public TMP_Text multiplierText;

    void Start()
    {
        instance = this;
        scoreText.SetText("Score: 0");
        multiplierText.SetText("Multiplier: x1");
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                beatScroller.hasStarted = true;
                audioSource.PlayDelayed(1.75f);
            }
        }
    }

    public void NoteHit()
    {
        Debug.Log("Hit");

        currentScore += (int) Mathf.Round(scorePerNote * currentMultiplier);

        currentMultiplier = Mathf.Min(currentMultiplier + multiplierPerNote, multiplierMax);

        scoreText.SetText("Score: " + currentScore);
        multiplierText.SetText("Muliplier: x" + currentMultiplier);
    }

    public void NoteMiss()
    {
        Debug.Log("Miss");

        currentMultiplier = multiplierMin;
        multiplierText.SetText("Muliplier: x" + currentMultiplier);
    }
}
