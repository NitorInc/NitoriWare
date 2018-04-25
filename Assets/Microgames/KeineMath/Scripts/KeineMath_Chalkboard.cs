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

    private GameObject keine;
    private GameObject keineAnimator;
    private GameObject cheeringCrowd;
    private GameObject correctSymbol;
    private GameObject incorrectSymbol;
    private GameObject hundredSymbol;

    //private GameObject youtried; //kek

    // Use this for initialization
    void Start () {
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

        //Setting up Keine, the correct/incorrect symbols, and the cheering crowd.
        //They stay invisible or offscreen until an answer is chosen.
        keine = GameObject.Find("Keine");
        keineAnimator = keine.transform.Find("Keine_Rig").gameObject;
        keineAnimator.GetComponent<SpriteRenderer>().enabled = false;
        cheeringCrowd = GameObject.Find("Doodle_Cheering_Crowd");
        cheeringCrowd.SetActive(false);
        correctSymbol = GameObject.Find("Doodle_Correct");
        incorrectSymbol = GameObject.Find("Doodle_Incorrect");
        hundredSymbol = GameObject.Find("Doodle_Hundred");

        //youtried = GameObject.Find("youtried"); //kek

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
                if (termList[i] != 1) allones = false;
            }
            //We don't want all 1s because it ruins the answer-generating algorithm and also is boring.
            if (allones) termList[Random.Range(0, termCount)] += 1;
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
            if (correctAnswer == 1) sign = 1; //Answers of zero are not permitted
            answerOffsets.Add(i * sign); //First wrong answer is off by 1, second wrong answer is off by 2
        }
        for(int i = 1; i <= answerCount; i++)
        {
            //Pick a random offset, remove it, and apply it to the answer being generated
            answerOffset = answerOffsets[Random.Range(0, answerOffsets.Count)];
            answerOffsets.Remove(answerOffset);
            answerValue = correctAnswer + answerOffset;
            float newx = answer.transform.position.x + (3.7f * i) + 1f; //Values in here determine answer positioning
            float newy = answer.transform.position.y;
            Vector3 newposition = new Vector3(newx, newy, 0);
            GameObject newanswer = Object.Instantiate(answer, newposition, Quaternion.identity);
            newanswer.GetComponent<KeineMath_Answer>().value = answerValue;
            if (answerOffset == 0) newanswer.GetComponent<KeineMath_Answer>().isCorrect = true;

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
                    if (hue > 1) hue -= 1;
                    loopSafeguard++;
                    diff = hue - hues[j];
                    j = -1; //Gets incremented to 0 immediately
                }
            }
            hues.Add(hue);
            Color answerColor = Color.HSVToRGB(hue, answerColorSaturation, 1);
            //Hack -- Manually adjust saturation for darker "problem" color(s)
            //This ended up being any color with a hue above 0.6
            if (hue > 0.6) answerColor = Color.HSVToRGB(hue, answerColorSaturation - 0.075f, 1);
            newanswer.GetComponent<KeineMath_Answer>().answerColor = answerColor;
        }
    }

    public void processAnswer(int answer, GameObject answerObject)
    {
        if (!answered)
        {
            Vector3 answerPosition = answerObject.transform.position;
            answered = true;
            keineAnimator.GetComponent<SpriteRenderer>().enabled = true;
            keineAnimator.GetComponent<Animator>().SetBool("answerSelected", true);
            if (answer == correctAnswer)
            {
                MicrogameController.instance.setVictory(victory: true, final: true);
                keineAnimator.GetComponent<Animator>().SetBool("answerCorrect", true);
                cheeringCrowd.SetActive(true);
                //cheeringCrowd.transform.position = new Vector3(answerPosition.x, cheeringCrowd.transform.position.y, 0);
                //StartCoroutine(AnimateAnswerSymbol(correctSymbol, answerPosition, 0));
                StartCoroutine(AnimateAnswerSymbol(hundredSymbol, new Vector3(answerPosition.x, answerPosition.y - 2.4f, 0), 0));
                //StartCoroutine(SetAnswerBG(new Color(0.6f, 1f, 0.6f, 0.38f), answerObject));
                MicrogameController.instance.playSFX(correctClip);
            }
            else
            {
                MicrogameController.instance.setVictory(victory: false, final: true);
                keineAnimator.GetComponent<Animator>().SetBool("answerCorrect", false);
                StartCoroutine(AnimateAnswerSymbol(incorrectSymbol, answerPosition, 0));
                //StartCoroutine(SetAnswerBG(new Color(1f, 0.7f, 0.7f, 0.38f), answerObject));
                MicrogameController.instance.playSFX(incorrectClip);
            }
        }
    }

    IEnumerator AnimateAnswerSymbol(GameObject symbol, Vector3 answerPosition, int mode = 0)
    {
        //Takes a symbol to be placed over the answer box and does so.
        //Additionally, an animation mode may be specified which will make this process more interesting.
        symbol.transform.position = answerPosition;
        int i;
        switch(mode)
        {
            case 1:
                //Flicker the symbol a couple times
                int flickerFrames = 5;
                for (i = 0; i < flickerFrames; i++) yield return null;
                symbol.SetActive(false);
                for (i = 0; i < flickerFrames; i++) yield return null;
                symbol.SetActive(true);
                for (i = 0; i < flickerFrames; i++) yield return null;
                symbol.SetActive(false);
                for (i = 0; i < flickerFrames; i++) yield return null;
                symbol.SetActive(true);
                break;
            case 2:
                //Have the symbol fade into position
                Color symbolColor = symbol.GetComponent<SpriteRenderer>().color;
                float initialXScale = symbol.transform.localScale.x;
                float initialYScale = symbol.transform.localScale.x;
                float initialSizeIncrease = 1.5f;
                float sizeIncrease = initialSizeIncrease;
                float alpha = 0;
                symbol.transform.localScale = new Vector3(initialXScale * sizeIncrease, initialYScale * sizeIncrease, symbol.transform.localScale.z);
                symbolColor.a = alpha;
                symbol.GetComponent<SpriteRenderer>().color = symbolColor;
                for (i = 1; i <= 20; i++)
                {
                    float currentFactor = (i * i) / 400f; //When this hits 1, we're done.
                    sizeIncrease = ((initialSizeIncrease - 1) * (1 - currentFactor)) + 1;
                    alpha = currentFactor;
                    symbol.transform.localScale = new Vector3(initialXScale * sizeIncrease, initialYScale * sizeIncrease, symbol.transform.localScale.z);
                    symbolColor.a = alpha;
                    symbol.GetComponent<SpriteRenderer>().color = symbolColor;
                    yield return null;
                }
                break;
            default:
                break;
        }
    }

    IEnumerator SetAnswerBG(Color newColor, GameObject answer)
    {
        Color baseColor = answer.transform.Find("AnswerBG").gameObject.GetComponent<SpriteRenderer>().color;
        //Color baseBorderColor = answer.GetComponent<SpriteRenderer>().color;
        answer.transform.Find("AnswerBG").gameObject.GetComponent<SpriteRenderer>().color = newColor;
        //answer.GetComponent<SpriteRenderer>().color = newColor;
        int i = 0;
        int flickerFrames = 7;
        for (i = 0; i < flickerFrames; i++) yield return null;
        answer.transform.Find("AnswerBG").gameObject.GetComponent<SpriteRenderer>().color = baseColor;
        //answer.GetComponent<SpriteRenderer>().color = baseBorderColor;
        for (i = 0; i < flickerFrames; i++) yield return null;
        answer.transform.Find("AnswerBG").gameObject.GetComponent<SpriteRenderer>().color = newColor;
        //answer.GetComponent<SpriteRenderer>().color = newColor;
        for (i = 0; i < flickerFrames; i++) yield return null;
        answer.transform.Find("AnswerBG").gameObject.GetComponent<SpriteRenderer>().color = baseColor;
        //answer.GetComponent<SpriteRenderer>().color = baseBorderColor;
        for (i = 0; i < flickerFrames; i++) yield return null;
        answer.transform.Find("AnswerBG").gameObject.GetComponent<SpriteRenderer>().color = newColor;
    }
}
