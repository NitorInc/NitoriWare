using UnityEngine;
using System.Collections;

public class NitoriScorePlaceholder : MonoBehaviour
{

	public void setScore(int score)
	{
		TextMesh text = GetComponent<TextMesh>();

		text.text = text.text.Substring(0, text.text.Length - 3);

		int number = score;
		if (number < 10)
			text.text += "00" + number.ToString();
		else if (number < 100)
			text.text += "0" + number.ToString();
		else
			text.text += number.ToString();
	}
}
