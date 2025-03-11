using UnityEngine;

public class Pattern
{
	public Pattern()
	{
	}

	public static int[] MakePattern(int len)
	{
		int[] pattern = new int[len];
		for (int i = 0; i < len; i++)
		{
			pattern[i] = Random.Range(1, 5);
		}
		return pattern;
	}
}
