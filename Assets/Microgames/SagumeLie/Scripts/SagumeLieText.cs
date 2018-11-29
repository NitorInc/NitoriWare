using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class SagumeLieText : MonoBehaviour
{
    public int liePosition;
    public int questionIndex;

    // these can be edited in the "ScriptController" GameObject

    [SerializeField]
    private TMP_Text QuestionText;
    [SerializeField]
    private TMP_Text Answer0;
    [SerializeField]
    private TMP_Text Answer1;
    [SerializeField]
    private TMP_Text Answer2;
    [SerializeField]
    private TMP_Text Answer3;

    string[] Answer = new string[4];

    void Start()
    {
        // get traits
        var traits = (SagumeLieTraits)MicrogameController.instance.getTraits();

        // questionIndex is a random question from the pool of all questions. QuestionText is set to this, so it'll be in the bubble.
        questionIndex = Random.Range(0, traits.QuestionPool.Length);
        QuestionText.SetText(traits.getLocalizedQuestionText(questionIndex));

        // set the number of maximum answers. should be changed based on difficulty. right now, it's 4 for testing.
        int maxAnswer = 4;

        // pick which position is the Lie. Might be better to have more than one lie option.
        liePosition = Random.Range(0, maxAnswer);

        // make a list of the Truths for the given question.
        List<string> unpickedTruths = new List<string>();
        for (int i = 0; i < traits.QuestionPool[questionIndex].TruthResponses.Length; i++)
        {
            unpickedTruths.Add(traits.getLocalizedResponseText(questionIndex, false, i));
        };

        // !!! DEBUG !!! if there's room for 3 truths, but we only have 2 on the list, it'll error.
        // this'll be fixed by either reducing answers, or making sure every question has 3 possible truths. 
        if (traits.QuestionPool[questionIndex].TruthResponses.Length < 3)
        { unpickedTruths.Add("Not enough answers for this question!");
        };

        // go through the number of Answers. if it's the Lie position, make it a Lie. if it's the Truth position, make it a Truth then remove it from the list.
        for (int i = 0; i < maxAnswer; i++)
        {
            if (i == liePosition)
                Answer[i] = traits.getLocalizedResponseText(questionIndex, true, Random.Range(0, traits.QuestionPool[questionIndex].LieResponses.Length));

            else
                Answer[i] = unpickedTruths[Random.Range(0, unpickedTruths.Count)];
                unpickedTruths.Remove(Answer[i]);
        }


        // set answers to what was picked
        Answer0.SetText(Answer[0]);
        Answer1.SetText(Answer[1]);
        Answer2.SetText(Answer[2]);
        Answer3.SetText(Answer[3]);

    }

    // victory and failure functions

    [SerializeField]
    private Animator DoremyAnimator;
    [SerializeField]
    private Animator SagumeAnimator;
    [SerializeField]
    private Animator BackgroundAnimator;
    [SerializeField]
    private GameObject QuestionBox;
    [SerializeField]
    private GameObject Button0;
    [SerializeField]
    private GameObject Button1;
    [SerializeField]
    private GameObject Button2;
    [SerializeField]
    private GameObject Button3;

    public void SagumeVictory()
    {
        SagumeAnimator.SetBool("Success", true);
        DoremyAnimator.SetBool("Success", true);
        BackgroundAnimator.SetBool("Success", true);
        MicrogameController.instance.setVictory(victory: true, final: true);
    }

    public void SagumeFailure()
    {
        SagumeAnimator.SetBool("Failure", true);
        DoremyAnimator.SetBool("Failure", true);
        BackgroundAnimator.SetBool("Failure", true);
        MicrogameController.instance.setVictory(victory: false, final: true);
    }

    // answer check function
    // the script is referenced in the "OnClick" of the answer buttons.
    // (note: add script to buttons by dragging "ScriptController" GameObject)


    public void AnswerCheck(int answerButtonClicked)
    {
        QuestionBox.SetActive(false);
        Button0.SetActive(false);
        Button1.SetActive(false);
        Button2.SetActive(false);
        Button3.SetActive(false);

        if (answerButtonClicked == liePosition)
            SagumeVictory();
        else
            SagumeFailure();
    }
}