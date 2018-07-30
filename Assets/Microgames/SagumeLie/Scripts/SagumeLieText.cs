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

        sagumeCorrectAnswer = Random.Range(1, 3);

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

    public void SagumeVictoryAnimation()
    {
        SagumeAnimator.SetBool("Success", true);
        DoremyAnimator.SetBool("Success", true);
        BackgroundAnimator.SetBool("Success", true);
        MicrogameController.instance.setVictory(victory: true, final: true);
    }

    public void SagumeFailureAnimation()
    {
        SagumeAnimator.SetBool("Failure", true);
        DoremyAnimator.SetBool("Failure", true);
        BackgroundAnimator.SetBool("Failure", true);
        MicrogameController.instance.setVictory(victory: false, final: true);
    }

    // that set up the question & answer text.
    // now, we check to see if the questions are answered.

    // TODO: see if there's a better way to do this?
    // it works but i feel there must be a more efficient way
    // both for checking if the button pressed is the correct one
    // and setting the animations for all the sprites to go off

    // this script is referenced in the "OnClick" of AnswerButton1 and AnswerButton2
    // it runs the relevant code below to check for answer correctness


    // if button 1 is clicked

    public void AnswerCheckButton1()
    {
        QuestionBox.SetActive(false);

        if (sagumeCorrectAnswer == 1)
            SagumeVictoryAnimation();
        else
        {
            SagumeFailureAnimation();
        }
    }

    // if button 2 is clicked

    public void AnswerCheckButton2()
    {
        QuestionBox.SetActive(false);

        if (sagumeCorrectAnswer == 2)
            SagumeVictoryAnimation();
        else
            SagumeFailureAnimation();

    }


}