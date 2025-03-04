using UnityEngine;

public class BeatRoller : MonoBehaviour
{
    public AudioSource audio;
    public GameObject ball;
    public GameObject wholeBeat;
    public GameObject halfBeat;
    public float bpm = 130f;
    private float halfDt; // Time between half beats
    private float halfBeatDist; // Distance between half beats
    private float halfBeatSpeed; // Speed half beats travel
    private int rangeStart = -9; // Left edge of UI
    private int rangeEnd = 9; // Right edge of UI
    private int rangeLen;
    private int numBeatDivisions; // Number of indicators on screen
    private GameObject[] allBeats; // Array to hold copied sprites
    private Vector3 ballOrigin; // Bottom of ball bounce
    private Vector3 ballTarget; // Peak of ball bounce
    private float[] timeElapsed; // Helps to calculate how much beat sprites should shift each frame
    private int iter = 0; // Help to make random golden notes
    private SpriteRenderer wholeBeatSR; // Used to check if a copy is a whole beat

    void Start()
    {
        ballOrigin = new Vector3(ball.transform.position.x, ball.transform.position.y, ball.transform.position.y);
        ballTarget = ballOrigin + new Vector3(0,1,0);

        rangeLen = rangeEnd - rangeStart;
        numBeatDivisions = rangeLen;

        wholeBeatSR = wholeBeat.GetComponent<SpriteRenderer>();

        wholeBeat.SetActive(false);
        halfBeat.SetActive(false);
        ball.SetActive(true);

        allBeats = new GameObject[numBeatDivisions];
        timeElapsed = new float[numBeatDivisions];

        halfDt = (60f / bpm) / 2f;
        halfBeatDist = (rangeEnd - rangeStart) / numBeatDivisions;
        halfBeatSpeed = halfBeatDist / halfDt;

        // Clone whole beat and half beat sprites to fill range
        // TO-DO: fix first beat offset, maybe check once audio time has passed offset before starting the ping pong in update, and add offset to Instantiate x position
        for (int i = 0; i < rangeLen; i++) {
            if (i % 2 != 0) {
                allBeats[i] = Instantiate(wholeBeat, new Vector3(rangeStart+i*halfBeatDist, wholeBeat.transform.position.y, -1), new Quaternion(0, 0, 0, 0));
            } else {
                allBeats[i] = Instantiate(halfBeat, new Vector3(rangeStart+i*halfBeatDist, halfBeat.transform.position.y, -1), new Quaternion(0, 0, 0, 0));
            }
            allBeats[i].SetActive(true);
        }

        audio.Play(); // Start music after setup
    }

    void Update()
    {
        // Move ball up and down with a period of 1 beat
        ball.transform.position = Vector3.Lerp(ballOrigin, ballTarget, Mathf.PingPong(Time.time * halfBeatSpeed, 1));

        // Move each half and whole beat sprite to the left
        for (int i = 0; i < rangeLen; i++) {
            GameObject currBeat = allBeats[i];

            // Wrap to right edge of range if sprite hits left edge
            if (currBeat.transform.position.x <= rangeStart) {
                // Reset color of any golden notes
                SpriteRenderer sr = currBeat.GetComponent<SpriteRenderer>();
                sr.color = Color.white;

                currBeat.transform.position = new Vector3(rangeEnd, halfBeat.transform.position.y, -2);

                // Insert random golden notes on whole beats
                if (iter % 17 == 0 && sr.sprite == wholeBeatSR.sprite) {
                    sr.color = Color.yellow;
                }
            }

            // Move beats left, calculated using speed and difference between current time and time in previous frame
            currBeat.transform.position = new Vector3(currBeat.transform.position.x - (halfBeatSpeed*(Time.time - timeElapsed[i])), wholeBeat.transform.position.y, -2);
            timeElapsed[i] = Time.time;
        }
        iter++;
    }
}
