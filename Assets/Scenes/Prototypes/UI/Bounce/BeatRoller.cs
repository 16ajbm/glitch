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
    private int rangeStart = -6; // Left edge of UI
    private int rangeEnd = 6; // Right edge of UI
    private int rangeLen = 12;
    private int numBeatDivisions = 12;
    private GameObject[] allBeats; // Array to hold copied sprites
    private Vector3 ballOrigin = new Vector3(0,1.2f,-2); // Bottom of ball bounce
    private Vector3 ballTarget = new Vector3(0,3,-2); // Peak of ball bounce
    private float[] timeElapsed; // Helps to calculate how much beat sprites should shift each frame 

    void Start()
    {
        wholeBeat.SetActive(false);
        halfBeat.SetActive(false);
        ball.SetActive(true);

        allBeats = new GameObject[numBeatDivisions];
        timeElapsed = new float[numBeatDivisions]; 

        halfDt = (60f / bpm) / 2f;
        halfBeatDist = (rangeEnd - rangeStart) / numBeatDivisions;
        halfBeatSpeed = halfBeatDist / halfDt;

        // Clone whole beat and half beat sprites to fill range
        for (int i = 0; i < rangeLen; i++) {
            if (i % 2 == 0) {
                allBeats[i] = Instantiate(wholeBeat, new Vector3(rangeStart+i*halfBeatDist,0,-1), new Quaternion(0,0,0,0));
            } else {
                allBeats[i] = Instantiate(halfBeat, new Vector3(rangeStart+i*halfBeatDist,0,-1), new Quaternion(0,0,0,0));
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
                currBeat.transform.position = new Vector3(rangeEnd, 0, -2);
            }

            // Move beats left, calculated using speed and difference between current time and time in previous frame
            currBeat.transform.position = new Vector3(currBeat.transform.position.x - (halfBeatSpeed*(Time.time - timeElapsed[i])), 0, -2);
            timeElapsed[i] = Time.time;
        }
    }
}
