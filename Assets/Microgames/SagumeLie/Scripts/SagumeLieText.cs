using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class SagumeLieText : MonoBehaviour
{
    public int correctAnswerPosition;
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
        var traits = (SagumeLieTraits)MicrogameController.instance.getTraits();

        questionIndex = Random.Range(0, traits.QuestionPool.Length);

        QuestionText.SetText(traits.getLocalizedQuestionText(questionIndex));

        // set number of maximum answers
        // this is prep for higher difficulty; 4 for testing purposes

        int maxAnswer = 4;

        // find the answers
        // note: can have duplicate answers right now. find fix??

        correctAnswerPosition = Random.Range(0, maxAnswer);

        for (int i = 0; i < maxAnswer; i++)
        {
            if (i == correctAnswerPosition)
                Answer[i] = traits.getLocalizedResponseText(questionIndex, false, Random.Range(0, traits.QuestionPool[questionIndex].TruthResponses.Length));

            else
                Answer[i] = traits.getLocalizedResponseText(questionIndex, true, Random.Range(0, traits.QuestionPool[questionIndex].LieResponses.Length));
        }

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

        if (answerButtonClicked == correctAnswerPosition)
            SagumeVictory();
        else
            SagumeFailure();
    }

}