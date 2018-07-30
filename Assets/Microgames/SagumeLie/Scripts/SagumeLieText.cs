using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class SagumeLieText : MonoBehaviour
{
    // setting correct answer to 0 before it's decided
    public int sagumeCorrectAnswer;
    public int sagumeQuestionID;


    // use SerializeField to reference GameObjects that will be used in the script
    // these can be edited in the "ScriptController" GameObject

    [SerializeField]
    private Animator DoremyAnimator;
    [SerializeField]
    private Animator SagumeAnimator;
    [SerializeField]
    private Animator BackgroundAnimator;

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

    [SerializeField]
    private GameObject QuestionBox;

    void Start()
    {
        // import traits

        var traits = (SagumeLieTraits)MicrogameController.instance.getTraits();

        // randomize question - currently 2 options; edit later.
        sagumeQuestionID = Random.Range(0, 2);

        // set question text to the randomized question
        QuestionText.SetText(traits.getLocalizedQuestionText(sagumeQuestionID));

        // set number of maximum answers
        // this is prep for higher difficulty; 4 for testing purposes
        // then, randomize position of correct answer

        int maxAnswer = 4;

        sagumeCorrectAnswer = Random.Range(0, maxAnswer);

        // randomize correct answer text
        int correctAnswerID = Random.Range(0, 2);

        Answer0.SetText(traits.getLocalizedResponseText(sagumeQuestionID, false, 1));




    }

    // victory and failure conditions

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

    // when buttons are clicked, determine if it's the right answer or not.
    // the script is referenced in the "OnClick" of the answer buttons.
    // (note: add script to buttons by dragging "ScriptController" GameObject)


    public void AnswerCheck(int answerButtonClicked)
    {
        QuestionBox.SetActive(false);

        if (answerButtonClicked == sagumeCorrectAnswer)
            SagumeVictory();
        else
            SagumeFailure();
    }

}