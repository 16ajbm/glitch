using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Source: https://www.youtube.com/watch?v=PMfhS-kEvc0&list=PLLPYMaP0tgFKZj5VG82316B63eet0Pvsv&index=2
    public AudioSource theMusic;
    public bool startPlaying;
    public BeatScroller theBS;
    public static GameManager instance; 
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                theBS.hasStarted = true;
                theMusic.Play();
            }
        }
    }

    public void NoteHit()
    {
        Debug.Log("Hit");
    }

    public void NoteMiss()
    {
        Debug.Log("Miss");
    }
}
