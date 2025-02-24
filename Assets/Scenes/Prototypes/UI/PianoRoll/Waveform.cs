using UnityEngine;
using System.IO;
using System;

public class Waveform : MonoBehaviour
{
    // Audio
    public AudioSource audio;
    private float bpm = 130f; // BPM of music track
    private float dt; // time between beats
    public LineRenderer lineRenderer;
    private float beatSpeed; // used to calculate left shift of beats
    private float firstBeatOffset = 0.18f; // offset from t=0 of first beat
    private float[] bpmTimes; // timestamps of beats
    public GameObject beatSprite; // sprite for beat
    private GameObject[] allBeats; // copied beat sprites

    // Waveform
    private string filename = "Assets\\Scenes\\Prototypes\\UI\\PianoRoll\\points2.txt"; // waveform data
    private string[] lines; // temporary waveform data storage
    private float[] time; // waveform timestamps
    private float[] amplitude; // waveform amplitudes
    private int n; // length of waveform data

    // Misc
    public float xScale=5; // scale waveform in x direction
    public float yScale=10; // scale waveform in y direction
    private float indicatorX = -6f; // indicator xpos
    private float lastTime = 0f; // time since last left shift of beats


    void Start()
    {
        dt = 60f / bpm;
        GetData();

        lineRenderer.material.color = new Color(66 / 255f, 135 / 255f, 245f / 255f);
        lineRenderer.sortingOrder = 10;
        for (int i = 0; i < n; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(xScale*time[i]+indicatorX, yScale*amplitude[i]-0.5f*yScale, -2));
        }

        RenderBeats();

        beatSpeed = xScale;

        lastTime = Time.time;
        audio.Play();
    }

    void Update()
    {
        // Move waveform left at beatVelocity

        for (int i = 0; i < n; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(xScale*time[i]+indicatorX-beatSpeed*Time.time, yScale*amplitude[i]-0.5f*yScale, -2));
        }

        // Move beat sprites left at beatVelocity

        for (int i = 0; i < allBeats.Length; i++)
        {
            GameObject currBeat = allBeats[i];
            currBeat.transform.position = new Vector3(currBeat.transform.position.x - beatSpeed*(Math.Abs(Time.time - lastTime)), currBeat.transform.position.y, -2);
        }

        lastTime = Time.time;
    }

    void GetData()
    {
        // Load x and y data for wavefrom from file

        lines = File.ReadAllLines(filename);
        n = lines.Length;
        time = new float[n];
        amplitude = new float[n];

        for (int i = 0; i < n; i++) {
            time[i] = float.Parse(lines[i].Split(' ')[0]);
            amplitude[i] = float.Parse(lines[i].Split(' ')[1]);
        }

        lineRenderer.positionCount = n;

        // Calculate beat timestamps

        float maxTime = time[n-1];
        float cTime = 0f;
        int count = 0;
        while (cTime <= maxTime) {
            cTime += dt;
            count++;
        }

        bpmTimes = new float[count];
        for (int i = 0; i < count; i++) {
            bpmTimes[i] = i*dt+firstBeatOffset;
        }
    }

    void RenderBeats()
    {
        // Copy sprite for beat and place them on the graph

        allBeats = new GameObject[bpmTimes.Length];
        for (int i = 0; i < bpmTimes.Length; i++) {
            allBeats[i] = Instantiate(beatSprite, new Vector3(xScale*bpmTimes[i]+indicatorX,FindY(bpmTimes[i]),-2), new Quaternion(0,0,0,0));
            allBeats[i].SetActive(true);
        }
    }

    float FindY(float t)
    {
        // Find y position for a given beat timestamp.
        // Finds closest timestamp in time array and returns scaled amplitude

        float prevDist = Math.Abs(time[0] - t);
        for (int i = 1; i < time.Length; i++) {
            if (prevDist < Math.Abs(time[i] - t)) {
                return yScale*amplitude[i-1]-0.5f*yScale;
            }
            prevDist = Math.Abs(time[i] - t);
        }

        return 0f;
    }
}
