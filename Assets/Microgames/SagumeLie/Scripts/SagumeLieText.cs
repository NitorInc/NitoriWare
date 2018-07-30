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
    private TMP_Text AnswerText1;
    [SerializeField]
    private TMP_Text AnswerText2;

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

        // TEMPORARY
        // select which slot the correct answer will be in
        // this is just to show it can be randomized
        // if there's a better way, i'd love to know!

        sagumeCorrectAnswer = Random.Range(0, 2);

        // placeholder answers

        if (sagumeCorrectAnswer == 1)
            {
             AnswerText1.SetText("Correct Answer!");
             AnswerText2.SetText("Incorrect Answer!");
            }

        else if (sagumeCorrectAnswer == 2)
            {
              AnswerText1.SetText("Incorrect Answer!");
              AnswerText2.SetText("Correct Answer!");
            }

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