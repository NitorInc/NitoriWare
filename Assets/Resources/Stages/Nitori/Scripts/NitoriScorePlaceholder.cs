using UnityEngine;
using System.Collections;

public class NitoriScorePlaceholder : MonoBehaviour
{

	public void setScore(int score)
	{
		TextMesh text = GetComponent<TextMesh>();

		text.text = text.text.Substring(0, text.text.Length - 3);

		int number = score;
		text.text += number.ToString("D3");
	}
}
