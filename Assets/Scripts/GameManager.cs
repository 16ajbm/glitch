using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public AudioSource audioSource;
    public bool startPlaying;
    public BeatScroller beatScroller;
    public static GameManager instance;

    public int currentScore = 0;

    public int numGoldenNotes;


    public int scorePerNote = 100;
    public int scorePerGoldenNote = 250;

    public TMP_Text scoreText;
    public TMP_Text goldenNoteText;

    void Start()
    {
        instance = this;
        scoreText.SetText("Score: 0");
        goldenNoteText.SetText("Golden Notes: 0");
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
                audioSource.PlayDelayed(3.25f);
            }
        }
    }

    public void NoteHit(NoteObject note)
    {
        Debug.Log("Hit");

        if (note.isGoldenNote)
        {
            currentScore += note.goldenNoteScore;
            numGoldenNotes++;
            goldenNoteText.SetText("Golden Notes: " + numGoldenNotes);
        }
        else
        {
            currentScore += note.defaultScore;
        }

        scoreText.SetText("Score: " + currentScore);
    }

    public void NoteMiss()
    {
        Debug.Log("Miss");
    }
}
