using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BeatRoller : MonoBehaviour
{
	#region Music
	new public AudioSource audio;
	[Range(0f, 1f), SerializeField] float volume = 1f;
	public float bpm = 130f;
	public float firstBeatOffset = 0.55f;
	#endregion

	#region UI
	public Canvas parentCanvas;
	private int rangeStart = -1920 / 2;
	private int rangeEnd = 1920 / 2;
	private int rangeLen = 1920;
	private int numBeatDivisions = 16;
	private Vector2 ballOrigin;
	private Vector2 ballTarget;
	#endregion

	#region Sprites
	public GameObject ball;
	public GameObject wholeBeat;
	public GameObject halfBeat;
	private Beat[] allBeats;
	#endregion

	#region Calculations
	private float halfBeatDelta;
	private float halfBeatDist;
	private float halfBeatSpeed;
	private int frameCount;
	#endregion

	#region Pattern
	public TMP_Text timer;
    public TMP_Text goldenNoteCounterDisplay;
    public TMP_Text multiplierDisplay;
	private float timerVal = 10f;
	public GameObject patternBackground;
	public GameObject arrow;
	private GameObject[] allArrows;
	private PatternDisplay pDisp;
	private bool makePattern = false;
	private bool patternHasBeenMade = false;
	private int patternLen;
	private int[] pattern;
	private bool turnStarted;
	private bool turnEnded;
	private int patternIndex = 0;
	private float score = 0f;
	private float scoreMultiplier = 1f;
	private int numGoldenNotes = 0;
	private int beatIndex;
	public float tolerance = 75;
	public float defaultScore = 50f;
	public float multiplierIncrease = 0.1f;
	public float goldenNoteMultiplier = 3;
	private bool madeFirstMove = false;
	private bool madeMoveThisRound = false;
	private int numBlueNotes = 0;
	private bool finalScoringDone = false;
	#endregion

	void Start()
	{
		wholeBeat.SetActive(false);
		halfBeat.SetActive(false);
		ball.SetActive(true);

		ballOrigin = ball.GetComponent<Image>().GetComponent<RectTransform>().anchoredPosition;
		ballTarget = ballOrigin + new Vector2(0, 150);

		allBeats = new Beat[numBeatDivisions];

		halfBeatDelta = (60f / bpm) / 2;
		halfBeatDist = rangeLen / numBeatDivisions;
		halfBeatSpeed = halfBeatDist / halfBeatDelta;

		for (int i = 0; i < numBeatDivisions; i++)
		{
			if (i % 2 == 0)
			{
				allBeats[i] = new Beat("WholeBeat", wholeBeat, parentCanvas);
			} else
			{
				allBeats[i] = new Beat("HalfBeat", halfBeat, parentCanvas);
			}

			allBeats[i].SetPos(rangeStart + i * halfBeatDist, wholeBeat.GetComponent<Image>().GetComponent<RectTransform>().anchoredPosition.y);

			allBeats[i].Activate();
		}

		pDisp = new PatternDisplay(patternBackground, parentCanvas, arrow, timer);

		audio.Play();
	}

	void Update()
	{
		if (audio != null)
            audio.volume = volume;

		if (audio.time <= firstBeatOffset) return;

		ball.GetComponent<Image>().GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(ballOrigin, ballTarget, Mathf.PingPong(Time.time / halfBeatDelta, 1));

		if (makePattern && !patternHasBeenMade)
		{
			patternHasBeenMade = true;
			pattern = Pattern.MakePattern(patternLen);
			Debug.Log("Pattern: " + string.Join(", ", pattern));
			allArrows = pDisp.Display(pattern);
		}

		if (makePattern && !turnStarted)
		{
			timerVal -= Time.deltaTime;
			if (timerVal <= 0)
			{
				timerVal = 10f;
				pDisp.Cleanup(allArrows);
				makePattern = false;
				turnStarted = true;
                goldenNoteCounterDisplay.gameObject.SetActive(true);
                multiplierDisplay.gameObject.SetActive(true);
			}
			timer.text = timerVal.ToString("F2");
		}

		ShiftBeats();
		frameCount++;

		if (turnEnded) return;

		if (turnStarted && patternIndex == patternLen)
		{
			turnStarted = false;
			turnEnded = true;
			madeFirstMove = false;
			patternIndex = 0;
			numGoldenNotes = 0;
			numBlueNotes = 0;
			//Debug.Log($"Final Score: {score}");
			finalScoringDone = true;
            goldenNoteCounterDisplay.gameObject.SetActive(false);
            multiplierDisplay.gameObject.SetActive(false);
			return;
		}

        goldenNoteCounterDisplay.text = "Golden Notes: " + numGoldenNotes.ToString();
        multiplierDisplay.text = "Multiplier: " + scoreMultiplier.ToString("F1");

		if (turnStarted && Input.GetKeyDown("up"))
		{
			madeMoveThisRound = true;
			if (pattern[patternIndex] == 1)
			{
				CheckCorrectness(true);
			} else
			{
				CheckCorrectness(false);
			}
		} else if (turnStarted && Input.GetKeyDown("right"))
		{
			madeMoveThisRound = true;
			if (pattern[patternIndex] == 2)
			{
				CheckCorrectness(true);
			} else
			{
				CheckCorrectness(false);
			}
		} else if (turnStarted && Input.GetKeyDown("down"))
		{
			madeMoveThisRound = true;
			if (pattern[patternIndex] == 3)
			{
				CheckCorrectness(true);
			} else
			{
				CheckCorrectness(false);
			}
		} else if (turnStarted && Input.GetKeyDown("left"))
		{
			madeMoveThisRound = true;
			if (pattern[patternIndex] == 4)
			{
				CheckCorrectness(true);
			} else
			{
				CheckCorrectness(false);
			}
		}

		if (turnStarted && madeFirstMove && madeMoveThisRound)
		{
			patternIndex++;
			beatIndex = (beatIndex + 2) % numBeatDivisions;
			madeMoveThisRound = false;
		}

	}

	void ShiftBeats()
	{
		for (int i = 0; i < numBeatDivisions; i++)
		{
			Beat currBeat = allBeats[i];

			if (currBeat.GetXPos() < rangeStart)
			{
				if (turnStarted && numBlueNotes < patternLen)
				{
					currBeat.SetTurnColor();
					if (currBeat.spriteName == "WholeBeat")
					{
						numBlueNotes++;
						if (numBlueNotes == 1)
						{
							beatIndex = i;
							madeFirstMove = true;
						}
					}
				}
				else
				{
					currBeat.ResetColor();
				}

				currBeat.SetPos(rangeEnd, wholeBeat.GetComponent<Image>().GetComponent<RectTransform>().anchoredPosition.y);

				if (frameCount % 17 == 0 && currBeat.spriteName == "WholeBeat")
				{
					currBeat.SetGolden();
				}
			}

			currBeat.SetPos(currBeat.GetXPos() - halfBeatSpeed * Time.deltaTime, wholeBeat.GetComponent<Image>().GetComponent<RectTransform>().anchoredPosition.y);
		}
	}

	public void Score(int len, System.Action<float> onScoreCalculated)
	{
		score = 0;
		makePattern = true;
		patternLen = len;
		turnEnded = false;
		finalScoringDone = false;

		StartCoroutine(WaitForFinalScore(onScoreCalculated));
	}

	void CheckCorrectness(bool keyCorrect)
	{

		if (keyCorrect && allBeats[beatIndex].GetDistFromCenter() <= tolerance)
		{
			Debug.Log($"HIT: {patternIndex}");
			if (allBeats[beatIndex].IsGolden())
			{
				numGoldenNotes++;
				score += defaultScore * goldenNoteMultiplier * scoreMultiplier;
			}
			else
			{
				score += defaultScore * scoreMultiplier;
			}
			allBeats[beatIndex].SetColor(new Color(0f, 1f, 0f));
			scoreMultiplier += multiplierIncrease;
		}
		else
		{
			Debug.Log($"MISS: {patternIndex}");
			scoreMultiplier = 1;
			return;
		}
	}

	private IEnumerator WaitForFinalScore(System.Action<float> onScoreCalculated)
	{
		while (!finalScoringDone)
		{
			yield return null;
		}

		Debug.Log($"WaitForFinalScore: {score}");
		onScoreCalculated?.Invoke(score);
	}
}
