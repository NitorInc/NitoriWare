using UnityEngine;
using System.Collections;

public class MicrogameNumber : MonoBehaviour
{

	public TextMesh text;

	void Start ()
	{
	
	}
	

	void Update ()
	{
	
	}

	public void increaseNumber()
	{
		if (text.text == "999")
			return;
		int number = int.Parse(text.text) + 1;
		if (number < 10)
			text.text = "00" + number.ToString();
		else if (number < 100)
			text.text = "0" + number.ToString();
		else
			text.text = number.ToString();

	}
}
