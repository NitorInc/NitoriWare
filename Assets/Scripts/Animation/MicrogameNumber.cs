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
		short number = short.Parse(text.text) + 1;
		text.text = number < 10 ? "00" + number.ToString() : number < 100 ? "0" + number.ToString() : number.ToString();
	}
}
