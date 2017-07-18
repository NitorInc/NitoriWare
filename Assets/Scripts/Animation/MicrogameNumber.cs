using UnityEngine;
using System.Collections;

public class MicrogameNumber : MonoBehaviour
{
	public static MicrogameNumber instance;

	public TextMesh text;

	void Awake()
	{
		instance = this;
	}

	public void increaseNumber()
	{
		if (text.text == "999")
			return;
		int number = int.Parse(text.text) + 1;
		text.text = number.ToString("D3");
	}

    public void decreaseNumber()
    {
        if (text.text == "999")
            return;
        int number = int.Parse(text.text) - 1;
        text.text = number.ToString("D3");
    }

    public int getNumber()
    {
        return int.Parse(text.text);
    }

	public void resetNumber()
	{
		text.text = "000";
	}
}
