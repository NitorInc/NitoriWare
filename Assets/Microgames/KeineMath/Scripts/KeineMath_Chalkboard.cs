using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeineMath_Chalkboard : MonoBehaviour {

    [Header("Number of terms")]
    [SerializeField]
    private int termCount = 2;

    [Header("Minimum size of terms")]
    [SerializeField]
    private int minTerm = 1;

    [Header("Maximum size of terms")]
    [SerializeField]
    private int maxTerm = 5;

    [Header("Number of answers to generate")]
    [SerializeField]
    private int answerCount = 3;

    [Header("Operation to use")]
    [SerializeField]
    private string operation = "+";

    [Header("List of sprites to pick from for icons")]
    [SerializeField]
    private List<Sprite> iconList = new List<Sprite>();

    private List<int> termList = new List<int>();

    private GameObject term1;
    private GameObject term2;
    private GameObject term3;
    private List<GameObject> terms = new List<GameObject>();
    private GameObject answer;
    private GameObject plusSymbol;
    private GameObject minusSymbol;
    private GameObject operationSymbol;
    private int correctAnswer;
    private bool answered = false;

    // Use this for initialization
    void Start () {
        //Enforcing displayable values
        if (minTerm < 1) minTerm = 1;
        if (maxTerm > 15) maxTerm = 15;
        if (termCount < 2) termCount = 2;
        if (termCount > 3) termCount = 3;
        if (answerCount > 3) answerCount = 3;
        if (answerCount < 1) answerCount = 1;

        //Setting up the microgame
        term1 = GameObject.Find("Term1");
        terms.Add(term1);
        term2 = GameObject.Find("Term2");
        terms.Add(term2);
        term3 = GameObject.Find("Term3");
        terms.Add(term3);
        plusSymbol = GameObject.Find("Plus");
        minusSymbol = GameObject.Find("Minus");
        answer = GameObject.Find("Answer");
        selectIcons();
        generateProblem();
        generateAnswers();
	}

    void selectIcons()
    {
        Sprite iconSelected;
        for (int i = 0; i < 3; i++)
        {
            iconSelected = iconList[Random.Range(0, iconList.Count)];
            iconList.Remove(iconSelected);
            terms[i].GetComponent<SpriteRenderer>().sprite = iconSelected;
        }
        iconSelected = iconList[Random.Range(0, iconList.Count)];
        iconList.Remove(iconSelected);
        answer.transform.Find("AnswerTerm").gameObject.GetComponent<SpriteRenderer>().sprite = iconSelected;
    }

    void generateProblem()
    {
        //IMPORTANT: The bottom term in the equation is term 0.
        //This is because we "generate up" (i.e. new terms are added on top)
        if (operation.Equals("+"))
        {
            //Generate each term between the minimum and maximum and add them up
            operationSymbol = plusSymbol;
            minusSymbol.transform.position = new Vector3(50, 0, 0); //Move the minus offscreen
            for (int i = 0; i < termCount; i++)
            {
                termList.Add(Random.Range(minTerm, (maxTerm + 1)));
            }
            correctAnswer = 0;
            for(int i = 0; i < terms.Count; i++)
            {
                if (i < termCount)
                {
                    terms[i].GetComponent<KeineMath_Term>().setValue(termList[i]);
                    correctAnswer += termList[i];
                } else
                {
                    terms[i].SetActive(false);
                }
            }
        } else if (operation.Equals("-"))
        {
            //Generate an answer, then two terms that will produce it with subtraction
            operationSymbol = minusSymbol;
            plusSymbol.transform.position = new Vector3(50, 0, 0); //Move the plus offscreen
            termCount = 2; //Subtraction with more than 2 terms is not supported.
            correctAnswer = Random.Range(1, maxTerm);
            int firstTerm;
            int secondTerm;
            if (maxTerm - correctAnswer == 1)
            {
                secondTerm = 1;
            } else
            {
                secondTerm = Random.Range(1, (maxTerm - correctAnswer));
            }
            firstTerm = correctAnswer + secondTerm;
            termList.Add(firstTerm);
            termList.Add(secondTerm);
            terms[0].GetComponent<KeineMath_Term>().setValue(secondTerm);
            terms[1].GetComponent<KeineMath_Term>().setValue(firstTerm);
            for (int i = 2; i < terms.Count; i++)
            {
                terms[i].SetActive(false);
            }
        } else
        {
            print("Invalid operation!");
        }
        //Move the operation symbol to the right of the terms
        float symbolOffset = (0.6f * Mathf.Max(termList.ToArray())) + (0.25f * Mathf.Floor(Mathf.Max(termList.ToArray()) / 5)) - 0.75f;
        float symbolx = operationSymbol.transform.position.x - symbolOffset;
        float symboly = operationSymbol.transform.position.y;
        operationSymbol.transform.position = new Vector3(symbolx, symboly, 0);
    }

    void generateAnswers()
    {
        int answerValue;
        int answerOffset;
        List<int> answerOffsets = new List<int>();
        for(int i = 0; i < answerCount; i++)
        {
            //Generate an amount to be wrong by. Note the first offset generated will be 0 (i.e. correct answer)
            int sign = (Random.Range(1, 3) * 2) - 3; //Generates 1 or -1
            if (correctAnswer == 1) sign = 1; //Answers of zero are not permitted
            answerOffsets.Add(i * sign);
        }
        for(int i = 1; i <= answerCount; i++)
        {
            //Pick a random offset, remove it, and apply it to the answer being generated
            answerOffset = answerOffsets[Random.Range(0, answerOffsets.Count)];
            answerOffsets.Remove(answerOffset);
            answerValue = correctAnswer + answerOffset;
            float newx = answer.transform.position.x + (3.33f * i) + 2f;
            float newy = answer.transform.position.y;
            Vector3 newposition = new Vector3(newx, newy, 0);
            GameObject newanswer = Object.Instantiate(answer, newposition, Quaternion.identity);
            newanswer.GetComponent<KeineMath_Answer>().value = answerValue;
        }
    }

    public void processAnswer(int answer)
    {
        if (!answered)
        {
            answered = true;
            if (answer == correctAnswer)
            {
                MicrogameController.instance.setVictory(victory: true, final: true);
            }
            else
            {
                MicrogameController.instance.setVictory(victory: false, final: true);
            }
        }
    }
}
