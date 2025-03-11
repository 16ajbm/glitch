using UnityEngine;
using UnityEngine.UI;

public class Beat
{
	public string spriteName;
	private GameObject beatImage;
	private Image img;
	private RectTransform rt;

	private bool isGolden = false;

	public Beat(string name, GameObject refImage, Canvas parentCanvas)
	{
		spriteName = name;
		beatImage = Object.Instantiate(refImage);
		beatImage.transform.SetParent(parentCanvas.transform, false);
		img = beatImage.GetComponent<Image>();
		rt = beatImage.GetComponent<RectTransform>();
	}

	public void Activate()
	{
		beatImage.SetActive(true);
	}

	public void SetPos(float x, float y)
	{
		rt.anchoredPosition = new Vector2(x, y);
	}

	public float GetXPos()
	{
		return rt.anchoredPosition.x;
	}

	public float GetDistFromCenter()
	{
		return Mathf.Abs(rt.anchoredPosition.x);
	}

	public bool IsGolden()
	{
		return isGolden;
	}

	public void SetGolden()//bool isGolden)
	{
		Color yellow = new Color(1f, 1f, 0f);
		yellow.a = 1;

		img.color = yellow;
		//this.isGolden = isGolden;
		this.isGolden = true;
	}

	public void SetTurnColor()
	{
		Color turnCol = new Color(0f, 0f, 1f);
		turnCol.a = 1;

		img.color = turnCol;
		this.isGolden = false;
	}

	public void SetColor(Color col)
	{
		col.a = 1;
		img.color = col;
		this.isGolden = false;
	}

	public void ResetColor()
	{
		Color white = new Color(1f, 1f, 1f);
		white.a = 1;

		img.color = white;
		this.isGolden = false;
	}
}
