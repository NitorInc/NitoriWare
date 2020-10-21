using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeineMath_Chalkboard : MonoBehaviour {

    [Header("Number of terms")] //Number of terms in equation. Can be 2 or 3.
    [SerializeField]            //Theoretically microgame can be modified to allow 4.
    private int termCount = 2;

    [Header("Minimum size of terms")] //Smallest term allowed to be generated.
    [SerializeField]
    private int minTerm = 1;

    [Header("Maximum size of terms")] //Largest term allowed to be generated.
    [SerializeField]                  //Note that terms larger than 6 will overlap Keine and mess up the layout.
    private int maxTerm = 5;

    [Header("Number of answers to generate")] //Number of answers to generate.
    [SerializeField]                          //Should be left at 3 without layout changes.
    private int answerCount = 3;

    [Header("Operation to use")] //Whether to add or subtract.
    [SerializeField]
    private string operation = "+";

    [Header("List of sprites to pick from for icons")] //Exactly what it says on the tin.
    [SerializeField]
    private List<Sprite> iconList = new List<Sprite>();

    [Header("Sound to play on correct answer")] //I wonder what this does?
    [SerializeField]
    private AudioClip correctClip;

    [Header("Sound to play on incorrect answer")] //it_is_a_mystery.mp3
    [SerializeField]
    private AudioClip incorrectClip;

    [Header("How close answer hues are permitted to be to each other")] //NOTE: Large values may cause this to become non-functional
    [SerializeField]
    private float answerHueTolerance = 0.15f;

    [Header("Value for answer color saturation")] //NOTE: Large values may cause this to become non-functional
    [SerializeField]
    private float answerColorSaturation = 0.25f;

    private List<int> termList = new List<int>();

    [SerializeField] private GameObject term1;
    [SerializeField] private GameObject term2;
    [SerializeField] private GameObject term3;
    private List<GameObject> terms = new List<GameObject>();
    [SerializeField] private GameObject answer;
    [SerializeField] private GameObject plusSymbol;
    [SerializeField] private GameObject minusSymbol;
    private GameObject operationSymbol;
    private int correctAnswer;
    private bool answered = false;

    [SerializeField] private GameObject keineAnimator;
    [SerializeField] private GameObject cheeringCrowd;

    // Use this for initialization
    void Start () {
        //Setting up the microgame
        terms.Add(term1);
        terms.Add(term2);
        terms.Add(term3);

        //Setting up Keine, the correct/incorrect symbols, and the cheering crowd.
        //They stay invisible or offscreen until an answer is chosen.
        keineAnimator.GetComponent<SpriteRenderer>().enabled = false;
        cheeringCrowd.SetActive(false);

        //These functions randomize the microgame
        selectIcons();
        generateProblem();
        generateAnswers();
	}

    void selectIcons()
    {
        //This function allocates a unique icon to each term in the equation, and to the term used for answers
        Sprite iconSelected;
        for (int i = 0; i < terms.Count; i++)
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
            bool allones = true;
            for (int i = 0; i < termCount; i++)
            {
                termList.Add(Random.Range(minTerm, (maxTerm + 1)));
                if (termList[i] != 1)
                {
                    allones = false;
                }
            }
            //We don't want all 1s because it ruins the answer-generating algorithm and also is boring.
            if (allones)
            {
                termList[Random.Range(0, termCount)] += 1;
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
        float symbolOffset = (0.6f * Mathf.Max(termList.ToArray())) + (0.17f * Mathf.Floor(Mathf.Max(termList.ToArray()) / 5)) - 0.75f;
        float symbolx = operationSymbol.transform.position.x - symbolOffset;
        float symboly = operationSymbol.transform.position.y;
        operationSymbol.transform.position = new Vector3(symbolx, symboly, 0);
        operationSymbol.GetComponent<Vibrate>().resetOffset();
    }

    void generateAnswers()
    {
        List<float> hues = new List<float>();
        int answerValue;
        int answerOffset;
        List<int> answerOffsets = new List<int>();
        for(int i = 0; i < answerCount; i++)
        {
            //Generate an amount to be wrong by. Note the first offset generated will be 0 (i.e. correct answer)
            int sign = (Random.Range(1, 3) * 2) - 3; //Generates 1 or -1
            if (correctAnswer == 1 || (correctAnswer == 2 && i >= 2))
            {
                sign = 1; //Answers of zero are not permitted
            }
            answerOffset = i * sign;
            if (i != 0 && 
                (correctAnswer + (i * sign)) == terms[0].GetComponent<KeineMath_Term>().value + terms[1].GetComponent<KeineMath_Term>().value) {
                //Special case: An incorrect answer equals the sum of the two terms.
                //We modify the answer in this case because if the problem is subtraction...
                //...we don't want the player adding by mistake because the answer to the addition is there.
                //Process: If the above case where the sign HAS to be positive is present, AND the offset is positive...
                //...increment the offset since we can't make it negative without causing problems.
                //Otherwise, flip the sign as either it's allowed to be negative or it's already negative.
                if ((correctAnswer == 1 || (correctAnswer == 2 && i >= 2)) && answerOffset > 0)
                {
                    answerOffset++;
                }
                else
                {
                    answerOffset *= -1;
                }
            }
            answerOffsets.Add(answerOffset); //In normal circumstances first wrong answer is off by 1, second wrong answer is off by 2
        }
        for(int i = 1; i <= answerCount; i++)
        {
            //Pick a random offset, remove it, and apply it to the answer being generated
            answerOffset = answerOffsets[Random.Range(0, answerOffsets.Count)];
            answerOffsets.Remove(answerOffset);
            answerValue = correctAnswer + answerOffset;
            float newx = answer.transform.position.x + (3.85f * i) + 1f; //Values in here determine answer positioning
            float newy = answer.transform.position.y;
            Vector3 newposition = new Vector3(newx, newy, 0);
            GameObject newanswer = Object.Instantiate(answer, newposition, Quaternion.identity);
            newanswer.GetComponent<KeineMath_Answer>().value = answerValue;
            if (answerOffset == 0)
            {
                newanswer.GetComponent<KeineMath_Answer>().isCorrect = true;
            }

            //Set the color of this answer here.
            //Procedure: Generate random hue, iterate through existing hues, nudge hue up to get away from them.
            //If we nudge enough times to get back to where we started, assume we're in an infinite loop and bail.
            //NOTE: We don't nudge in the most efficient direction (i.e. up if already higher, down if already lower)
            //because this can lead to the third answer ping-ponging between running away from the first answer or the second one.
            float hue = Random.Range(0f, 1f);
            int loopSafeguard = 0;
            for(int j = 0; j < hues.Count; j++)
            {
                float diff = hue - hues[j];
                //Check if we're too close.
                if ((Mathf.Abs(diff) < answerHueTolerance || Mathf.Abs(diff) > (1 - answerHueTolerance)) && loopSafeguard < (1 / answerHueTolerance))
                {
                    //Collision: Bump the hue up by the tolerance value, set it between 0 and 1 again, and give it another go.
                    hue += answerHueTolerance;
                    if (hue > 1)
                    {
                        hue -= 1;
                    }
                    loopSafeguard++;
                    diff = hue - hues[j];
                    j = -1; //Gets incremented to 0 immediately
                }
            }
            hues.Add(hue);
            Color answerColor = Color.HSVToRGB(hue, answerColorSaturation, 1);
            //Hack -- Manually adjust saturation for darker "problem" color(s)
            //This ended up being any color with a hue above 0.6
            if (hue > 0.6)
            {
                answerColor = Color.HSVToRGB(hue, answerColorSaturation - 0.075f, 1);
            }
            newanswer.GetComponent<KeineMath_Answer>().answerColor = answerColor;
        }
    }

    public void processAnswer(int answer)
    {
        if (!answered)
        {
            //Vector3 answerPosition = answerObject.transform.position;
            answered = true;
            keineAnimator.GetComponent<SpriteRenderer>().enabled = true;
            keineAnimator.GetComponent<Animator>().SetBool("answerSelected", true);
            if (answer == correctAnswer)
            {
                MicrogameController.instance.setVictory(victory: true, final: true);
                keineAnimator.GetComponent<Animator>().SetBool("answerCorrect", true);
                cheeringCrowd.SetActive(true);
                MicrogameController.instance.playSFX(correctClip);
            }
            else
            {
                MicrogameController.instance.setVictory(victory: false, final: true);
                keineAnimator.GetComponent<Animator>().SetBool("answerCorrect", false);
                MicrogameController.instance.playSFX(incorrectClip);
            }
        }
    }
}
