using UnityEngine;
using System.Collections;

public class NitoriScorePlaceholder : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private TextMesh highScoreMesh;
#pragma warning restore 0649

    public void setScore(int score)
	{
        setNumber(GetComponent<TextMesh>(), score);
	}

    public void setHighScore(int score)
    {
        setNumber(highScoreMesh, score);
    }

    void setNumber(TextMesh text, int score)
    {
        text.text = text.text.Substring(0, text.text.Length - 3);

        int number = score;
        text.text += number.ToString("D3");
    }
}
