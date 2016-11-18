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
		text.text = number.ToString("D3");
	}
}
