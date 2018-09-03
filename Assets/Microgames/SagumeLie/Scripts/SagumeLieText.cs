using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


// not done:
// this is a placeholder!
// i need to actually get questions from list

// this was originally meant to just contain the script to change the "Question" and "Answer" text,
// but i kind of accidentally put everything else in here too.
// todo: split that up for convenience????

public class SagumeLieText : MonoBehaviour
{

    // texts 

    TMP_Text QuestionText;
    TMP_Text Answer1;
    TMP_Text Answer2;

    // setting correct answer to 0 before it's decided
    public int sagumeCorrectAnswer = 0;

    // objects to be animated
    Animator spriteDoremy;
    Animator spriteSagume;
    Animator spriteBackground;
    

    void Start()
    {
        // set animators
        spriteDoremy = GameObject.Find("Doremy").GetComponent<Animator>();
        spriteSagume = GameObject.Find("Sagume").GetComponent<Animator>();
        spriteBackground = GameObject.Find("Background").GetComponent<Animator>();



        // find the GameObjects we're going to be putting the text in
        // TODO: is there a 'better' way to do this?

        QuestionText = GameObject.Find("QuestionText").GetComponent<TMP_Text>();
        Answer1 = GameObject.Find("AnswerText1").GetComponent<TMP_Text>();
        Answer2 = GameObject.Find("AnswerText2").GetComponent<TMP_Text>();
     
        // placeholder question text
        // TODO: get question from the question list & randomize 

        QuestionText.SetText("This is a test question! It exists to show that I can change the text from the code. " +
        "Isn't that neat? I'm going to make it really long so that I can see what happens if the question is way too long.");

        // TEMPORARY
        // select which slot the correct answer will be in
        // this is just to show it can be randomized
        // if there's a better way, i'd love to know!

        sagumeCorrectAnswer = Random.Range(1, 3);

        // placeholder answers

        if (sagumeCorrectAnswer == 1)
            {
                Answer1.SetText("Correct Answer!");
                Answer2.SetText("Incorrect Answer!");
            }

        else if (sagumeCorrectAnswer == 2)
            {
                Answer1.SetText("Incorrect Answer!");
                Answer2.SetText("Correct Answer!");
            }

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
        GameObject.Find("QuestionBox").SetActive(false);

        if (sagumeCorrectAnswer == 1)
        {
            spriteSagume.SetBool("Success", true);
            spriteDoremy.SetBool("Success", true);
            spriteBackground.SetBool("Success", true);
            MicrogameController.instance.setVictory(victory: true, final: true);
        }
        else
        {            
            spriteSagume.SetBool("Failure", true);
            spriteDoremy.SetBool("Failure", true);
            spriteBackground.SetBool("Failure", true);
            MicrogameController.instance.setVictory(victory: false, final: true);
        }
    }

    // if button 2 is clicked

    public void AnswerCheckButton2()
    {
        GameObject.Find("QuestionBox").SetActive(false);

        if (sagumeCorrectAnswer == 2)
        {            
            spriteSagume.SetBool("Success", true);
            spriteDoremy.SetBool("Success", true);
            spriteBackground.SetBool("Success", true);
            MicrogameController.instance.setVictory(victory: true, final: true);
        }
        else
        {            
            spriteSagume.SetBool("Failure", true);
            spriteDoremy.SetBool("Failure",true);
            spriteBackground.SetBool("Failure", true);
            MicrogameController.instance.setVictory(victory: false, final: true);
        }

    }

}