using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeineMath_Answer : MonoBehaviour {

    public int value;
    private int iconsPerRow = 5;
    private bool displayed;
    private GameObject answerTerm;
    private GameObject chalkboard;

	// Use this for initialization
	void Start () {
        answerTerm = transform.Find("AnswerTerm").gameObject;
        chalkboard = GameObject.Find("Chalkboard");
        displayValue();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void displayValue()
    {
        //Add new instances of the internal object until the value is reached
        //NOTE: Should not be called more than once
        for (int i = 1; i < value; i++)
        {
            float newx = answerTerm.transform.position.x + (0.55f * (i % iconsPerRow));
            float newy = answerTerm.transform.position.y - (0.8f * Mathf.Floor(i / iconsPerRow));
            Vector3 newposition = new Vector3(newx, newy, 0);
            Object.Instantiate(answerTerm, newposition, Quaternion.identity);
        }
    }

    private void OnMouseDown()
    {
        chalkboard.GetComponent<KeineMath_Chalkboard>().processAnswer(value, transform.position);
    }
}
