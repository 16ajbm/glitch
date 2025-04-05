using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PatternDisplay
{
	public GameObject displayBox;
	public GameObject timerBox;
	public float displayWidth;
	public float displayHeight;

	private float padding = 100f;
	public GameObject arrowSprite;
	public Canvas parentCanvas;
	public TMP_Text timer;

	public PatternDisplay(GameObject box, GameObject tBox, Canvas canvas, GameObject arrow, TMP_Text timer)
	{
		displayBox = box;
		timerBox = tBox;
		parentCanvas = canvas;
		arrowSprite = arrow;
		this.timer = timer;

		displayWidth = displayBox.GetComponent<RectTransform>().rect.width;
		displayHeight = displayBox.GetComponent<RectTransform>().rect.height;
	}

	public GameObject[] Display(int[] pattern)
	{
		displayBox.SetActive(true);
		timerBox.SetActive(true);

		GameObject[] allArrows = new GameObject[pattern.Length];

		float arrowWidth = (displayWidth - (pattern.Length + 1) * padding) / pattern.Length;
		
		float currentX = displayBox.GetComponent<Image>().GetComponent<RectTransform>().anchoredPosition.x - displayWidth / 2 + padding + arrowWidth / 2;

		for (int i = 0; i < pattern.Length; i++)
		{
			allArrows[i] = Object.Instantiate(arrowSprite);
			GameObject arrow = allArrows[i];
			arrow.transform.SetParent(parentCanvas.transform, false);
			Image img = arrow.GetComponent<Image>();
			RectTransform rt = img.GetComponent<RectTransform>();
			rt.anchoredPosition = new Vector2(currentX, displayBox.GetComponent<Image>().GetComponent<RectTransform>().anchoredPosition.y);
			arrow.transform.rotation = Quaternion.Euler(0, 0, -180 - 90 * pattern[i]);

			arrow.SetActive(true);
			
			currentX += arrowWidth + padding;
		}

		timer.gameObject.SetActive(true);

		return allArrows;
	}

	public void Cleanup(GameObject[] allArrows)
	{
		for (int i = 0; i < allArrows.Length; i++)
		{
			Object.Destroy(allArrows[i]);
			displayBox.SetActive(false);
			timerBox.SetActive(false);
			timer.text = "10.00";
			timer.gameObject.SetActive(false);
		}
	}
}
