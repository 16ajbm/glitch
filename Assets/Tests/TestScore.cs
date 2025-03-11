using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestScores : MonoBehaviour
{
    new public AudioSource audio;
    private bool hasBeenCalled = false;
    public BeatRoller beatRoller;

    void Start()
    {

    }

    void Update()
    {
        if (audio.time >= 5 && !hasBeenCalled)
        {
            hasBeenCalled = true;
            beatRoller.Score(6, (finalScore) => {
                Debug.Log($"Your score is: {finalScore}");
            });
        }
    }
}