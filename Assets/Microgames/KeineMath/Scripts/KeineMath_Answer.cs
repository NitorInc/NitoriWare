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
    public Color answerColor;
    public bool isCorrect = false;
    private bool displayed;
    private bool circled = false;
    private bool crossed = false;
    [SerializeField] private GameObject answerTerm;
    [SerializeField] private SpriteRenderer bgsprite;
    [SerializeField] private GameObject chalkboard;
    [SerializeField] private GameObject correctSymbol;
    [SerializeField] private GameObject hundredSymbol;
    [SerializeField] private GameObject incorrectSymbol;

    // Use this for initialization
    void Start () {
        //Handle background and term initialization
        GetComponent<SpriteRenderer>().color = answerColor;
        answerTerm.GetComponent<SpriteRenderer>().color = answerColor;
        if (bgsprite.color.a == 1)
        {
            editBGAlpha(0.25f); //Initialize only if it hasn't already been set.
        }
        displayValue();
    }

    private void Update()
    {
        if (MicrogameController.instance.getVictoryDetermined() && MicrogameController.instance.getVictory() && isCorrect && !circled)
        {
            circled = true;
            Instantiate(correctSymbol, transform.position, Quaternion.identity);
            Instantiate(hundredSymbol, new Vector3(transform.position.x, transform.position.y - 2.4f, 0), Quaternion.identity);
        }
        if (MicrogameController.instance.getVictoryDetermined() && !MicrogameController.instance.getVictory() && !isCorrect && !crossed)
        {
            crossed = true;
            Instantiate(incorrectSymbol, transform.position, Quaternion.identity);
        }
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
        answerTerm.transform.localScale /= transform.localScale.x;
    }

    private void OnMouseDown()
    {
        chalkboard.GetComponent<KeineMath_Chalkboard>().processAnswer(value);
    }

    private void OnMouseEnter()
    {
        if (!MicrogameController.instance.getVictoryDetermined())
        {
            editBGAlpha(0.38f);
        }
    }

    private void OnMouseExit()
    {
        if (!MicrogameController.instance.getVictoryDetermined())
        {
            editBGAlpha(0.25f);
        }
    }

    private void editBGAlpha(float newAlpha)
    {
        Color temp = bgsprite.color;
        temp.a = newAlpha;
        bgsprite.color = temp;
    }
}
