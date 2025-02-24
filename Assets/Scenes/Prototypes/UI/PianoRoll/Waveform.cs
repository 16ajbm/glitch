using UnityEngine;
using System.IO;
using System;

public class Waveform : MonoBehaviour
{
    public AudioSource audio;
    public LineRenderer lineRenderer;
    public GameObject indicator;
    private string filename = "Assets\\Scenes\\Prototypes\\UI\\PianoRoll\\points2.txt";
    private string[] lines;
    private float[] time;
    private float[] amplitude;
    private int n;
    private float bpm = 130f;
    private float dt;
    public float xScale=5;
    public float yScale=10;
    private float indicatorX = -6f;
    private float indicatorY = 0f;
    private float maxTime;
    private float[] bpmTimes;
    public GameObject beatSprite;
    private GameObject[] allBeats;
    private float firstBeatOffset = 0.18f;
    private float timeElapsed = 0f;
    private float lastTime = 0f;
    private float beatSpeed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

        //beatSpeed = (xScale / dt) / 2;
        beatSpeed = xScale;

        timeElapsed = Time.time;
        lastTime = Time.time;
        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // Move plot and beats left
        for (int i = 0; i < n; i++)
        {
            //lineRenderer.SetPosition(i, new Vector3(xScale*time[i]+indicatorX-beatSpeed*(Math.Abs(Time.time-timeElapsed)), yScale*amplitude[i]-0.5f*yScale, 0));
            lineRenderer.SetPosition(i, new Vector3(xScale*time[i]+indicatorX-beatSpeed*Time.time, yScale*amplitude[i]-0.5f*yScale, -2));
        }

        for (int i = 0; i < allBeats.Length; i++)
        {
            GameObject currBeat = allBeats[i];
            currBeat.transform.position = new Vector3(currBeat.transform.position.x - beatSpeed*(Math.Abs(Time.time - lastTime)), currBeat.transform.position.y, -2);
        }

        lastTime = Time.time;
        timeElapsed += Time.time;
    }

    void GetData()
    {
        lines = File.ReadAllLines(filename);
        n = lines.Length;
        time = new float[n];
        amplitude = new float[n];

        for (int i = 0; i < n; i++) {
            time[i] = float.Parse(lines[i].Split(' ')[0]);
            amplitude[i] = float.Parse(lines[i].Split(' ')[1]);
        }

        lineRenderer.positionCount = n;

        maxTime = time[n-1];
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
        // Copy sprite for beat at each position xScale*bpmTimes[i]+indicatorX

        allBeats = new GameObject[bpmTimes.Length];
        for (int i = 0; i < bpmTimes.Length; i++) {
            allBeats[i] = Instantiate(beatSprite, new Vector3(xScale*bpmTimes[i]+indicatorX,FindY(bpmTimes[i]),-2), new Quaternion(0,0,0,0));
            allBeats[i].SetActive(true);
        }
    }

    float FindY(float t)
    {
        // Find y height given bpm time
        /*int distIndex = 0;
        float dist = 1000;
        while (distIndex < bpmTimes.Length && dist > time - bpmTimes[distIndex])*/

        float prevDist = Math.Abs(time[0] - t);
        for (int i = 1; i < time.Length; i++) {
            if (prevDist < Math.Abs(time[i] - t)) {
                return yScale*amplitude[i-1]-0.5f*yScale;
            }
            prevDist = Math.Abs(time[i] - t);
        }

        return 0f;
    }

    /*void DrawGraph(float[] x, float[] y)
    {
        // Draw LineRenderer graph given x and y
        for (int i = 0; i < n; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(xScale*time[i]+indicatorX, yScale*amplitude[i]-0.5f*yScale, 0));
        }
    }*/
}
