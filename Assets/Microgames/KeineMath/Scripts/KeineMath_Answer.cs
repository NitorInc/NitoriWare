using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeineMath_Answer : MonoBehaviour {

    [Header("Maximum icons per row")] //Number of icons that show up in each row of the answer box
    [SerializeField]
    private int iconsPerRow = 4;

    [Header("Space between icons (horizontal)")] //The gap between one icon and the next horizontally
    [SerializeField]
    private float horizoncalIconGap = 0.55f;

    [Header("Space between icons (vertical)")] //The gap between rows of icons vertically
    [SerializeField]
    private float verticalIconGap = 0.8f;

    public int value;
    private bool displayed;
    private GameObject answerTerm;
    private GameObject bg;
    private GameObject chalkboard;
    private Color answerColor;

	// Use this for initialization
	void Start () {
        //Find scene objects
        answerTerm = transform.Find("AnswerTerm").gameObject;
        chalkboard = GameObject.Find("Chalkboard");
        bg = transform.Find("AnswerBG").gameObject;

        //Generate random HSV color and apply it to the answer box
        answerColor = Color.HSVToRGB(Random.Range(0f, 1f), 0.35f, 1);
        GetComponent<SpriteRenderer>().color = answerColor;

        //Handle background and term initialization     
        answerTerm.GetComponent<SpriteRenderer>().color = answerColor;
        editBGAlpha(0.25f);
        displayValue();
    }

    public void displayValue()
    {
        //Add new instances of the internal object until the value is reached
        //NOTE: Should not be called more than once
        for (int i = 1; i < value; i++)
        {
            float newx = answerTerm.transform.position.x + (horizoncalIconGap * (i % iconsPerRow));
            float newy = answerTerm.transform.position.y - (verticalIconGap * Mathf.Floor(i / iconsPerRow));
            Vector3 newposition = new Vector3(newx, newy, 0);
            GameObject newObject = Object.Instantiate(answerTerm, newposition, Quaternion.identity);
            //newObject.GetComponent<SpriteRenderer>().color = answerColor;
        }
    }

    private void OnMouseDown()
    {
        chalkboard.GetComponent<KeineMath_Chalkboard>().processAnswer(value, transform.position);
    }

    private void OnMouseEnter()
    {
        if (!MicrogameController.instance.getVictoryDetermined()) editBGAlpha(0.5f);
    }

    private void OnMouseExit()
    {
        if (!MicrogameController.instance.getVictoryDetermined()) editBGAlpha(0.25f);
    }

    private void editBGAlpha(float newAlpha)
    {
        Color temp = bg.GetComponent<SpriteRenderer>().color;
        temp.a = newAlpha;
        bg.GetComponent<SpriteRenderer>().color = temp;
    }
}
