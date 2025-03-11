using UnityEngine;
using UnityEngine.UI;

public class BeatRoller : MonoBehaviour
{
    public float firstBeatOffset = 0.55f;
    new public AudioSource audio;
    public Canvas parentCanvas;
    [Range(0f, 1f), SerializeField] float volume = 1f;

    public GameObject ball;
    public GameObject wholeBeat;
    public GameObject halfBeat;
    public float bpm = 130f;
    private float halfBeatDelta; // Time between half beats
    private float halfBeatDist; // Distance between half beats
    private float halfBeatSpeed;
    private int rangeStart = -1920 / 2;
    private int rangeEnd = 1920 / 2;
    private int rangeLen;
    private int numBeatDivisions = 16; // Number of indicators on screen
    private GameObject[] allBeats; // Array to hold copied sprites
    private Vector2 ballOrigin; // Bottom of ball bounce
    private Vector2 ballTarget; // Peak of ball bounce
    private float[] timeElapsed; // Helps to calculate how much beat sprites should shift each frame
    private int iter = 0; // Help to make random golden notes
    private Color white;
    private Color yellow;

    void Start()
    {
        // Set up default and golden note colors
        white = new Color(1f, 1f, 1f);
        white.a = 1;
        yellow = new Color(1f, 1f, 0f);
        yellow.a = 1;

        ballOrigin = ball.GetComponent<Image>().GetComponent<RectTransform>().anchoredPosition;
        ballTarget = ballOrigin + new Vector2(0, 150);

        rangeLen = rangeEnd - rangeStart;

        wholeBeat.SetActive(false);
        halfBeat.SetActive(false);
        ball.SetActive(true);

        allBeats = new GameObject[numBeatDivisions];
        timeElapsed = new float[numBeatDivisions];

        halfBeatDelta = (60f / bpm) / 2f;
        halfBeatDist = (rangeEnd - rangeStart) / (numBeatDivisions);
        halfBeatSpeed = halfBeatDist / halfBeatDelta;

        for (int i = 0; i < numBeatDivisions; i++)
        {
            float xCalc = rangeStart + i * halfBeatDist;
            if (i % 2 == 0)
            {
                allBeats[i] = Instantiate(wholeBeat);
            }
            else
            {
                allBeats[i] = Instantiate(halfBeat);
            }

            allBeats[i].transform.SetParent(parentCanvas.transform, false);
            Image img = allBeats[i].GetComponent<Image>();
            RectTransform rt = img.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(xCalc, halfBeat.GetComponent<Image>().GetComponent<RectTransform>().anchoredPosition.y);

            allBeats[i].SetActive(true);
        }

        audio.Play();
    }

    void Update()
    {
        // Apply the volume value from the editor to the AudioSource
        if (audio != null)
            audio.volume = volume;

        if (audio.time >= firstBeatOffset)
        {
            // Move ball up and down with a period of 1 beat
            ball.transform.position = Vector3.Lerp(ballOrigin, ballTarget, Mathf.PingPong(Time.time * halfBeatSpeed, 1));

            ball.GetComponent<Image>().GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(ballOrigin, ballTarget, Mathf.PingPong(Time.time / halfBeatDelta, 1));

            // Shift beats left
            for (int i = 0; i < numBeatDivisions; i++)
            {
                GameObject currBeat = allBeats[i];

                Image currIMG = currBeat.GetComponent<Image>();
                RectTransform currRT = currIMG.GetComponent<RectTransform>();

                // Wrap to end of range
                if (currRT.anchoredPosition.x <= rangeStart)
                {
                    currIMG.color = white;

                    currRT.anchoredPosition = new Vector2(rangeEnd, halfBeat.GetComponent<Image>().GetComponent<RectTransform>().anchoredPosition.y);

                    // Insert random golden notes
                    if (iter % 17 == 0 && currBeat.CompareTag("WholeBeat"))
                    {
                        currIMG.color = yellow;
                    }
                }

                currRT.anchoredPosition = new Vector2(currRT.anchoredPosition.x - (halfBeatSpeed * (Time.time - timeElapsed[i])), wholeBeat.GetComponent<Image>().GetComponent<RectTransform>().anchoredPosition.y);
                timeElapsed[i] = Time.time;
            }
            iter++;
        }
    }
}