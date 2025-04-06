using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class BeatRoller : MonoBehaviour
{
	#region Music
	new public AudioSource audio;
	[Range(0f, 1f), SerializeField] float volume = 1f;
	public float bpm = 130f;
	public float firstBeatOffset = 0.15f;
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
	private float lastAudioTime;
	private bool audioStarted = false;
	private float ballTime = 0f;
	private float timeStarted;
	#endregion

	#region Pattern
	public TMP_Text timer;
	public GameObject timerBox;
	public GameObject scoreBox;
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
	private float maxScore = 0f;
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

	#region Tutorial
    private bool isTutorial = false;
	private int tipIndex = 0;
	public GameObject welcome;
	public GameObject characters;
	public GameObject rhythm;
	public GameObject combatMenu;
	public GameObject damage;
	public GameObject lightAttack;
	public GameObject patternDisp;
	public GameObject minigame;
	#endregion

	void Start()
	{
        if (SceneManager.GetActiveScene().name == "Tutorial") isTutorial = true;

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
				allBeats[i].SetPos(rangeStart + i * halfBeatDist, wholeBeat.GetComponent<Image>().GetComponent<RectTransform>().anchoredPosition.y);
			}
			else
			{
				allBeats[i] = new Beat("HalfBeat", halfBeat, parentCanvas);
				allBeats[i].SetPos(rangeStart + i * halfBeatDist, halfBeat.GetComponent<Image>().GetComponent<RectTransform>().anchoredPosition.y);
			}

			allBeats[i].Activate();
		}

		pDisp = new PatternDisplay(patternBackground, timerBox, parentCanvas, arrow, timer);

		timeStarted = Time.time;
		ballTime = 0f;
	}

	void Update()
	{
		if (Time.time - timeStarted < 2) return;

		if (audio != null)
			audio.volume = volume;

		if (!isTutorial && !audioStarted) 
		{
			audio.Play();
			lastAudioTime = Time.time;
			audioStarted = true;
		}

        if (isTutorial) {
		    if (tipIndex < 6)
		    {
		    	Time.timeScale = 0f;
		    }

		    if (Input.GetKeyDown(KeyCode.Return)) 
		    {
		    	if (tipIndex == 5)
		    	{
		    		lightAttack.SetActive(false);
		    		Time.timeScale = 1f;
		    		audio.Play();
					lastAudioTime = Time.time;
		    	}
		    	else if (tipIndex == 7)
		    	{
		    		minigame.SetActive(false);
		    		Time.timeScale = 1f;
		    		audio.Play();
		    	}
		    	tipIndex++;
		    }

		    switch (tipIndex)
		    {
		    	case 0:
		    		welcome.SetActive(true);
		    		break;
		    	case 1:
		    		welcome.SetActive(false);
		    		characters.SetActive(true);
		    		break;
		    	case 2:
		    		characters.SetActive(false);
		    		rhythm.SetActive(true);
		    		break;
		    	case 3:
		    		rhythm.SetActive(false);
		    		combatMenu.SetActive(true);
		    		break;
		    	case 4:
		    		combatMenu.SetActive(false);
		    		damage.SetActive(true);
		    		break;
		    	case 5:
		    		damage.SetActive(false);
		    		lightAttack.SetActive(true);
		    		break;
		    	case 6:
		    		if (patternHasBeenMade)
		    		{
		    			patternDisp.SetActive(true);
		    		}
		    		break;
		    	case 7:
		    		patternDisp.SetActive(false);
		    		minigame.SetActive(true);
		    		break;
		    }
        }

		if (audio.time <= firstBeatOffset) return;

		if (makePattern && !patternHasBeenMade)
		{
			patternHasBeenMade = true;
			pattern = Pattern.MakePattern(patternLen);
			Debug.Log("Pattern: " + string.Join(", ", pattern));
			allArrows = pDisp.Display(pattern);

			// Important for tutorial
			if (isTutorial && tipIndex == 6)
			{
				Time.timeScale = 0f;
				audio.Pause();
			}
		}

		if (makePattern && !turnStarted && Time.timeScale == 1f)
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
				scoreBox.SetActive(true);
			}
			timer.text = timerVal.ToString("F2");
		}

		float currAudioTime = Time.time;
		if ((isTutorial && tipIndex > 5) || !isTutorial) ballTime += currAudioTime - lastAudioTime;
		ball.GetComponent<Image>().GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(ballOrigin, ballTarget, Mathf.PingPong(ballTime / halfBeatDelta, 1));
		ShiftBeats(currAudioTime);
		frameCount++;

		if (turnEnded) return;

		if (turnStarted && patternIndex == patternLen)
		{
			turnStarted = false;
			turnEnded = true;
			madeFirstMove = false;
			patternIndex = 0;
			numGoldenNotes = 0;
			scoreMultiplier = 1.0f;
			numBlueNotes = 0;
			finalScoringDone = true;
			goldenNoteCounterDisplay.gameObject.SetActive(false);
			multiplierDisplay.gameObject.SetActive(false);
			scoreBox.SetActive(false);
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
			}
			else
			{
				CheckCorrectness(false);
			}
		}
		else if (turnStarted && Input.GetKeyDown("right"))
		{
			madeMoveThisRound = true;
			if (pattern[patternIndex] == 2)
			{
				CheckCorrectness(true);
			}
			else
			{
				CheckCorrectness(false);
			}
		}
		else if (turnStarted && Input.GetKeyDown("down"))
		{
			madeMoveThisRound = true;
			if (pattern[patternIndex] == 3)
			{
				CheckCorrectness(true);
			}
			else
			{
				CheckCorrectness(false);
			}
		}
		else if (turnStarted && Input.GetKeyDown("left"))
		{
			madeMoveThisRound = true;
			if (pattern[patternIndex] == 4)
			{
				CheckCorrectness(true);
			}
			else
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

	void ShiftBeats(float currAudioTime)
	{
		float deltaAudioTime = currAudioTime - lastAudioTime;

		if (deltaAudioTime > 0)
		{
			float moveDist = deltaAudioTime * halfBeatSpeed;
			
			for (int i = 0; i < numBeatDivisions; i++)
			{
				Beat currBeat = allBeats[i];
				float newXPos = currBeat.GetXPos() - moveDist;
				
				if (newXPos < rangeStart)
				{
					if (turnStarted && numBlueNotes < patternLen)
					{
						if (frameCount % 17 == 0 && currBeat.spriteName == "WholeBeat")
						{
							currBeat.SetGolden();
						}
						else
						{
							currBeat.SetTurnColor();
						}
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

					newXPos = rangeEnd - (rangeStart - newXPos);
				}

				if (currBeat.spriteName == "WholeBeat")
				{
					currBeat.SetPos(newXPos, wholeBeat.GetComponent<Image>().GetComponent<RectTransform>().anchoredPosition.y);
				}
				else
				{
					currBeat.SetPos(newXPos, halfBeat.GetComponent<Image>().GetComponent<RectTransform>().anchoredPosition.y);
				}
			}
			lastAudioTime = currAudioTime;
		}
	}

	public void Score(int len, System.Action<(float, float)> onScoreCalculated)
	{
		score = 0;
		makePattern = true;
		patternLen = len;
		turnEnded = false;
		finalScoringDone = false;
		// Resetting this is critical to ensure the display is triggered each turn
		patternHasBeenMade = false;

		// Setting max score
		maxScore = (int)((float)defaultScore * (float)patternLen * (1 + ((float)patternLen - 1)/20));

		StartCoroutine(WaitForFinalScore(onScoreCalculated));
	}

	void CheckCorrectness(bool keyCorrect)
	{

		if (keyCorrect && allBeats[beatIndex].GetDistFromCenter() <= tolerance)
		{
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
			scoreMultiplier = 1;
			return;
		}
	}

	private IEnumerator WaitForFinalScore(System.Action<(float, float)> onScoreCalculated)
	{
		while (!finalScoringDone)
		{
			yield return null;
		}

		onScoreCalculated?.Invoke((score, maxScore));
	}
}
